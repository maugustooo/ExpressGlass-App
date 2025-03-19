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
        public NotionService(string token, string databaseId)
        {
			_token = token;
            _dataBaseId = databaseId;
        }

		public async Task UpdateStatus()
		{
			var client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
			client.DefaultRequestHeaders.Add("Notion-Version", "2022-06-28");
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
			var response = await client.PostAsync(
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

				var updateResponse = await client.SendAsync(request);
				var responseContent = await updateResponse.Content.ReadAsStringAsync();
				Console.WriteLine($"Resposta da API do Notion:\n{responseContent}");
				Console.WriteLine($"Atualizando página {pageId} com JSON:\n{jsonUpdate}");

				if (!updateResponse.IsSuccessStatusCode)
				{
					MessageBox.Show($"Erro ao atualizar a página {pageId}: {updateResponse.StatusCode}");
				}
			}
			MessageBox.Show("Todos os status 'Novo' foram atualizados para 'Concluído'!");
		}


		private async Task<string> getLoja(string pageId, string notionApiKey)
		{
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("Authorization", $"Bearer {notionApiKey}");
				client.DefaultRequestHeaders.Add("Notion-Version", "2022-06-28");

				string url = $"https://api.notion.com/v1/pages/{pageId}";
				HttpResponseMessage response = await client.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					string jsonResponse = await response.Content.ReadAsStringAsync();
					var pageData = JsonNode.Parse(jsonResponse);

					var properties = pageData["properties"]?.AsObject();
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
			}
			return "Sem Loja";
		}

		private async Task<(string Nome, string Cod, string Loja)> getNameAndId(string idRelacionado, string notionApiKey)
		{
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("Authorization", $"Bearer {notionApiKey}");
				client.DefaultRequestHeaders.Add("Notion-Version", "2022-06-28"); // Versão correta da API

				string url = $"https://api.notion.com/v1/pages/{idRelacionado}";
				HttpResponseMessage response = await client.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					string jsonResponse = await response.Content.ReadAsStringAsync();
					var pageData = JsonNode.Parse(jsonResponse);

					var properties = pageData["properties"]?.AsObject();
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
			}

			return ("Sem Nome", "Sem ID", "Sem Loja");
		}


		public async Task<List<string[]>> GetDatabase()
        {
            var client = new RestClient($"{dataBaseUrl}{_dataBaseId}/query");
            var request = new RestRequest();
            request.Method = Method.Post;

            request.AddHeader("Authorization", "Bearer " + _token);
            request.AddHeader("Notion-Version", "2022-06-28");
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new { });

            var response = await client.ExecuteAsync(request);
            var entries = new List<string[]>();
            if (response.IsSuccessful)
            {
                var content = JObject.Parse(response.Content);
                var results = content["results"];
                foreach (var result in results)
                {
                    try
                    {
                        var nomeRelacoes = result["properties"]?["Nome"]?["relation"]?.ToObject<List<Dictionary<string, object>>>();
                        if (nomeRelacoes == null || nomeRelacoes.Count == 0)
                        {
                            continue;
                        }
                        var primeiroItem = nomeRelacoes.First();
                        string? idRelacionado = primeiroItem["id"]?.ToString();
                        string name = "Sem Nome";
						string id = "Sem ID";
						string loja = "Sem Loja";
						if (idRelacionado != null)
                        {
                            var (nome, cod, lojas) = await getNameAndId(idRelacionado, _token);
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
                        entries.Add(new string[] { data, name, matricula, localidade, motivo, klm, status, id, loja});
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to get title: {ex.Message}");
                    }

                }
            }
            else
            {
                throw new Exception("Failed to get database");
            }
            return entries;
        }
    }
}
