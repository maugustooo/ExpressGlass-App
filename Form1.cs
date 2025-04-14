using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;
using Gerador_PDF.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Gerador_ecxel
{

	public partial class Form1 : Form
	{
		private readonly NotionService notionService;
		private string _folderPath = Path.Combine(Application.StartupPath, "PDFs");
		public Form1(Config config)
		{
			InitializeComponent();
			notionService = new NotionService(config.NotionApiKey, config.NotionDatabaseId, config.NotionDatabaseIdKPIs, config.NotionDatabaseIdLojas);
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
				var notionKeyKPIs = config.NotionDatabaseIdKPIs;
				var notionKeyLojas = config.NotionDatabaseIdLojas;
				if (notionKey == null || databaseId == null)
				{
					progressBar1.Visible = false;
					statusLabel.Visible = false;
					MessageBox.Show("Erro: Chaves de API não foram definidas.");
					return;
				}
				NotionService notionService = new NotionService(notionKey, databaseId, notionKeyKPIs, notionKeyLojas);
				List<string[]> entries = await notionService.GetDatabase();
				pdfGenerator pdfGenerator = new pdfGenerator(entries, this, _folderPath);
			}
			catch (Exception ex)
			{
				progressBar1.Visible = false;
				statusLabel.Visible = false;
				MessageBox.Show($"Erro ao gerar PDF: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		public void UpdateStatusPdf(string statusText)
		{
			if (statusText != "")
			{
				statusLabel.Text = statusText;
			}
			else
			{
				progressBar1.Visible = false;
				statusLabel.Visible = false;
			}
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
				MessageBox.Show("A atualização falhou ou foi cancelada", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
			else
				MessageBox.Show("Atualização feita com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private async void button3_Click(object sender, EventArgs e)
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
					var selectedItem = comboBox1.SelectedItem != null ? comboBox1.SelectedItem.ToString() : "";
					readExcel readExcel = new readExcel(excelPath, selectedItem);

					statusLabel2.Text = "A atualizar Dados...";
					statusLabel2.Visible = true;
					statusLabel2.ForeColor = Color.Blue;
					progressBar3.Style = ProgressBarStyle.Marquee;
					progressBar3.Visible = true;
					await notionService.UpdateNotionDatabase(readExcel.faturados, readExcel.complementares, readExcel.monthStores, readExcel.mes);

					statusLabel2.Visible = false;
					progressBar3.Visible = false;
					MessageBox.Show("Dados atualizados no Notion!");
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

		private async void button4_Click(object sender, EventArgs e)
		{
			string mes = comboBox1.SelectedItem?.ToString();
			if (!string.IsNullOrEmpty(mes))
			{
				await notionService.DeleteMonth(mes + " 2025");
				MessageBox.Show($"Registros do mês '{mes}' foram eliminados.");
			}
			else
			{
				MessageBox.Show("Selecione um mês para eliminar.");
			}
		}
	}
}