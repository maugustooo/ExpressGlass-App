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
		private string _dataBaseIdLojas;
		private Form1 _form1;
		public consola(Config config, Form1 form)
		{
			_dataBaseIdLojas = config.NotionDatabaseIdLojas;
			_form1 = form;
		}

		public async Task ExecutarAsync()
		{
			var notionService = new NotionService(_dataBaseIdLojas);
			await notionService.CarregarLojasParaBaseLocalAsync();
			_form1.CarregarLojasNoComboBox();
		}
		public void createDataBase()
		{
			if (File.Exists("lojas.db"))
			{
				ExecutarAsync().Wait();
				return;
			}
			Batteries_V2.Init();
			using (var conn = new SqliteConnection("Data Source=lojas.db"))
			{
				conn.Open();

				string sql = @"
                    CREATE TABLE IF NOT EXISTS Lojas (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nome TEXT NOT NULL
                    )";

				using (var cmd = new SqliteCommand(sql, conn))
				{
					cmd.ExecuteNonQuery();
				}
			}
		}
	}
}
