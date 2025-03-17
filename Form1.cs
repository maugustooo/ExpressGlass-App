using System.IO;
using PdfiumViewer;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Gerador_ecxel
{
	public partial class Form1 : Form
	{
		private readonly NotionService notionService;
		private string _colaborador;
		private string _data;
		private string _matricula;
		private string _localidade;
		private string _motivo;
		private string _klm;
		private string _status;
		private string _loja;
		private string _cod;

		public Form1()
		{
			InitializeComponent();
		}

		private async void GerarPdf_Click(object sender, EventArgs e)
		{
			try
			{
				NotionService notionService = new NotionService("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", "1a2a53a0578180849ed2c31ac791c876");
				List<string[]> entries = await notionService.GetDatabase();
				string previousName = null;

				for (int i = 0; i < entries.Count; i++)
				{
					_data = entries[i][0];
					_colaborador = entries[i][1];
					_matricula = entries[i][2];
					_localidade = entries[i][3];
					_motivo = entries[i][4];
					_klm = entries[i][5];
					_status = entries[i][6];
					_cod = entries[i][7];
					_loja = entries[i][8];
					if (_colaborador != previousName && _status == "Novo")
					{
						await GeneratePdf(entries);
						previousName = _colaborador;
					}
				}
				MessageBox.Show("PDF gerado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Erro ao gerar PDF: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public async Task GeneratePdf(List<string[]> entries)
		{
			var baseColor = new BaseColor(75, 85, 87, 255);
			string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Relatorios");
			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);
			string filePath = Path.Combine(folderPath, $"Relatorio_Notion_{_colaborador}.pdf");
			Document doc = new Document(PageSize.A4);
			PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

			doc.Open();

			string fontPath =  "Sources\\calibri-regular.ttf";
			BaseFont bfCalibri = BaseFont.CreateFont(fontPath, BaseFont.WINANSI, BaseFont.EMBEDDED);

			fontPath = "Sources\\calibri-regular.ttf";
			BaseFont bfcalibriBold = BaseFont.CreateFont(fontPath, BaseFont.WINANSI, BaseFont.EMBEDDED);

			fontPath = "Sources\\calibri-regular.ttf";
			BaseFont bfcalibriBIt = BaseFont.CreateFont(fontPath, BaseFont.WINANSI, BaseFont.EMBEDDED);

			fontPath = "Sources\\calibri-regular.ttf";
			BaseFont bfVerdana = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

			iTextSharp.text.Font titleFont = new iTextSharp.text.Font(bfcalibriBold, 13, iTextSharp.text.Font.BOLD);
			iTextSharp.text.Font subTitleFont = new iTextSharp.text.Font(bfVerdana, 12, iTextSharp.text.Font.BOLD, baseColor);
			iTextSharp.text.Font tableHeaderFont = new iTextSharp.text.Font(bfcalibriBIt, 11, iTextSharp.text.Font.BOLD, baseColor);
			iTextSharp.text.Font tableFont = new iTextSharp.text.Font(bfCalibri, 8, iTextSharp.text.Font.NORMAL, baseColor);
			iTextSharp.text.Font sectionFont = new iTextSharp.text.Font(bfVerdana, 10, iTextSharp.text.Font.NORMAL, baseColor);
			iTextSharp.text.Font cellFont = new iTextSharp.text.Font(bfVerdana, 7, iTextSharp.text.Font.NORMAL, baseColor);
			iTextSharp.text.Font boldFont = new iTextSharp.text.Font(bfcalibriBold, 9, iTextSharp.text.Font.BOLD, baseColor);

			string imagePath = "Sources\\logo.jpeg";
			if (File.Exists(imagePath))
			{
				iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
				img.ScaleAbsolute(160f, 40f);
				img.SetAbsolutePosition(20, doc.PageSize.Height - doc.TopMargin - 35);
				doc.Add(img);
			}
			Paragraph title = new Paragraph("DESPESAS DE KM EM VIATURA PRÓPRIA\n", titleFont)
			{
				Alignment = Element.ALIGN_CENTER
			};
			doc.Add(title);
			doc.Add(new Paragraph("\n"));

			doc.Add(new Paragraph("Identificação: \n\n", subTitleFont));

			PdfPTable identificacaoTable = new PdfPTable(2);
			identificacaoTable.WidthPercentage = 100;
			identificacaoTable.SetWidths(new float[] { 30f, 70f });
			identificacaoTable.HorizontalAlignment = Element.ALIGN_LEFT;
			AddCell(identificacaoTable, "Utilizador:", sectionFont, 14, "nenhuma");
			AddCell(identificacaoTable, _colaborador, sectionFont, 14, "cheia");
			doc.Add(identificacaoTable);
			doc.Add(new Paragraph("\n"));

			identificacaoTable = new PdfPTable(2);
			identificacaoTable.WidthPercentage = 60;
			identificacaoTable.SetWidths(new float[] { 30f, 30f });
			identificacaoTable.HorizontalAlignment = Element.ALIGN_LEFT;
			AddCell(identificacaoTable, "Nº Colaborador:", sectionFont, 14f, "nenhuma");
			AddCellAlign(identificacaoTable, _cod, sectionFont, 14f, "cheia", Element.ALIGN_CENTER);
			doc.Add(identificacaoTable);
			doc.Add(new Paragraph("\n"));

			identificacaoTable = new PdfPTable(2);
			identificacaoTable.WidthPercentage = 60;
			identificacaoTable.SetWidths(new float[] { 30f, 30f });
			identificacaoTable.HorizontalAlignment = Element.ALIGN_LEFT;
			AddCell(identificacaoTable, "Empresa:", sectionFont, 14f, "nenhuma");
			AddCellAlign(identificacaoTable, "Expressglass SA", sectionFont, 14f, "cheia", Element.ALIGN_CENTER);
			doc.Add(identificacaoTable);
			doc.Add(new Paragraph("\n"));

			identificacaoTable = new PdfPTable(2);
			identificacaoTable.WidthPercentage = 60;
			identificacaoTable.SetWidths(new float[] { 30f, 30f });
			identificacaoTable.HorizontalAlignment = Element.ALIGN_LEFT;
			AddCell(identificacaoTable, "Centro de Custo:", sectionFont, 14f, "nenhuma");
			AddCell(identificacaoTable, _loja, sectionFont, 14f, "cheia");
			doc.Add(identificacaoTable);
			doc.Add(new Paragraph("\n"));

			doc.Add(new Paragraph("Despesas - Mapa de Km \n\n", subTitleFont));

			identificacaoTable = new PdfPTable(2);
			identificacaoTable.WidthPercentage = 60;
			identificacaoTable.SetWidths(new float[] { 30f, 30f });
			identificacaoTable.HorizontalAlignment = Element.ALIGN_LEFT;
			AddCell(identificacaoTable, "Data",sectionFont, 14f, "nenhuma");
			AddCell(identificacaoTable, _data, sectionFont, 14f, "cheia");
			doc.Add(identificacaoTable);
			doc.Add(new Paragraph("\n"));

			identificacaoTable = new PdfPTable(2);
			identificacaoTable.WidthPercentage = 60;
			identificacaoTable.SetWidths(new float[] { 30f, 30 });
			identificacaoTable.HorizontalAlignment = Element.ALIGN_LEFT;
			AddCell(identificacaoTable, "Matrícula:", sectionFont, 14f, "nenhuma");
			AddCell(identificacaoTable, _matricula, sectionFont, 14f, "cheia");
			doc.Add(identificacaoTable);
			doc.Add(new Paragraph("\n"));

			identificacaoTable = new PdfPTable(2);
			identificacaoTable.WidthPercentage = 100;
			identificacaoTable.SetWidths(new float[] { 30f, 70f });
			identificacaoTable.HorizontalAlignment = Element.ALIGN_LEFT;
			AddCell(identificacaoTable, "Proprietário:", sectionFont, 14f, "nenhuma");
			AddCell(identificacaoTable, _colaborador, sectionFont, 14f, "cheia");
			doc.Add(identificacaoTable);
			doc.Add(new Paragraph("\n"));


			PdfPTable table = new PdfPTable(6);
			table.WidthPercentage = 100;
			table.SetWidths(new float[] { 25f, 15f, 15f, 10f, 25f, 45f});

			string[] headers = { "Dia", "Saída", "Chegada", "Km's", "Local", "Motivo" };
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
				if (entry[1] != _colaborador) continue;
				string[] row = {entry[0], "09H00", "18H00", entry[5], entry[3], entry[4] };
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
			AddCellUL(summaryTable, "0,36 €", sectionFont, 14, Element.ALIGN_RIGHT);
			doc.Add(summaryTable);

			AddExpenseTable(doc, subTitleFont, totalKM);

			PdfPTable obsTable = new PdfPTable(2);
			obsTable.SetWidths(new float[] { 10, 50 });
			obsTable.WidthPercentage = 100;
			obsTable.SpacingBefore = 20f;
			AddCell(obsTable, "Observações:", sectionFont, 70f, "nenhuma");
			PdfPCell cellObs = new PdfPCell(new Phrase(""))
			{				
				FixedHeight = 70f,
				BorderWidth = 0.5f,
			};
			obsTable.AddCell(cellObs);
			AddCell(obsTable, "", sectionFont, 70f, "fina");
			doc.Add(obsTable);

			PdfPTable assignTable = new PdfPTable(3);
			assignTable.WidthPercentage = 75;
			assignTable.SetWidths(new float[] { 30f, 30f, 40f });
			assignTable.HorizontalAlignment = Element.ALIGN_LEFT;
			assignTable.SpacingBefore = 35;
			PdfPCell cellBot = new PdfPCell(new Phrase("O Colaborador:", sectionFont))
			{
				VerticalAlignment = Element.ALIGN_BOTTOM,
				FixedHeight = 50,
				Border = PdfPCell.NO_BORDER
			};
			assignTable.AddCell(cellBot);
			AddCellUL(assignTable, "", sectionFont, 50, Element.ALIGN_RIGHT);
			cellBot = new PdfPCell(new Phrase("O Responsável:", sectionFont))
			{
				HorizontalAlignment = Element.ALIGN_CENTER,
				VerticalAlignment = Element.ALIGN_BOTTOM,
				FixedHeight = 50,
				Border = PdfPCell.NO_BORDER
			};
			assignTable.AddCell(cellBot);

			iTextSharp.text.Image imgBot = iTextSharp.text.Image.GetInstance("Sources\\Assign.png");
			imgBot.ScaleAbsolute(100, 60f);
			imgBot.SetAbsolutePosition(doc.PageSize.Width - 190, doc.BottomMargin + 125);
			doc.Add(imgBot);
			doc.Add(assignTable);
			doc.Add(new Paragraph("\n"));
			doc.Add(new Paragraph("Nota: valores recebidos até dia 16 do mês N, serão pagos no mês N, valores recebidos entre dia 17 e 31 do mês N serão pagos no mês N+1", sectionFont));
			doc.Close();
			Console.WriteLine("PDF Gerado com sucesso: " + filePath);
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

			PdfPCell rightCell = new PdfPCell(new Phrase((totalKM * 0.36).ToString() + " €", subTitleFont));
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
			string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Relatorios\\Relatorio_Notion_Teresa Nunes.pdf");
			if (!File.Exists(filePath))
			{
				MessageBox.Show("Arquivo PDF não encontrado: " + filePath, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			PdfViewer pdfViewer = new PdfViewer();
			pdfViewer.Dock = DockStyle.Fill;
			pdfViewer.Document = PdfiumViewer.PdfDocument.Load(filePath); 
			panelPDF.Controls.Add(pdfViewer);
		}
	}
}
