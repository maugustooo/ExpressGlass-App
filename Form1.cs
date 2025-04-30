using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;
using Gerador_PDF.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Data.Sqlite;

namespace Gerador_ecxel
{

	public partial class Form1 : Form
	{
		private readonly NotionService notionService;
		private string _folderPath = Path.Combine(Application.StartupPath, "PDFs");
		public Form1(Config config)
		{
			InitializeComponent();
			//consola c = new consola(config, this);
			//c.createDataBase();
			notionService = new NotionService(config.NotionApiKey, config.NotionDatabaseId, config.NotionDatabaseIdKPIs, config.NotionDatabaseIdLojas, config.NotionDatabaseIdStockParado);
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
				var notionKeyStockParado = config.NotionDatabaseIdStockParado;
				if (notionKey == null || databaseId == null)
				{
					progressBar1.Visible = false;
					statusLabel.Visible = false;
					MessageBox.Show("Erro: Chaves de API não foram definidas.");
					return;
				}
				NotionService notionService = new NotionService(notionKey, databaseId, notionKeyKPIs, notionKeyLojas, notionKeyStockParado);
				List<string[]> entries = await notionService.GetDatabase();
				pdfGenerator pdfGenerator = new pdfGenerator(entries, this, _folderPath);
				notionService.UpdateStatus();
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
			var configPath = Path.Combine(Application.StartupPath, "NotionConfig.json");

			var newConfig = NotionConfigHelper.updateConfig(configPath);
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
					readExcel readExcel = new readExcel(excelPath, selectedItem, "kpi");

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
		public void CarregarLojasNoComboBox()
		{
			//using (var conn = new SqliteConnection("Data Source=lojas.db"))
			//{
			//	conn.Open();
			//	string sql = "SELECT Nome FROM Lojas";

			//	using (var cmd = new SqliteCommand(sql, conn))
			//	using (var reader = cmd.ExecuteReader())
			//	{
			//		comboBoxLojas.Items.Clear(); // Limpa antes de carregar
			//		while (reader.Read())
			//		{
			//			string nome = reader.GetString(0);
			//			comboBoxLojas.Items.Add(nome);
			//		}
			//	}
			//}
		}
		private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private async void button5_Click(object sender, EventArgs e)
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
					readExcel readExcel = new readExcel(excelPath, "", "stockParado");

					label2.Text = "A atualizar Dados...";
					label2.Visible = true;
					label2.ForeColor = Color.Blue;
					progressBar4.Style = ProgressBarStyle.Marquee;
					progressBar4.Visible = true;
					await notionService.updateStockParado(readExcel.stockParado);

					label2.Visible = false;
					progressBar4.Visible = false;
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
	}
}