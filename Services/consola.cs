using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SQLitePCL;
using Microsoft.Data.Sqlite;
using Gerador_ecxel;
namespace Gerador_PDF.Services
{
	class consola
	{
		private Config _config;
		private Form1 _form1;

		public consola(Config config, Form1 form)
		{
			_config = config;
			_form1 = form;
		}

		public async void ExecutarAsync()
		{
			var notionService = new NotionService(_config.NotionApiKey, _config.NotionDatabaseId, _config.NotionDatabaseIdKPIs, _config.NotionDatabaseIdLojas, _config.NotionDatabaseIdStockParado);
			_form1.UpdateStatusBar(1);
			await notionService.CarregarLojasParaBaseLocalAsync();
			_form1.UpdateStatusBar(0);
			//_form1.CarregarLojasNoComboBox();
		}
		public void createDataBase()
		{
			if (File.Exists("lojas.db"))
			{
				ExecutarAsync();
				return;
			}
			else
			{
				using (var fs = File.Create("lojas.db"))
				{
					fs.Close();
				}
			}
			Batteries_V2.Init();
			using (var conn = new SqliteConnection("Data Source=lojas.db"))
			{
				conn.Open();

				string sql = @"
				CREATE TABLE IF NOT EXISTS Data (
					Id INTEGER PRIMARY KEY AUTOINCREMENT,
					NomeLoja TEXT NOT NULL UNIQUE,
					obj TEXT NOT NULL,
					Faturados TEXT NOT NULL,
					""TX REP %"" TEXT NOT NULL,
					VAPS TEXT NOT NULL,
					FTE TEXT NOT NULL,
					""QTD Escovas"" INTEGER NOT NULL
				)";


				using (var cmd = new SqliteCommand(sql, conn))
				{
					cmd.ExecuteNonQuery();
				}
			}
			ExecutarAsync();
		}
	}
}
