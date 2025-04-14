using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;
using static Gerador_ecxel.Form1;

namespace Gerador_PDF.Services
{
    public class readExcel
    {
		public class FaturadoData
		{
			public string loja { get; set; }
			public double faturados { get; set; }
			public double fte { get; set; }
			public double objAoDia { get; set; }
			public double objMes { get; set; }
			public double taxRep { get; set; }
			public double qntRep { get; set; }
		}

		public class ComplementarData
		{
			public string lojas { get; set; }
			public double vaps { get; set; }
			public double escovas { get; set; }
			public double escovasPercent { get; set; }
			public double polimento { get; set; }
		}

		public class monthStoreData
		{
			public string loja { get; set; }
			public double NPS { get; set; }
		}

		public List<FaturadoData> ?faturados;
		public List<ComplementarData> complementares;
		public List<monthStoreData> monthStores;
		public string mes { get; set; }
		public readExcel(string excelPath, string selectedItem)
        {
			var dados = loadData(excelPath, selectedItem);

			faturados = (List<FaturadoData>)dados["faturados"];
			complementares = (List<ComplementarData>)dados["complementares"];
			monthStores = (List<monthStoreData>)dados["monthStores"];
			mes = (string)dados["mes"];
		}

		private static double TryRound(DataRow row, int columnIndex, int decimalPlaces)
		{
			if (row.IsNull(columnIndex)) return 0;

			var rawValue = row[columnIndex]?.ToString()?.Trim();
			if (string.IsNullOrWhiteSpace(rawValue)) return 0;

			double value;

			if (columnIndex == 9 || columnIndex == 7 || columnIndex == 27)
			{
				if (double.TryParse(rawValue, NumberStyles.Any, new CultureInfo("pt-PT"), out value))
				{
					value *= 100;
					return Math.Round(value, decimalPlaces);
				}

				if (double.TryParse(rawValue, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
				{
					return Math.Round(value, decimalPlaces);
				}
			}
			if (double.TryParse(rawValue, NumberStyles.Any, new CultureInfo("pt-PT"), out value))
			{
				return Math.Round(value, decimalPlaces);
			}

			if (double.TryParse(rawValue, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
			{
				return Math.Round(value, decimalPlaces);
			}
			return 0;
		}
		private string GetRigthMonth(string mes)
		{
			try
			{
				DateTime data = DateTime.Parse(mes);
				string monthName = data.ToString("MMMM", new System.Globalization.CultureInfo("pt-PT"));
				monthName = char.ToUpper(monthName[0]) + monthName.Substring(1);

				return $"{monthName} {data.Year}";
			}
			catch (FormatException)
			{
				throw new ArgumentException("Data inválida");
			}
		}
		private string GetSheetName(string mesComboBox, int ano)
		{
			var abrevs = new Dictionary<string, string>
			{
				{"Janeiro", "Jan"},
				{"Fevereiro", "Fev"},
				{"Março", "Mar"},
				{"Abril", "Abr"},
				{"Maio", "Mai"},
				{"Junho", "Jun"},
				{"Julho", "Jul"},
				{"Agosto", "Ago"},
				{"Setembro", "Set"},
				{"Outubro", "Out"},
				{"Novembro", "Nov"},
				{"Dezembro", "Dez"}
			};

			if (!abrevs.ContainsKey(mesComboBox))
				throw new ArgumentException("Mês inválido na combo box");

			string abrev = abrevs[mesComboBox];
			string anoDoisDigitos = (ano % 100).ToString("D2");
			return $"Lojas {abrev}{anoDoisDigitos}";
		}

		private Dictionary<string, object> loadData(string filePath, string monthSelected)
		{
			string nomeSheet = null;
			if (!string.IsNullOrEmpty(monthSelected))
				nomeSheet = GetSheetName(monthSelected, 2025); ;
			string[] lojas = { "Lojas Jan25", "Lojas Fev25", "Lojas Mar25", "Lojas Abr25", "Lojas Mai25", "Lojas Jun25", "Lojas Jul25", "Lojas Ago25", "Lojas Set25", "Lojas Out25", "Lojas Nov25", "Lojas Dez25" };
			DataTable monthStores = null;
			var faturadosList = new List<FaturadoData>();
			var complementaresList = new List<ComplementarData>();
			var monthStoreList = new List<monthStoreData>();
			string mes = "";

			using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
			{
				using (var reader = ExcelReaderFactory.CreateReader(stream))
				{
					var result = reader.AsDataSet(new ExcelDataSetConfiguration
					{
						ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
					});

					if (result.Tables.Contains(nomeSheet))
					{
						monthStores = result.Tables[nomeSheet];

						for (int linha = 2; linha <= 3; linha++)
						{
							if (!monthStores.Rows[linha].IsNull(1))
							{
								mes = monthStores.Rows[linha][1]?.ToString()?.Trim() ?? "";
								if (!string.IsNullOrWhiteSpace(mes))
								{
									mes = GetRigthMonth(mes);
									break;
								}
							}
						}

						monthStoreList = monthStores.AsEnumerable()
							.Skip(5)
							.Select(static row => new monthStoreData
							{
								loja = row.IsNull(1) ? string.Empty : row[1]?.ToString() ?? string.Empty,
								NPS = TryRound(row, 27, 1),
							})
							.Where(data => !string.IsNullOrEmpty(data.loja))
							.Distinct()
							.ToList();

						return new Dictionary<string, object>
						{
							{"mes", mes },
							{"faturados", faturadosList},
							{"complementares", complementaresList},
							{"monthStores", monthStoreList}
						};
					}
					var faturadosTable = result.Tables["Faturados"];
					if (faturadosTable != null)
					{
						for (int linha = 6; linha <= 7; linha++)
						{
							for (int coluna = 1; coluna <= 2; coluna++)
							{
								if (!faturadosTable.Rows[linha].IsNull(coluna))
								{
									mes = faturadosTable.Rows[linha][coluna]?.ToString()?.Trim() ?? "";
									if (!string.IsNullOrWhiteSpace(mes))
										break;
								}
							}
							if (!string.IsNullOrWhiteSpace(mes))
								break;
						}
						faturadosList = faturadosTable.AsEnumerable()
						.Skip(9)
						.Select(static row => new FaturadoData
						{
							loja = row.IsNull(1) ? string.Empty : row[1]?.ToString() ?? string.Empty,
							faturados = TryRound(row, 2, 1),
							fte = TryRound(row, 3, 1),
							objAoDia = TryRound(row, 4, 0),
							objMes = TryRound(row, 5, 0),
							taxRep = TryRound(row, 9, 2),
							qntRep = TryRound(row, 10, 0)

						})
						.Where(data => !string.IsNullOrEmpty(data.loja))
						.Distinct()
						.ToList();

					}
					var complementaresTable = result.Tables["Complementares"];
					if (complementaresTable != null)
					{
						complementaresList = complementaresTable.AsEnumerable()
						.Skip(10)
						.Select(row => new ComplementarData
						{
							lojas = row.IsNull(2) ? string.Empty : row[2]?.ToString() ?? string.Empty,
							vaps = TryRound(row, 3, 1),
							escovas = TryRound(row, 6, 1),
							escovasPercent = TryRound(row, 7, 2),
							polimento = TryRound(row, 8, 0)
						})
						.ToList();
					}
				}
			}
			Console.WriteLine($"mes: {mes}");
			return new Dictionary<string, object>
			{
				{"mes", mes },
				{"faturados", faturadosList},
				{"complementares", complementaresList},
				{"monthStores", monthStoreList}
			};
		}
	}
}
