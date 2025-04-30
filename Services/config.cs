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

	[JsonProperty("NotionDatabaseIdStockParado")]
	public string NotionDatabaseIdStockParado { get; set; }

	[JsonProperty("NotionDatabaseIdLojas")]
	public string NotionDatabaseIdLojas { get; set; }

}

public static class NotionConfigHelper
{
	private static string configFilePath = Path.Combine(Application.StartupPath, "NotionConfig.json");

	public static Config GetConfig()
	{
		var apiKey = Environment.GetEnvironmentVariable("NOTION_API_KEY", EnvironmentVariableTarget.Machine);
		var dbId = "";
		var dbIdLojas = "";
		var dbIdStockParado = "";
		var dbIdKpi = "";

		if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(dbId) || string.IsNullOrEmpty(dbIdLojas) || string.IsNullOrEmpty(dbIdKpi) || string.IsNullOrEmpty(dbIdStockParado))
		{
			if (File.Exists(configFilePath))
			{
				var json = File.ReadAllText(configFilePath);
				var config = JsonConvert.DeserializeObject<Config>(json);
				apiKey ??= config.NotionApiKey;
				dbId = config.NotionDatabaseId;
				dbIdKpi = config.NotionDatabaseIdKPIs;
				dbIdStockParado = config.NotionDatabaseIdStockParado;
				dbIdLojas = config.NotionDatabaseIdLojas;
			}
		}

		if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(dbId) || string.IsNullOrEmpty(dbIdKpi) || string.IsNullOrEmpty(dbIdLojas) || string.IsNullOrEmpty(dbIdStockParado))
		{
			apiKey = Prompt("Insira o token da API do Notion:");
			dbId = Prompt("Insira o ID da tabela Mapa Klm:");
			dbIdKpi = Prompt("Insira o ID da tabela KPI:");
			dbIdStockParado = Prompt("Insira o ID da tabela Stock Parado:");
			dbIdLojas = Prompt("Insira o ID da tabela Lojas:");
			if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(dbId) || string.IsNullOrEmpty(dbIdKpi) || string.IsNullOrEmpty(dbIdLojas) || string.IsNullOrEmpty(dbIdStockParado))
			{
				MessageBox.Show("Erro: Token da API e as Id's das tabelas não podem ser vazios", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
				//if (File.Exists(configFilePath))
				//	File.Delete(configFilePath);
				return null;
			} 
			var config = new Config { NotionApiKey = apiKey, NotionDatabaseId = dbId, NotionDatabaseIdLojas = dbIdLojas, NotionDatabaseIdKPIs = dbIdKpi, NotionDatabaseIdStockParado = dbIdStockParado };
			File.WriteAllText(configFilePath, JsonConvert.SerializeObject(config, Formatting.Indented));
		}

		return new Config { NotionApiKey = apiKey, NotionDatabaseId = dbId, NotionDatabaseIdLojas = dbIdLojas, NotionDatabaseIdKPIs = dbIdKpi, NotionDatabaseIdStockParado = dbIdStockParado };
	}

	public static Config updateConfig(string configPath)
	{
		var apiKey = Environment.GetEnvironmentVariable("NOTION_API_KEY", EnvironmentVariableTarget.Machine);
		var dbId = "";
		var dbIdLojas = "";
		var dbIdStockParado = "";
		var dbIdKpi = "";
		apiKey = Prompt("Insira o token da API do Notion:");
		dbId = Prompt("Insira o ID da tabela Mapa Klm:");
		dbIdKpi = Prompt("Insira o ID da tabela KPI:");
		dbIdStockParado = Prompt("Insira o ID da tabela Stock Parado:");
		dbIdLojas = Prompt("Insira o ID da tabela Lojas:");
		if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(dbId) || string.IsNullOrEmpty(dbIdKpi) || string.IsNullOrEmpty(dbIdLojas) || string.IsNullOrEmpty(dbIdStockParado))
		{
			MessageBox.Show("Erro: Token da API e as Id's das tabelas não podem ser vazios", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return null;
		}
		if (File.Exists(configPath))
			File.Delete(configPath);
		var config = new Config { NotionApiKey = apiKey, NotionDatabaseId = dbId, NotionDatabaseIdLojas = dbIdLojas, NotionDatabaseIdKPIs = dbIdKpi, NotionDatabaseIdStockParado = dbIdStockParado };
		File.WriteAllText(configFilePath, JsonConvert.SerializeObject(config, Formatting.Indented));
		return new Config { NotionApiKey = apiKey, NotionDatabaseId = dbId, NotionDatabaseIdLojas = dbIdLojas, NotionDatabaseIdKPIs = dbIdKpi, NotionDatabaseIdStockParado = dbIdStockParado };

	}

	private static string Prompt(string text)
	{
		return Microsoft.VisualBasic.Interaction.InputBox(text, "Configuração Inicial", "");
	}
}
