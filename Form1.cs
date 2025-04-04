using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using ExcelDataReader;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Gerador_ecxel
{
	public partial class Form1 : Form
	{
		private readonly NotionService notionService;
		private string _colaborador = string.Empty;
		private string _data = string.Empty;
		private string _matricula = string.Empty;
		private string _status = string.Empty;
		private string _loja = string.Empty;
		private string _cod = string.Empty;
		private string _folderPath = string.Empty;
		private DataTable _dataTable;
		public Form1(Config config)
		{
			InitializeComponent();
			notionService = new NotionService(config.NotionApiKey, config.NotionDatabaseId);
		}
		private async void GerarPdf_Click(object sender, EventArgs e)
		{
			try
			{
				statusLabel.Visible = true;
				statusLabel.Text = "A Recolher Dados...";
				statusLabel.ForeColor = Color.Blue;
				progressBar1.Style = ProgressBarStyle.Marquee;
				progressBar1.Visible = true;
				var config = NotionConfigHelper.GetConfig();
				var notionKey = config.NotionApiKey;
				var databaseId = config.NotionDatabaseId;
				if (notionKey == null || databaseId == null)
				{
					progressBar1.Visible = false;
					statusLabel.Visible = false;
					MessageBox.Show("Erro: Chaves de API n�o foram definidas.");
					return;
				}
				NotionService notionService = new NotionService(notionKey, databaseId);
				List<string[]> entries = await notionService.GetDatabase();
				string previousName = "";
				int pdfCount = 0;
				statusLabel.Text = "A Gerar PDf`s...";
				for (int i = 0; i < entries.Count; i++)
				{
					_data = entries[i][0];
					_colaborador = entries[i][1];
					_matricula = entries[i][2];
					_status = entries[i][6];
					_cod = entries[i][7];
					_loja = entries[i][8];
					if (_colaborador != previousName && _status == "Novo")
					{
						GeneratePdf(entries);
						pdfCount++;
						previousName = _colaborador;
					}
				}
				if (pdfCount <= 0)
				{
					MessageBox.Show("Nenhum Dado para gerar", "Aviso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
					progressBar1.Visible = false;
					statusLabel.Visible = false;
					return;
				}
				statusLabel.Visible = false;
				progressBar1.Visible = false;
				MessageBox.Show("PDF gerado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				await notionService.UpdateStatus();
			}
			catch (Exception ex)
			{
				progressBar1.Visible = false;
				statusLabel.Visible = false;
				MessageBox.Show($"Erro ao gerar PDF: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void GeneratePdf(List<string[]> entries)
		{
			var baseColor = new BaseColor(75, 85, 87, 255);
			_folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Relatorios");
			if (!Directory.Exists(_folderPath))
				Directory.CreateDirectory(_folderPath);
			if (string.IsNullOrEmpty(_colaborador) || string.IsNullOrEmpty(_data) || string.IsNullOrEmpty(_folderPath))
			{
				throw new Exception("Vari�veis de nome do arquivo est�o nulas ou vazias.");
			}
			string filePath = Path.Combine(_folderPath, $"Relatorio_{_colaborador}" + "_" + $"{DateTime.Now:yyyy-MM-dd_HH-mm}.pdf");
			Document doc = new Document(PageSize.A4);
			if (doc == null)
				throw new Exception("O documento n�o foi inicializado corretamente.");
			PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

			doc.Open();
			string sourceDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Source");

			BaseFont bfCalibri = BaseFont.CreateFont(Path.Combine(sourceDir, "calibri-regular.ttf"), BaseFont.WINANSI, BaseFont.EMBEDDED);
			BaseFont bfcalibriBold = BaseFont.CreateFont(Path.Combine(sourceDir, "calibri-bold.ttf"), BaseFont.WINANSI, BaseFont.EMBEDDED);
			BaseFont bfcalibriBIt = BaseFont.CreateFont(Path.Combine(sourceDir, "calibri-bold-italic.ttf"), BaseFont.WINANSI, BaseFont.EMBEDDED);
			BaseFont bfVerdana = BaseFont.CreateFont(Path.Combine(sourceDir, "verdana.ttf"), BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

			iTextSharp.text.Font titleFont = new iTextSharp.text.Font(bfcalibriBold, 13, iTextSharp.text.Font.BOLD);
			iTextSharp.text.Font subTitleFont = new iTextSharp.text.Font(bfVerdana, 12, iTextSharp.text.Font.BOLD, baseColor);
			iTextSharp.text.Font tableHeaderFont = new iTextSharp.text.Font(bfcalibriBIt, 11, iTextSharp.text.Font.BOLD, baseColor);
			iTextSharp.text.Font tableFont = new iTextSharp.text.Font(bfCalibri, 8, iTextSharp.text.Font.NORMAL, baseColor);
			iTextSharp.text.Font sectionFont = new iTextSharp.text.Font(bfVerdana, 10, iTextSharp.text.Font.NORMAL, baseColor);

			string imagePath = Path.Combine(sourceDir, "logo.jpeg");
			if (File.Exists(imagePath))
			{
				iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
				img.ScaleAbsolute(160f, 40f);
				img.SetAbsolutePosition(20, doc.PageSize.Height - doc.TopMargin - 35);
				doc.Add(img);
			}
			Paragraph title = new Paragraph("DESPESAS DE KM EM VIATURA PR�PRIA\n", titleFont)
			{
				Alignment = Element.ALIGN_CENTER
			};
			doc.Add(title);
			doc.Add(new Paragraph("\n"));

			doc.Add(new Paragraph("Identifica��o: \n\n", subTitleFont));

			addTable(doc, 100, new float[] { 30f, 70f }, new (string, string, int, string, string, int?)[]{
			("Utilizador:", _colaborador, 14, "nenhuma", "cheia", null)
			}, sectionFont);

			addTable(doc, 60, new float[] { 30f, 30f }, new (string, string, int, string, string, int?)[] {
				("N� Colaborador:", _cod, 14, "nenhuma", "cheia",  Element.ALIGN_CENTER),
				("Empresa:", "Expressglass SA", 14, "nenhuma", "cheia",  Element.ALIGN_CENTER),
				("Centro de Custo:", _loja, 14, "nenhuma", "cheia", null),
			}, sectionFont);

			doc.Add(new Paragraph("Despesas - Mapa de Km \n\n", subTitleFont));

			addTable(doc, 60, new float[] { 30f, 30f }, new (string, string, int, string, string, int?)[] {
				("Data", _data, 14, "nenhuma", "cheia", null),
				("Matr�cula:", _matricula, 14, "nenhuma", "cheia", null),
			}, sectionFont);


			addTable(doc, 100, new float[] { 30f, 70f }, new (string, string, int, string, string, int?)[]{
			("Propriet�rio:", _colaborador, 14, "nenhuma", "cheia", null)
			}, sectionFont);

			PdfPTable table = new PdfPTable(6);
			table.WidthPercentage = 100;
			table.SetWidths(new float[] { 25f, 15f, 15f, 10f, 25f, 45f });

			string[] headers = { "Dia", "Sa�da", "Chegada", "Km's", "Local", "Motivo" };
			foreach (var header in headers)
			{
				PdfPCell cell = new PdfPCell(new Phrase(header, tableHeaderFont))
				{
					HorizontalAlignment = Element.ALIGN_LEFT,
					FixedHeight = 15f,
				};
				table.AddCell(cell);
			}
			float totalKM = 0;
			foreach (var entry in entries)
			{
				string status = entry[6];
				if (status != "Novo" || entry[1] != _colaborador) continue;
				string[] row = { entry[0], "09H00", "18H00", entry[5], entry[3], entry[4] };
				foreach (var dataValue in row)
				{
					if (int.TryParse(dataValue, out int number) || DateTime.TryParse(dataValue, out DateTime date))
					{
						totalKM += number;
						PdfPCell cellInt = new PdfPCell(new Phrase(dataValue, tableFont))
						{
							HorizontalAlignment = Element.ALIGN_RIGHT,
							FixedHeight = 15f,
						};
						table.AddCell(cellInt);
					}
					else
					{
						PdfPCell cell = new PdfPCell(new Phrase(dataValue, tableFont))
						{
							HorizontalAlignment = Element.ALIGN_CENTER,
							FixedHeight = 15f,
						};
						table.AddCell(cell);
					}
				}
			}
			doc.Add(table);
			doc.Add(new Paragraph("\n"));

			PdfPTable summaryTable = new PdfPTable(2);
			summaryTable.SetWidths(new float[] { 20, 15 });
			summaryTable.WidthPercentage = 20;
			summaryTable.HorizontalAlignment = Element.ALIGN_CENTER;
			AddCell(summaryTable, "Total Km", sectionFont, 14, "nenhuma");
			AddCellUL(summaryTable, totalKM.ToString(), sectionFont, 14, Element.ALIGN_RIGHT);
			AddCell(summaryTable, "Valor/Km", sectionFont, 14, "nenhuma");
			AddCellUL(summaryTable, "0,36 �", sectionFont, 14, Element.ALIGN_RIGHT);
			doc.Add(summaryTable);

			AddExpenseTable(doc, subTitleFont, totalKM);

			PdfPTable obsTable = new PdfPTable(2);
			obsTable.SetWidths(new float[] { 10, 50 });
			obsTable.WidthPercentage = 100;
			obsTable.SpacingBefore = 20f;
			AddCell(obsTable, "Observa��es:", sectionFont, 70f, "nenhuma");
			PdfPCell cellObs = new PdfPCell(new Phrase(""))
			{
				FixedHeight = 70f,
				BorderWidth = 0.5f,
			};
			obsTable.AddCell(cellObs);
			AddCell(obsTable, "", sectionFont, 70f, "fina");
			doc.Add(obsTable);

			PdfPTable assignTable = new PdfPTable(3);
			assignTable.WidthPercentage = 100;
			assignTable.SetWidths(new float[] { 20f, 25f, 60f });
			assignTable.HorizontalAlignment = Element.ALIGN_LEFT;
			assignTable.SpacingBefore = 35;
			PdfPCell cellBot = new PdfPCell(new Phrase("O Colaborador:", sectionFont))
			{
				VerticalAlignment = Element.ALIGN_MIDDLE,
				FixedHeight = 62f,
				Border = PdfPCell.NO_BORDER
			};
			assignTable.AddCell(cellBot);
			PdfPCell middleLineCell = new PdfPCell(new Phrase("__________________", sectionFont))
			{
				Border = PdfPCell.NO_BORDER,
				HorizontalAlignment = Element.ALIGN_CENTER,
				VerticalAlignment = Element.ALIGN_MIDDLE,
				FixedHeight = 62f
			};
			assignTable.AddCell(middleLineCell);

			imagePath = Path.Combine(sourceDir, "Assign.png");
			iTextSharp.text.Image imgBot = iTextSharp.text.Image.GetInstance(imagePath);
			imgBot.ScaleAbsolute(100, 60f);

			PdfPTable innerTable = new PdfPTable(2);
			innerTable.WidthPercentage = 100;
			innerTable.SetWidths(new float[] { 60f, 100f });

			PdfPCell textCell = new PdfPCell(new Phrase("O Respons�vel:", sectionFont))
			{
				Border = PdfPCell.NO_BORDER,
				VerticalAlignment = Element.ALIGN_MIDDLE,
				HorizontalAlignment = Element.ALIGN_LEFT,
				PaddingLeft = 30
			};
			innerTable.AddCell(textCell);

			PdfPCell imgCell = new PdfPCell(imgBot)
			{
				Border = PdfPCell.NO_BORDER,
				VerticalAlignment = Element.ALIGN_MIDDLE,
				Padding = 0
			};
			innerTable.AddCell(imgCell);

			PdfPCell finalCell = new PdfPCell(innerTable)
			{
				Border = PdfPCell.NO_BORDER,
				FixedHeight = 62f,
				VerticalAlignment = Element.ALIGN_MIDDLE
			};
			assignTable.AddCell(finalCell);

			doc.Add(assignTable);
			doc.Add(new Paragraph("\n"));
			doc.Add(new Paragraph("Nota: valores recebidos at� dia 16 do m�s N, ser�o pagos no m�s N, valores recebidos entre dia 17 e 31 do m�s N ser�o pagos no m�s N+1", sectionFont));
			doc.Close();
		}

		void addTable(Document doc, float widthPercentage, float[] columnWidths, (string title, string value, int fontSize, string titleBorder, string border, int?)[] data, iTextSharp.text.Font font)
		{
			foreach (var (title, valor, fontSize, titleBorder, borderborder, alignment) in data)
			{
				PdfPTable table = new PdfPTable(2);
				table.WidthPercentage = widthPercentage;
				table.SetWidths(columnWidths);
				table.HorizontalAlignment = Element.ALIGN_LEFT;

				AddCell(table, title, font, fontSize, titleBorder);

				if (alignment.HasValue)
					AddCellAlign(table, valor, font, fontSize, borderborder, alignment.Value);
				else
					AddCell(table, valor, font, fontSize, borderborder);

				doc.Add(table);
				doc.Add(new Paragraph("\n"));
			}
		}

		private void AddCell(PdfPTable table, string text, iTextSharp.text.Font font, float height, string borderType)
		{
			var cellColor = new BaseColor(75, 86, 98, 255);
			PdfPCell cell = new PdfPCell(new Phrase(text, font))
			{
				HorizontalAlignment = Element.ALIGN_LEFT,
				FixedHeight = height
			};
			if (borderType == "nenhuma")
			{
				cell.Border = PdfPCell.NO_BORDER;
			}
			else if (borderType == "fina")
			{
				cell.BorderWidth = 0.5f;
				cell.BorderColor = cellColor;
			}
			else if (borderType == "cheia")
			{
				cell.BorderWidth = 1.2f;
				cell.BorderColor = cellColor;
			}
			else if (borderType == "full cheia")
			{
				cell.BorderWidth = 1.6f;
				cell.BorderColor = cellColor;
			}
			table.AddCell(cell);
		}
		private void AddCellUL(PdfPTable table, string text, iTextSharp.text.Font font, float height, int align)
		{
			var cellColor = new BaseColor(75, 86, 98, 255);
			PdfPCell cell = new PdfPCell(new Phrase(text, font))
			{
				HorizontalAlignment = align,
				FixedHeight = height,
				Border = PdfPCell.BOTTOM_BORDER
			};
			table.AddCell(cell);
		}
		private void AddCellAlign(PdfPTable table, string text, iTextSharp.text.Font font, float height, string borderType, int align)
		{
			var cellColor = new BaseColor(75, 86, 98, 255);
			PdfPCell cell = new PdfPCell(new Phrase(text, font))
			{
				HorizontalAlignment = align,
				FixedHeight = height
			};
			if (borderType == "nenhuma")
			{
				cell.BorderColor = cellColor;
				cell.Border = PdfPCell.NO_BORDER;
			}
			else if (borderType == "fina")
			{
				cell.BorderWidth = 0.5f;
				cell.BorderColor = cellColor;
			}
			else if (borderType == "cheia")
			{
				cell.BorderWidth = 1.2f;
				cell.BorderColor = cellColor;
			}
			table.AddCell(cell);
		}

		public void AddExpenseTable(Document doc, iTextSharp.text.Font subTitleFont, float totalKM)
		{
			var cellColor = new BaseColor(75, 86, 98, 255);
			PdfPTable outerTable = new PdfPTable(1);
			outerTable.WidthPercentage = 50;
			outerTable.HorizontalAlignment = Element.ALIGN_CENTER;
			outerTable.SpacingBefore = 15f;

			PdfPTable innerTable = new PdfPTable(2);
			innerTable.SetWidths(new float[] { 70, 30 });
			innerTable.WidthPercentage = 100;

			PdfPCell leftCell = new PdfPCell(new Phrase("Total de Despesas:", subTitleFont));
			leftCell.Border = PdfPCell.NO_BORDER;
			leftCell.HorizontalAlignment = Element.ALIGN_LEFT;
			leftCell.FixedHeight = 20f;

			PdfPCell rightCell = new PdfPCell(new Phrase(Math.Round(totalKM * 0.36, 2).ToString("0.00") + " �", subTitleFont));
			rightCell.Border = PdfPCell.NO_BORDER;
			rightCell.HorizontalAlignment = Element.ALIGN_RIGHT;
			rightCell.FixedHeight = 20f;
			innerTable.AddCell(leftCell);
			innerTable.AddCell(rightCell);

			PdfPCell outerCell = new PdfPCell(innerTable);
			outerCell.BorderWidth = 1.7f;
			outerCell.BorderColor = cellColor;
			outerTable.AddCell(outerCell);

			doc.Add(outerTable);
		}
		private void button1_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("explorer.exe", _folderPath);
		}

		private void GerarPdf_MouseEnter(object sender, EventArgs e)
		{
			((Button)sender).BackColor = Color.FromArgb(7, 43, 101);
			((Button)sender).ForeColor = Color.White;
		}

		private void GerarPdf_MouseLeave(object sender, EventArgs e)
		{
			((Button)sender).BackColor = Color.LightGray;
			((Button)sender).ForeColor = Color.Black;
		}

		private void resetData_Click(object sender, EventArgs e)
		{
			var configPath = Path.Combine(Application.StartupPath, "notion_config.json");

			if (File.Exists(configPath))
				File.Delete(configPath);

			var newConfig = NotionConfigHelper.GetConfig();
			if (newConfig == null)
				MessageBox.Show("A atualiza��o falhou ou foi cancelada", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
			else
				MessageBox.Show("Atualiza��o feita com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		private void button3_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls",
				RestoreDirectory = true
			};
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string excelPath = openFileDialog.FileName;
				try
				{
					loadData(excelPath);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Erro ao ler o ficheiro Excel: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
				MessageBox.Show("Nenhum ficheiro selecionado", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void loadData(string filePath)
		{
			using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
			{
				using (var reader = ExcelReaderFactory.CreateReader(stream))
				{
					var result = reader.AsDataSet(new ExcelDataSetConfiguration
					{
						ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
					});

					_dataTable = result.Tables[0];

					var lojas = _dataTable.AsEnumerable()
						.Skip(10)
						  .Select(row => new
						  {
							  B = row.IsNull(1) ? "" : row[1].ToString(),
							  C = row.IsNull(2) ? "" : row[2].ToString(),
							  D = row.IsNull(3) ? "" : row[3].ToString(),
							  E = row.IsNull(4) ? "" : row[4].ToString(),
							  F = row.IsNull(5) ? "" : row[5].ToString(),
							  I = row.IsNull(8) ? "" : row[8].ToString()
						  })
						.Where(name => !string.IsNullOrEmpty(name.B))
						.Distinct()
						.ToList();

					listBoxStores.Items.Clear();
					foreach (var loja in lojas)
					{
						listBoxStores.Items.Add(loja.B);
					}
				}
			}
		}


		private void buttonFilter_Click(object sender, EventArgs e)
		{
			var lojasSelecionadas = listBoxStores.CheckedItems.Cast<string>().ToList();
			if (lojasSelecionadas.Count == 0)
			{
				MessageBox.Show("Selecione pelo menos uma loja.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var dadosFiltrados = _dataTable.AsEnumerable()
				.Skip(10)
				.Where(row => lojasSelecionadas.Contains(row.Field<string>(1)?.Trim() ?? ""))
				.Select(row => new
				{
					  B = row.IsNull(1) ? "" : row[1].ToString(),
					  C = row.IsNull(2) ? "" : row[2].ToString(),
					  D = row.IsNull(3) ? "" : row[3].ToString(),
					  E = row.IsNull(4) ? "" : row[4].ToString(),
					  F = row.IsNull(5) ? "" : row[5].ToString(),
					  I = row.IsNull(8) ? "" : row[8].ToString()
				})
				.ToList();
			if (dadosFiltrados.Count == 0)
			{
				MessageBox.Show("Nenhum dado encontrado para os filtros selecionados.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			dataGridView1.DataSource = dadosFiltrados;
			foreach (DataGridViewColumn col in dataGridView1.Columns)
			{
				col.Visible = col.Index >= 0 && col.Index <= 5;
				col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			}
			dataGridView1.RowTemplate.Height = 30;
			dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
		}
	}
}

//{
//	B = row.Field<string>(1),
//							C = row.Field<double>(2),
//							D = row.Field<double>(3),
//							E = row.Field<double>(4),
//							F = row.Field<double>(5),
//							I = row.Field<double>(8)
//						})