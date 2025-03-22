using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

public class Config
{
	public string NotionApiKey { get; set; }
	public string NotionDatabaseId { get; set; }
}

public static class NotionConfigHelper
{
	private static string configFilePath = Path.Combine(Application.StartupPath, "notion_config.json");

	public static Config GetConfig()
	{
		var apiKey = Environment.GetEnvironmentVariable("NOTION_API_KEY", EnvironmentVariableTarget.Machine);
		var dbId = Environment.GetEnvironmentVariable("NOTION_DATABASE_ID", EnvironmentVariableTarget.Machine);

		if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(dbId))
		{
			if (File.Exists(configFilePath))
			{
				var json = File.ReadAllText(configFilePath);
				var config = JsonConvert.DeserializeObject<Config>(json);
				apiKey ??= config.NotionApiKey;
				dbId ??= config.NotionDatabaseId;
			}
		}

		if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(dbId))
		{
			apiKey = Prompt("Insira o token da API do Notion:");
			dbId = Prompt("Insira o ID da base de dados do Notion:");
			if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(dbId))
			{
				MessageBox.Show("Erro: Token da API ou ID da base de dados não podem ser vazios.");
				return null;
			}
			var config = new Config { NotionApiKey = apiKey, NotionDatabaseId = dbId };
			File.WriteAllText(configFilePath, JsonConvert.SerializeObject(config, Formatting.Indented));
		}

		return new Config { NotionApiKey = apiKey, NotionDatabaseId = dbId };
	}

	private static string Prompt(string text)
	{
		return Microsoft.VisualBasic.Interaction.InputBox(text, "Configuração Inicial", "");
	}
}
