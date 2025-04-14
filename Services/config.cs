using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

public class Config
{
	[JsonProperty("NotionApiKey")]
	public string NotionApiKey { get; set; }

	[JsonProperty("NotionDatabaseId")]
	public string NotionDatabaseId { get; set; }

	[JsonProperty("NotionDatabaseIdKPIs")]
	public string NotionDatabaseIdKPIs { get; set; }

	[JsonProperty("NotionDatabaseIdLojas")]
	public string NotionDatabaseIdLojas { get; set; }
}

public static class NotionConfigHelper
{
	private static string configFilePath = Path.Combine(Application.StartupPath, "notion_config.json");

	public static Config GetConfig()
	{
		var apiKey = Environment.GetEnvironmentVariable("NOTION_API_KEY", EnvironmentVariableTarget.Machine);
		var dbId = "";
		var dbIdLojas = "";
		var dbIdKpi = "";

		if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(dbId) || string.IsNullOrEmpty(dbIdLojas) || string.IsNullOrEmpty(dbIdKpi))
		{
			if (File.Exists(configFilePath))
			{
				var json = File.ReadAllText(configFilePath);
				var config = JsonConvert.DeserializeObject<Config>(json);
				apiKey ??= config.NotionApiKey;
				dbId = config.NotionDatabaseId;
				dbIdKpi = config.NotionDatabaseIdKPIs;
				dbIdLojas = config.NotionDatabaseIdLojas;
			}
		}

		if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(dbId) || string.IsNullOrEmpty(dbIdKpi) || string.IsNullOrEmpty(dbIdLojas))
		{
			apiKey = Prompt("Insira o token da API do Notion:");
			dbId = Prompt("Insira o ID da tabela Mapa Klm:");
			dbIdKpi = Prompt("Insira o ID da tabela KPI:");
			dbIdLojas = Prompt("Insira o ID da tabela Lojas:");
			if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(dbId) || string.IsNullOrEmpty(dbIdKpi) || string.IsNullOrEmpty(dbIdLojas))
			{
				MessageBox.Show("Erro: Token da API e as Id's das tabelas não podem ser vazios", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
				if (File.Exists(configFilePath))
					File.Delete(configFilePath);
				return null;
			} 
			var config = new Config { NotionApiKey = apiKey, NotionDatabaseId = dbId, NotionDatabaseIdLojas = dbIdLojas, NotionDatabaseIdKPIs = dbIdKpi };
			File.WriteAllText(configFilePath, JsonConvert.SerializeObject(config, Formatting.Indented));
		}

		return new Config { NotionApiKey = apiKey, NotionDatabaseId = dbId, NotionDatabaseIdLojas = dbIdLojas, NotionDatabaseIdKPIs = dbIdKpi };
	}

	private static string Prompt(string text)
	{
		return Microsoft.VisualBasic.Interaction.InputBox(text, "Configuração Inicial", "");
	}
}
