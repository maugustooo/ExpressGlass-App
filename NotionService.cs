using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Gerador_ecxel
{
    public class NotionService
    {
        private const string dataBaseUrl = "https://api.notion.com/v1/databases/";
        private readonly string Token;
        private readonly string DatabaseId;
        public NotionService(string token, string databaseId)
        {
            Token = token;
            DatabaseId = databaseId;
        }

        public async Task<string> getName(string idRelacionado, string notionApiKey)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {notionApiKey}");
                client.DefaultRequestHeaders.Add("Notion-Version", "2022-06-28");  // Use a versão correta da API

                string url = $"https://api.notion.com/v1/pages/{idRelacionado}";
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var pageData = JsonNode.Parse(jsonResponse);

                    var properties = pageData["properties"]?.AsObject();
                    foreach (var prop in properties)
                    {
                        var titleField = prop.Value?["title"]?.AsArray();
                        if (titleField != null && titleField.Count > 0)
                        {
                            string titulo = titleField[0]?["text"]?["content"]?.ToString() ?? "Sem Nome";
                            return titulo;
                        }
                    }
                }
            }
            return "Sem Nome";
        }

        public async Task<List<string[]>> GetDatabase()
        {
            var client = new RestClient($"{dataBaseUrl}{DatabaseId}/query");
            var request = new RestRequest();
            request.Method = Method.Post;

            request.AddHeader("Authorization", "Bearer " + Token);
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
                        var nome = (string?)null;
                        if (idRelacionado != null)
                            nome = await getName(idRelacionado, Token);
                        var data = result["properties"]?["Data"]?["date"]?["start"]?.ToString() ?? "";
                        var matricula = result["properties"]?["Matrícula"]?["rich_text"]?.First?["text"]?["content"]?.ToString() ?? "";
                        var localidade = result["properties"]?["Localidade"]?["rich_text"]?.First?["text"]?["content"]?.ToString() ?? "";
                        var motivo = result["properties"]?["Motivo"]?["rich_text"]?.First?["text"]?["content"]?.ToString() ?? "";
                        var klm = result["properties"]?["KLM"]?["number"]?.ToString() ?? "0";
                        var status = result["properties"]?["Status"]?["status"]?["name"]?.ToString() ?? "";
                        entries.Add(new string[] { data, nome, matricula, localidade, motivo, klm, status });
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
