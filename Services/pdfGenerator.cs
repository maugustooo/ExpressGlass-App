using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Gerador_ecxel;
using static iTextSharp.text.pdf.codec.TiffWriter;
using static Gerador_ecxel.Form1;

namespace Gerador_PDF.Services
{
    public partial class pdfGenerator
    {
		private string _colaborador;
		private string _data;
		private string _loja;
		private string _cod;
		private string _status;
		private string _matricula;
		private Form1 _form1;
		BaseFont bfCalibri;
		BaseFont bfcalibriBold ;
		BaseFont bfcalibriBIt;
		BaseFont bfVerdana;

		string sourceDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Source");

		void generateFont()
		{
			bfCalibri = BaseFont.CreateFont(Path.Combine(sourceDir, "calibri-regular.ttf"), BaseFont.WINANSI, BaseFont.EMBEDDED);
			bfcalibriBold = BaseFont.CreateFont(Path.Combine(sourceDir, "calibri-bold.ttf"), BaseFont.WINANSI, BaseFont.EMBEDDED);
			bfcalibriBIt = BaseFont.CreateFont(Path.Combine(sourceDir, "calibri-bold-italic.ttf"), BaseFont.WINANSI, BaseFont.EMBEDDED);
			bfVerdana = BaseFont.CreateFont(Path.Combine(sourceDir, "verdana.ttf"), BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
		}
		public pdfGenerator(Form1 form)
		{
			generateFont();
			_form1 = form;
		}
		public pdfGenerator(List<string[]> entries, Form1 form, string folderPath)
		{
			generateFont();
			_form1 = form;
			string previousName = "";
			int pdfCount = 0;
			_form1.UpdateStatusPdf("A gerar PDF's...");
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
					GeneratePdf(entries, folderPath);
					pdfCount++;
					previousName = _colaborador;
				}
			}
			if (pdfCount <= 0)
			{
				MessageBox.Show("Nenhum Dado para gerar", "Aviso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
				_form1.UpdateStatusPdf("");
				return;
			}
			_form1.UpdateStatusPdf("");
			MessageBox.Show("PDF gerado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		public void GenerateConsola(List<DadosPainel> dadosPainel)
		{
			generateFont();
			string folderPath = Path.Combine(Application.StartupPath, "PDFs-consola");
			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);
			string filePath = Path.Combine(folderPath, $"Consola" + "_" + $"{DateTime.Now:yyyy-MM-dd_HH-mm}.pdf");
			Document doc = new Document(PageSize.A4);
			if (doc == null)
				throw new Exception("O documento não foi inicializado corretamente.");
			PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
			doc.Open();

			var titleFont = FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD);
			var subFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.UNDEFINED);
			var normalFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL);

			string logoPath = Path.Combine(sourceDir, "logo.jpeg");
			if (File.Exists(logoPath))
			{
				iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
				logo.ScaleAbsolute(160, 35);
				logo.Alignment = Element.ALIGN_LEFT;
				doc.Add(logo);
			}

			doc.Add(new Paragraph("\n"));

			// Cabeçalho - Loja / Mês
			PdfPTable header = new PdfPTable(2);
			header.WidthPercentage = 100;
			header.SetWidths(new float[] { 1, 1 });

			AddHeaderCell(header, "Loja:", normalFont);
			AddHeaderCell(header, "Mês:", normalFont);
			doc.Add(header);

			doc.Add(new Paragraph("\n"));

			AddSection(doc, sourceDir, "Vidros Substituídos:", "icone_vidro_substituido.png", subFont,titleFont,
				dadosPainel.Where(d => d._title == "Serviços").Select(d => d.text).ToList());

			AddSection(doc, sourceDir, "Vidros Reparados:", "icone_vidro_reparado.png", subFont,titleFont,
				dadosPainel.Where(d => d._title == "Vidros reparados").Select(d => d.text).ToList());

			AddSection(doc, sourceDir, "Vendas Complementares:", "icone_carrinho.png", subFont,titleFont,
				dadosPainel.Where(d => d._title == "Vendas Complementares").Select(d => d.text).ToList());

			AddSection(doc, sourceDir, "Eficiência (FTE):", "icone_eficiencia.png", subFont,titleFont,
				dadosPainel.Where(d => d._title == "Efeciência(FTE)").Select(d => d.text).ToList());

			AddSection(doc, sourceDir, "Venda de Escovas:", "icone_escovas.png", subFont,titleFont,
				dadosPainel.Where(d => d._title == "Venda de escovas").Select(d => d.text).ToList());


			doc.Close();
		}

		private void AddHeaderCell(PdfPTable table, string label, iTextSharp.text.Font font)
		{
			PdfPCell cell = new PdfPCell(new Phrase(label + " ____________________", font));
			cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
			cell.PaddingBottom = 10f;
			table.AddCell(cell);
		}

		private void AddSection(Document doc, string sourceDir, string label, string iconFile,
			iTextSharp.text.Font lineFont, iTextSharp.text.Font titleFont, List<string> text)
		{
			PdfPTable table = new PdfPTable(2);
			table.WidthPercentage = 100;
			table.SetWidths(new float[] { 1, 9 });

			// Imagem
			string iconPath = Path.Combine(sourceDir, iconFile);
			iTextSharp.text.Image icon = File.Exists(iconPath) ? iTextSharp.text.Image.GetInstance(iconPath) : null;
			if (icon != null)
			{
				icon.ScaleAbsolute(25, 25);
				PdfPCell imgCell = new PdfPCell(icon);
				imgCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
				imgCell.VerticalAlignment = Element.ALIGN_TOP;
				table.AddCell(imgCell);
			}
			else
			{
				table.AddCell(""); // célula vazia se não houver imagem
			}

			// Texto (título + observações)
			PdfPCell textCell = new PdfPCell();
			textCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

			Paragraph p = new Paragraph();

			// Título sublinhado
			var underlinedFont = new iTextSharp.text.Font(titleFont);
			underlinedFont.SetStyle(iTextSharp.text.Font.UNDERLINE);
			p.Add(new Chunk(label + "\n", underlinedFont));

			// Observações normais
			if (text != null && text.Count > 0)
			{
				foreach (var obs in text)
				{
					p.Add(new Chunk("- " + obs + "\n", lineFont));
				}
			}
			else
			{
				for (int i = 0; i < 3; i++)
				{
					p.Add(new Chunk("__________________________________________________________\n", lineFont));
				}
			}

			textCell.AddElement(p);
			table.AddCell(textCell);

			doc.Add(table);
			doc.Add(new Paragraph("\n"));
		}



		public void GeneratePdf(List<string[]> entries, string folderPath)
		{
			var baseColor = new BaseColor(75, 85, 87, 255);
			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);
			if (string.IsNullOrEmpty(_colaborador) || string.IsNullOrEmpty(_data) || string.IsNullOrEmpty(folderPath))
			{
				throw new Exception("Variáveis de nome do arquivo estão nulas ou vazias.");
			}
			string filePath = Path.Combine(folderPath, $"Relatorio_{_colaborador}" + "_" + $"{DateTime.Now:yyyy-MM-dd_HH-mm}.pdf");
			Document doc = new Document(PageSize.A4);
			if (doc == null)
				throw new Exception("O documento não foi inicializado corretamente.");
			PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

			doc.Open();

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
			Paragraph title = new Paragraph("DESPESAS DE KM EM VIATURA PRÓPRIA\n", titleFont)
			{
				Alignment = Element.ALIGN_CENTER
			};
			doc.Add(title);
			doc.Add(new Paragraph("\n"));

			doc.Add(new Paragraph("Identificação: \n\n", subTitleFont));

			addTable(doc, 100, new float[] { 30f, 70f }, new (string, string, int, string, string, int?)[]{
			("Utilizador:", _colaborador, 14, "nenhuma", "cheia", null)
			}, sectionFont);

			addTable(doc, 60, new float[] { 30f, 30f }, new (string, string, int, string, string, int?)[] {
				("Nº Colaborador:", _cod, 14, "nenhuma", "cheia",  Element.ALIGN_CENTER),
				("Empresa:", "Expressglass SA", 14, "nenhuma", "cheia",  Element.ALIGN_CENTER),
				("Centro de Custo:", _loja, 14, "nenhuma", "cheia", null),
			}, sectionFont);

			doc.Add(new Paragraph("Despesas - Mapa de Km \n\n", subTitleFont));

			addTable(doc, 60, new float[] { 30f, 30f }, new (string, string, int, string, string, int?)[] {
				("Data", _data, 14, "nenhuma", "cheia", null),
				("Matrícula:", _matricula, 14, "nenhuma", "cheia", null),
			}, sectionFont);


			addTable(doc, 100, new float[] { 30f, 70f }, new (string, string, int, string, string, int?)[]{
			("Proprietário:", _colaborador, 14, "nenhuma", "cheia", null)
			}, sectionFont);

			PdfPTable table = new PdfPTable(6);
			table.WidthPercentage = 100;
			table.SetWidths(new float[] { 25f, 15f, 15f, 10f, 25f, 45f });

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

			PdfPCell textCell = new PdfPCell(new Phrase("O Responsável:", sectionFont))
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
			doc.Add(new Paragraph("Nota: valores recebidos até dia 16 do mês N, serão pagos no mês N, valores recebidos entre dia 17 e 31 do mês N serão pagos no mês N+1", sectionFont));
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

			PdfPCell rightCell = new PdfPCell(new Phrase(Math.Round(totalKM * 0.36, 2).ToString("0.00") + " €", subTitleFont));
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
	}
}
