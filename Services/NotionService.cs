using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using static Gerador_PDF.Services.readExcel;

namespace Gerador_PDF.Services
{
	public class NotionService
    {
        private const string dataBaseUrl = "https://api.notion.com/v1/databases/";
        private readonly string _token;
        private readonly string _dataBaseId;
		private readonly string _dataBaseIdKPI;
		private readonly string _dataBaseIdLoja;
		private static readonly HttpClient _client = new HttpClient();
		private readonly Dictionary<string, (string Nome, string Cod, string Loja)> _relatedCache = new();
		public NotionService(string token, string databaseId, string dataBaseIdKPI, string dataBaseIdLojas)
        {
			_token = token;
            _dataBaseId = databaseId;
			_dataBaseIdKPI = dataBaseIdKPI;
			_dataBaseIdLoja = dataBaseIdLojas;
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

		public async Task DeleteMonth(string mes)
		{
			var urlQuery = $"https://api.notion.com/v1/databases/{_dataBaseIdKPI}/query";

			var filtro = new
			{
				filter = new
				{
					property = "Mês",
					multi_select = new { contains = mes }
				}
			};

			var request = new HttpRequestMessage(HttpMethod.Post, urlQuery)
			{
				Content = new StringContent(JsonConvert.SerializeObject(filtro), Encoding.UTF8, "application/json")
			};

			var response = await _client.SendAsync(request);
			var json = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine($"❌ Erro ao buscar páginas: {response.StatusCode}");
				Console.WriteLine(json);
				return;
			}

			var resultado = JObject.Parse(json);
			var paginas = resultado["results"] as JArray;

			if (paginas == null || paginas.Count == 0)
			{
				Console.WriteLine($"Nenhuma página encontrada para o mês '{mes}'");
				return;
			}

			foreach (var pagina in paginas)
			{
				string pageId = pagina["id"]?.ToString();
				if (!string.IsNullOrEmpty(pageId))
				{
					var deleteRequest = new HttpRequestMessage(HttpMethod.Patch, $"https://api.notion.com/v1/pages/{pageId}")
					{
						Content = new StringContent("{\"archived\":true}", Encoding.UTF8, "application/json")
					};
					await _client.SendAsync(deleteRequest);
				}
			}
		}

		public async Task<string> GetLojaUUID(string lojaNome)
		{
			var url = $"https://api.notion.com/v1/databases/{_dataBaseIdLoja}/query";

			var query = new
			{
				filter = new
				{
					property = "Loja",
					title = new { equals = lojaNome }
				}
			};

			var request = new HttpRequestMessage(HttpMethod.Post, url)
			{
				Content = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json")
			};

			var response = await _client.SendAsync(request);
			var responseContent = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine($"Erro ao buscar UUID da loja {lojaNome}: {response.StatusCode}");
				Console.WriteLine($"Detalhes do erro: {responseContent}");
				return null;
			}

			var jsonResponse = JObject.Parse(responseContent);
			var results = jsonResponse["results"] as JArray;

			return results?.FirstOrDefault()?["id"]?.ToString();
		}

		public async Task updateNPs(List<monthStoreData> monthStores, string mes)
		{
			var databaseId = _dataBaseIdKPI;
			var url = "https://api.notion.com/v1/pages";

			var todasLojas = monthStores.Select(f => f.loja)
				.Distinct()
				.ToList();

			Console.WriteLine($"🔍 Lojas encontradas: {string.Join(", ", todasLojas)}");

			var lojaUUIDMap = new Dictionary<string, string>();
			foreach (var loja in todasLojas)
			{
				var uuid = await GetLojaUUID(loja);
				if (!string.IsNullOrEmpty(uuid))
					lojaUUIDMap[loja] = uuid;

			}

			var monthStoresMap = monthStores
				.GroupBy(f => f.loja)
				.ToDictionary(g => g.Key, g => g.First());

			foreach (var loja in todasLojas)
			{
				if (!lojaUUIDMap.TryGetValue(loja, out var lojaUUID))
				{
					continue;
				}

				monthStoresMap.TryGetValue(loja, out var monthStore);

				string existingPageId = await BuscarPaginaExistente(databaseId, lojaUUID, mes);
				if (existingPageId != null)
				{
					var properties = new Dictionary<string, object>
					{
						["NPS"] = new { number = monthStore.NPS }
					};

					string patchUrl = $"https://api.notion.com/v1/pages/{existingPageId}";
					var updateRequest = new HttpRequestMessage(HttpMethod.Patch, patchUrl)
					{
						Content = new StringContent(JsonConvert.SerializeObject(new { properties }), Encoding.UTF8, "application/json")
					};

					var updateResponse = await _client.SendAsync(updateRequest);
					var updateContent = await updateResponse.Content.ReadAsStringAsync();

				}
			}
		}

		public async Task UpdateNotionDatabase(List<FaturadoData> faturados, List<ComplementarData> complementares, List<monthStoreData> monthStores, string mes)
		{
			var databaseId = _dataBaseIdKPI;
			var url = "https://api.notion.com/v1/pages";

			if (monthStores != null && monthStores.Count > 0)
			{
				await updateNPs(monthStores, mes);
				return;
			}
			var todasLojas = faturados.Select(f => f.loja)
				.Union(complementares.Select(c => c.lojas))
				.Distinct()
				.ToList();
			var lojaUUIDMap = new Dictionary<string, string>();
			foreach (var loja in todasLojas)
			{
				var uuid = await GetLojaUUID(loja);
				if (!string.IsNullOrEmpty(uuid))
				{
					lojaUUIDMap[loja] = uuid;
				}
			}

			var faturadosMap = faturados
			.GroupBy(f => f.loja)
			.ToDictionary(g => g.Key, g => g.First());

			var complementaresMap = complementares
			.GroupBy(c => c.lojas)
			.ToDictionary(g => g.Key, g => g.First());

			foreach (var loja in todasLojas)
			{
				if (!lojaUUIDMap.TryGetValue(loja, out var lojaUUID))
				{
					continue;
				}

				faturadosMap.TryGetValue(loja, out var faturado);
				complementaresMap.TryGetValue(loja, out var complementar);

				var properties = new Dictionary<string, object>
				{
					["Loja"] = new { relation = new[] { new { id = lojaUUID } } },
					["Mês"] = new { multi_select = new[] { new { name = mes } } }
				};

				if (faturado != null)
				{
					properties["Faturados"] = new { number = faturado.faturados };
					properties["FTE"] = new { number = faturado.fte };
					properties["Obj.Dia"] = new { number = faturado.objAoDia };
					properties["Obj.Mes"] = new { number = faturado.objMes };
					properties["TX REP %"] = new { number = faturado.taxRep };
					properties["Qtd Reparações"] = new { number = faturado.qntRep };
				}

				if (complementar != null)
				{
					properties["VAPS"] = new { number = complementar.vaps };
					properties["QTD Escovas"] = new { number = complementar.escovas };
					properties["Escovas %"] = new { number = complementar.escovasPercent };
					properties["Polimentos"] = new { number = complementar.polimento };
				}

				string existingPageId = await BuscarPaginaExistente(databaseId, lojaUUID, mes);

				if (existingPageId != null)
				{
					string patchUrl = $"https://api.notion.com/v1/pages/{existingPageId}";
					var updateRequest = new HttpRequestMessage(HttpMethod.Patch, patchUrl)
					{
						Content = new StringContent(JsonConvert.SerializeObject(new { properties }), Encoding.UTF8, "application/json")
					};

					var updateResponse = await _client.SendAsync(updateRequest);
					var updateContent = await updateResponse.Content.ReadAsStringAsync();

					if (!updateResponse.IsSuccessStatusCode)
					{
						Console.WriteLine($"⚠️ Erro ao atualizar página: {updateContent}");
					}
				}
				else
				{
					var createBody = new
					{
						parent = new { database_id = databaseId },
						properties
					};

					var request = new HttpRequestMessage(HttpMethod.Post, url)
					{
						Content = new StringContent(JsonConvert.SerializeObject(createBody), Encoding.UTF8, "application/json")
					};

					var response = await _client.SendAsync(request);
					var responseContent = await response.Content.ReadAsStringAsync();

					if (!response.IsSuccessStatusCode)
					{
						Console.WriteLine($"⚠️ Erro ao criar página: {responseContent}");
					}
				}
			}
		}

		private async Task<string?> BuscarPaginaExistente(string databaseId, string lojaId, string mes)
		{
			var filter = new
			{
				filter = new
				{
					and = new object[]
					{
						new
						{
							property = "Loja",
							relation = new { contains = lojaId }
						},
						new
						{
							property = "Mês",
							multi_select  = new { contains  = mes }
						}
					}
				}
			};

			string json = JsonConvert.SerializeObject(filter);
			var response = await _client.PostAsync(
				"https://api.notion.com/v1/databases/" + databaseId + "/query",
				new StringContent(json, Encoding.UTF8, "application/json")
			);

			var content = await response.Content.ReadAsStringAsync();
			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine($"🔎 Falha ao buscar página existente: {response.StatusCode}\n{content}");
				return null;
			}

			dynamic resultado = JsonConvert.DeserializeObject(content);
			if (resultado.results.Count > 0)
			{
				return resultado.results[0].id;
			}

			return null;
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
				new StringContent(JsonConvert.SerializeObject(searchBody), Encoding.UTF8, "application/json")
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

				string jsonUpdate = JsonConvert.SerializeObject(updateBody, Formatting.Indented);
				Console.WriteLine($"Atualizando página {pageId} com JSON:\n{jsonUpdate}");

				var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"https://api.notion.com/v1/pages/{pageId}")
				{
					Content = new StringContent(JsonConvert.SerializeObject(updateBody), Encoding.UTF8, "application/json")
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
					var responseBody = await response.Content.ReadAsStringAsync();

					string reason;
					switch (response.StatusCode)
					{
						case HttpStatusCode.Unauthorized:
							reason = "Token da API inválido ou expirado.";
							break;
						case HttpStatusCode.Forbidden:
							reason = "Acesso negado. Verifique se o token tem permissão para acessar este Database ID.";
							break;
						case HttpStatusCode.NotFound:
							reason = "Database ID não encontrado ou não existe.";
							break;
						default:
							reason = $"Erro desconhecido: {response.StatusCode}";
							break;
					}
					throw new Exception($"{reason}\n");
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
