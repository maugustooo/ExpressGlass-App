using RestSharp;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Diagnostics;

namespace Gerador_ecxel
{
	public class NotionService
    {
        private const string dataBaseUrl = "https://api.notion.com/v1/databases/";
        private readonly string _token;
        private readonly string _dataBaseId;
		private static readonly HttpClient _client = new HttpClient();
		private readonly Dictionary<string, (string Nome, string Cod, string Loja)> _relatedCache = new();
		public NotionService(string token, string databaseId)
        {
			_token = token;
            _dataBaseId = databaseId;
			_client.DefaultRequestHeaders.Clear();
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			_client.DefaultRequestHeaders.Add("Notion-Version", "2022-06-28");
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		private async Task<(string Nome, string Cod, string Loja)> GetNameAndIdCached(string idRelacionado)
		{
			if (_relatedCache.ContainsKey(idRelacionado))
				return _relatedCache[idRelacionado];

			var result = await getNameAndId(idRelacionado, _token);
			_relatedCache[idRelacionado] = result;

			return result;
		}

		public async Task UpdateStatus()
		{
			var searchBody = new
			{
				filter = new
				{
					property = "Status",
					status = new
					{
						equals = "Novo"
					}
				}
			};
			if (string.IsNullOrEmpty(dataBaseUrl) || string.IsNullOrEmpty(_dataBaseId))
			{
				MessageBox.Show("Erro: URL do banco de dados ou ID não estão definidos.");
				return;
			}
			var response = await _client.PostAsync(
				$"{dataBaseUrl}{_dataBaseId}/query",
				new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(searchBody), Encoding.UTF8, "application/json")
			);
			if (!response.IsSuccessStatusCode)
			{
				MessageBox.Show($"Erro ao buscar páginas: {response.StatusCode}");
				return;
			}
			var jsonResponse = await response.Content.ReadAsStringAsync();
			var jsonObject = JObject.Parse(jsonResponse);

			if (!jsonObject.ContainsKey("results") || jsonObject["results"] == null)
			{
				MessageBox.Show("Erro: resposta inesperada do Notion.");
				return;
			}
			var pages = JObject.Parse(jsonResponse)["results"];

			if (pages == null || !pages.HasValues)
			{
				MessageBox.Show("Nenhuma página com status 'Novo' encontrada.");
				return;
			}

			foreach (var page in pages)
			{
				var pageId = page["id"]?.ToString();
				if (string.IsNullOrEmpty(pageId)) continue;

				var updateBody = new
				{
					properties = new
					{
						Status = new
						{
							status = new
							{
								name = "Concluído"
							}
						}
					}
				};

				string jsonUpdate = Newtonsoft.Json.JsonConvert.SerializeObject(updateBody, Formatting.Indented);
				Console.WriteLine($"Atualizando página {pageId} com JSON:\n{jsonUpdate}");

				var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"https://api.notion.com/v1/pages/{pageId}")
				{
					Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(updateBody), Encoding.UTF8, "application/json")
				};

				var updateResponse = await _client.SendAsync(request);
				var responseContent = await response.Content.ReadAsStringAsync();

				if (!updateResponse.IsSuccessStatusCode)
				{
					MessageBox.Show($"Erro ao atualizar a página {pageId}: {updateResponse.StatusCode}");
				}
			}
			MessageBox.Show("Todos os status 'Novo' foram atualizados para 'Concluído'!");
		}


		private async Task<string> getLoja(string pageId, string notionApiKey)
		{

			string url = $"https://api.notion.com/v1/pages/{pageId}";
			HttpResponseMessage response = await _client.GetAsync(url);

			if (response.IsSuccessStatusCode)
			{
				string jsonResponse = await response.Content.ReadAsStringAsync();
				var pageData = JsonNode.Parse(jsonResponse);

				var properties = pageData?["properties"]?.AsObject();
				if (properties != null)
				{
					foreach (var prop in properties)
					{
						var titleField = prop.Value?["title"]?.AsArray();
						if (titleField != null && titleField.Count > 0)
							return titleField[0]?["text"]?["content"]?.ToString() ?? "Sem Loja";
					}
				}
			}
			return "Sem Loja";
		}

		private async Task<(string Nome, string Cod, string Loja)> getNameAndId(string idRelacionado, string notionApiKey)
		{
			string url = $"https://api.notion.com/v1/pages/{idRelacionado}";
			HttpResponseMessage response = await _client.GetAsync(url);

			if (response.IsSuccessStatusCode)
			{
				string jsonResponse = await response.Content.ReadAsStringAsync();
				var pageData = JsonNode.Parse(jsonResponse);

				var properties = pageData?["properties"]?.AsObject();
				if (properties != null)
				{
					string titulo = "Sem Nome";
					string cod = "Sem ID";
					string loja = "Sem Loja";

					foreach (var prop in properties)
					{
						if (prop.Value?["title"] != null)
						{
							var titleField = prop.Value["title"]?.AsArray();
							if (titleField != null && titleField.Count > 0)
								titulo = titleField[0]?["text"]?["content"]?.ToString() ?? "Sem Nome";
						}
						if (prop.Key == "Cod" && prop.Value?["number"] != null)
							cod = prop.Value["number"]?.ToString() ?? "Sem ID";
						if (prop.Key == "Lojas" && prop.Value?["relation"] != null)
						{
							var lojasArray = prop.Value["relation"]?.AsArray();
							if (lojasArray != null && lojasArray.Count > 0)
							{
								string lojaId = lojasArray[0]?["id"]?.ToString() ?? "";
								if (!string.IsNullOrEmpty(lojaId))
									loja = await getLoja(lojaId, notionApiKey);
							}
						}
					}
					return (titulo, cod, loja);
				}
			}
			return ("Sem Nome", "Sem ID", "Sem Loja");
		}


		public async Task<List<string[]>> GetDatabase()
		{
			var entries = new List<string[]>();
			string? nextCursor = "";
			do
			{
				var requestBody = new Dictionary<string, object>
				{
					{ "page_size", 100 }
				};

				if (!string.IsNullOrEmpty(nextCursor))
				{
					requestBody.Add("start_cursor", nextCursor);
				}
				var jsonBody = JsonConvert.SerializeObject(requestBody);

				var request = new HttpRequestMessage(HttpMethod.Post, $"{dataBaseUrl}{_dataBaseId}/query")
				{
					Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
				};

				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
				request.Headers.Add("Notion-Version", "2022-06-28");

				var response = await _client.SendAsync(request);
				if (!response.IsSuccessStatusCode)
				{
					throw new Exception($"Failed to get database: {response.StatusCode}");
				}

				var content = JObject.Parse(await response.Content.ReadAsStringAsync());
				var results = content["results"];

				foreach (var result in results)
				{
					try
					{
						var nomeRelacoes = result["properties"]?["Nome"]?["relation"]?.ToObject<List<Dictionary<string, object>>>();
						if (nomeRelacoes == null || nomeRelacoes.Count == 0)
							continue;

						var primeiroItem = nomeRelacoes.First();
						string? idRelacionado = primeiroItem["id"]?.ToString();

						string name = "Sem Nome";
						string id = "Sem ID";
						string loja = "Sem Loja";

						if (!string.IsNullOrEmpty(idRelacionado))
						{
							var (nome, cod, lojas) = await GetNameAndIdCached(idRelacionado);
							name = nome;
							id = cod;
							loja = lojas;
						}

						var data = result["properties"]?["Data"]?["date"]?["start"]?.ToString() ?? "";
						var matricula = result["properties"]?["Matrícula"]?["rich_text"]?.First?["text"]?["content"]?.ToString() ?? "";
						var localidade = result["properties"]?["Localidade"]?["rich_text"]?.First?["text"]?["content"]?.ToString() ?? "";
						var motivo = result["properties"]?["Motivo"]?["rich_text"]?.First?["text"]?["content"]?.ToString() ?? "";
						var klm = result["properties"]?["KLM"]?["number"]?.ToString() ?? "0";
						var status = result["properties"]?["Status"]?["status"]?["name"]?.ToString() ?? "";

						entries.Add(new string[] { data, name, matricula, localidade, motivo, klm, status, id, loja });
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Erro ao processar resultado: {ex.Message}");
					}
				}

				nextCursor = content["next_cursor"]?.ToString();

			} while (!string.IsNullOrEmpty(nextCursor));

			return entries;
		}
	}
}
