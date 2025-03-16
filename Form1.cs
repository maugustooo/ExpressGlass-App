using System.IO;
using PdfiumViewer;
using iTextSharp.text;
using iTextSharp.text.pdf;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;
using System.Windows.Forms.VisualStyles;

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

			string repoRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;

			string fontPath = Path.Combine(repoRoot, "Fontes\\calibri-regular.ttf");
			BaseFont bfCalibri = BaseFont.CreateFont(fontPath, BaseFont.WINANSI, BaseFont.EMBEDDED);

			fontPath = Path.Combine(repoRoot, "Fontes\\calibri-bold.ttf");
			BaseFont bfcalibriBold = BaseFont.CreateFont(fontPath, BaseFont.WINANSI, BaseFont.EMBEDDED);

			fontPath = Path.Combine(repoRoot, "Fontes\\calibri-bold-italic.ttf");
			BaseFont bfcalibriBIt = BaseFont.CreateFont(fontPath, BaseFont.WINANSI, BaseFont.EMBEDDED);

			fontPath = Path.Combine(repoRoot, "Fontes\\verdana.ttf");
			BaseFont bfVerdana = BaseFont.CreateFont(fontPath, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);

			iTextSharp.text.Font titleFont = new iTextSharp.text.Font(bfcalibriBold, 15, iTextSharp.text.Font.BOLD);
			iTextSharp.text.Font subTitleFont = new iTextSharp.text.Font(bfVerdana, 12, iTextSharp.text.Font.BOLD, new BaseColor(75, 86, 98, 255));
			iTextSharp.text.Font tableHeaderFont = new iTextSharp.text.Font(bfcalibriBIt, 11, iTextSharp.text.Font.BOLD);
			iTextSharp.text.Font tableFont = new iTextSharp.text.Font(bfCalibri, 10, iTextSharp.text.Font.NORMAL);
			iTextSharp.text.Font sectionFont = new iTextSharp.text.Font(bfVerdana, 11, iTextSharp.text.Font.NORMAL);
			iTextSharp.text.Font boldFont = new iTextSharp.text.Font(bfcalibriBold, 9, iTextSharp.text.Font.BOLD);
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

			AddCell(identificacaoTable, "Utilizador:", sectionFont, 25f, "nenhuma");
			AddCell(identificacaoTable, _colaborador, sectionFont, 25f, "cheia");
			AddCell(identificacaoTable, "Nº Colaborador:", sectionFont, 25f, "nenhuma");
			AddCell(identificacaoTable, "171", sectionFont, 25f, "cheia");
			AddCell(identificacaoTable, "Empresa:", sectionFont, 25f, "nenhuma");
			AddCell(identificacaoTable, "Expressglass SA", sectionFont, 25f, "cheia");
			AddCell(identificacaoTable, "Centro de Custo:", sectionFont, 25f, "nenhuma");
			AddCell(identificacaoTable, "BARCELOS", sectionFont, 25f, "cheia");

			doc.Add(identificacaoTable);
			doc.Add(new Paragraph("\n"));

			doc.Add(new Paragraph("Despesas - Mapa de Km \n\n", subTitleFont));
			PdfPTable DespesasTable = new PdfPTable(2);
			DespesasTable.WidthPercentage = 100;
			DespesasTable.SetWidths(new float[] { 30f, 70f });
			AddCell(DespesasTable, "Data",sectionFont, 25f, "nenhuma");
			AddCell(DespesasTable, _data, sectionFont, 25f, "Cheia");
			AddCell(DespesasTable, "Matrícula:", sectionFont, 25f, "nenhuma");
			AddCell(DespesasTable, _matricula, sectionFont, 25f, "cheia");
			AddCell(DespesasTable, "Proprietário:", sectionFont, 25f, "nenhuma");
			AddCell(DespesasTable, "IDFK", sectionFont, 25f, "cheia");
			doc.Add(DespesasTable);
			doc.Add(new Paragraph("\n"));


			PdfPTable table = new PdfPTable(6);
			table.WidthPercentage = 100;
			table.SetWidths(new float[] { 15f, 15f, 15f, 10f, 45f, 45f});

			string[] headers = { "Dia", "Saída", "Chegada", "Km's", "Local", "Motivo" };
			foreach (var header in headers)
			{
				PdfPCell cell = new PdfPCell(new Phrase(header, tableHeaderFont))
				{
					HorizontalAlignment = Element.ALIGN_CENTER,
					Padding = 5f
				};
				table.AddCell(cell);
			}

			foreach (var entry in entries)
			{
				string status = entry[6];
				if (status != "Novo") continue;
				string[] row = { entry[0], "09H00", "18H00", entry[5], entry[3], entry[4] };
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
			AddCell(summaryTable, "Total Km:", sectionFont, 20f, "nenhuma");
			AddCell(summaryTable, _klm.ToString(), tableFont, 20f, "nenhuma");
			AddCell(summaryTable, "Valor/Km:", sectionFont, 20f, "nenhuma");
			AddCell(summaryTable, "0,36 €", tableFont, 20f, "nenhuma");
			AddCell(summaryTable, "Total de Despesas:", sectionFont, 20f, "cheia");
			AddCell(summaryTable, "46,80 €", tableFont, 20f, "cheia");
			doc.Add(summaryTable);
			doc.Add(new Paragraph("\n"));

			doc.Add(new Paragraph("Observações:", sectionFont));
			doc.Add(new Paragraph("O Colaborador: " + _colaborador, tableFont));
			doc.Add(new Paragraph("O Responsável:", tableFont));
			doc.Close();
			Console.WriteLine("PDF Gerado com sucesso: " + filePath);
		}

		private void AddCell(PdfPTable table, string text, iTextSharp.text.Font font, float height, string borderType)
		{
			PdfPCell cell = new PdfPCell(new Phrase(text, font))
			{
				HorizontalAlignment = Element.ALIGN_LEFT,
				Padding = 5f,
				FixedHeight = height
			};
			if (borderType == "nenhuma")
			{
				cell.Border = PdfPCell.NO_BORDER;
			}
			else if (borderType == "fina")
			{
				cell.BorderWidth = 0.5f;
			}
			else if (borderType == "cheia")
			{
				cell.BorderWidth = 1.5f;
			}
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
