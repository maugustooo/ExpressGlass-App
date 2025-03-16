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
				string name;

				for (int i = 0; i < entries.Count; i++)
				{
					_data = entries[i][0];
					_colaborador = entries[i][1];
					_matricula = entries[i][2];
					_localidade = entries[i][3];
					_motivo = entries[i][4];
					_klm = entries[i][5];
					_status = entries[i][6];
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
			string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"Relatorio_Notion_{_colaborador}.pdf");
			Document doc = new Document(PageSize.A4);
			PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
			doc.Open();

			BaseFont bfArial = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.EMBEDDED);
			iTextSharp.text.Font titleFont = new iTextSharp.text.Font(bfArial, 16, iTextSharp.text.Font.BOLD);
			iTextSharp.text.Font tableHeaderFont = new iTextSharp.text.Font(bfArial, 12, iTextSharp.text.Font.BOLD);
			iTextSharp.text.Font tableFont = new iTextSharp.text.Font(bfArial, 10, iTextSharp.text.Font.NORMAL);
			iTextSharp.text.Font sectionFont = new iTextSharp.text.Font(bfArial, 12, iTextSharp.text.Font.BOLD);
			iTextSharp.text.Font boldFont = new iTextSharp.text.Font(bfArial, 10, iTextSharp.text.Font.BOLD);

			Paragraph title = new Paragraph("DESPESAS DE KM EM VIATURA PRÓPRIA\r\n", titleFont)
			{
				Alignment = Element.ALIGN_CENTER
			};
			doc.Add(title);
			doc.Add(new Paragraph("\n"));

			PdfPTable infoTable = new PdfPTable(2);
			infoTable.WidthPercentage = 100;
			infoTable.SetWidths(new float[] { 30f, 70f });
			AddCell(infoTable, "Identificação:", sectionFont, true);
			AddCell(infoTable, "", tableFont, false);
			AddCell(infoTable, "Utilizador:", sectionFont, true);
			AddCell(infoTable, _colaborador, tableFont, false);
			AddCell(infoTable, "Empresa:", sectionFont, true);
			AddCell(infoTable, "Expressglass SA", tableFont, false);
			doc.Add(infoTable);
			doc.Add(new Paragraph("\n"));

			PdfPTable dataTable = new PdfPTable(2);
			infoTable.WidthPercentage = 100;
			infoTable.SetWidths(new float[] { 30f, 70f });
			AddCell(dataTable, "Despesas - Mapa de Km:", sectionFont, true);
			AddCell(dataTable, "Data:", sectionFont, true);
			AddCell(dataTable, _data, tableFont, false);
			AddCell(dataTable, "Matrícula:", sectionFont, true);
			AddCell(dataTable, _matricula, tableFont, false);
			AddCell(dataTable, "Proprietario:", sectionFont, true);
			AddCell(dataTable, _colaborador, tableFont, false);
			doc.Add(dataTable);
			doc.Add(new Paragraph("\n"));

			PdfPTable table = new PdfPTable(5);
			table.WidthPercentage = 100;
			table.SetWidths(new float[] { 15f, 15f, 15f, 10f, 45f });

			string[] headers = { "Dia", "Saída", "Chegada", "Km's", "Local" };
			foreach (var header in headers)
			{
				PdfPCell cell = new PdfPCell(new Phrase(header, tableHeaderFont))
				{
					BackgroundColor = new BaseColor(100, 149, 237), // Azul Médio
					HorizontalAlignment = Element.ALIGN_CENTER,
					Padding = 5f
				};
				table.AddCell(cell);
			}

			foreach (var entry in entries)
			{
				string status = entry[6];
				if (status != "Novo") continue;
				string[] row = { entry[0], "09H00", "18H00", entry[5], entry[3] };
				foreach (var dataValue in row)
				{
					PdfPCell cell = new PdfPCell(new Phrase(dataValue, tableFont))
					{
						HorizontalAlignment = Element.ALIGN_CENTER,
						Padding = 5f
					};
					table.AddCell(cell);
				}
			}
			doc.Add(table);
			doc.Add(new Paragraph("\n"));

			PdfPTable summaryTable = new PdfPTable(2);
			summaryTable.WidthPercentage = 100;
			summaryTable.SetWidths(new float[] { 50f, 50f });
			AddCell(summaryTable, "Total Km:", sectionFont, true);
			AddCell(summaryTable, _klm.ToString(), tableFont, false);
			AddCell(summaryTable, "Valor/Km:", sectionFont, true);
			AddCell(summaryTable, "0,36 €", tableFont, false);
			AddCell(summaryTable, "Total de Despesas:", sectionFont, true);
			AddCell(summaryTable, "46,80 €", tableFont, false);
			doc.Add(summaryTable);
			doc.Add(new Paragraph("\n"));

			doc.Add(new Paragraph("Observações:", sectionFont));
			doc.Add(new Paragraph("O Colaborador: " + _colaborador, tableFont));
			doc.Add(new Paragraph("O Responsável:", tableFont));
			doc.Close();
			Console.WriteLine("PDF Gerado com sucesso: " + filePath);
		}

		private void AddCell(PdfPTable table, string text, iTextSharp.text.Font font, bool isHeader)
		{
			PdfPCell cell = new PdfPCell(new Phrase(text, font))
			{
				BackgroundColor = isHeader ? new BaseColor(230, 230, 230) : BaseColor.WHITE,
				HorizontalAlignment = Element.ALIGN_LEFT,
				Padding = 5f
			};
			table.AddCell(cell);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Relatorio_Notion_Teresa Nunes.pdf");
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
