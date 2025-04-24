namespace Gerador_ecxel
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			bindingSource1 = new BindingSource(components);
			tabPage2 = new TabPage();
			label4 = new Label();
			statusLabel2 = new Label();
			progressBar3 = new ProgressBar();
			panel3 = new Panel();
			button4 = new Button();
			deleteButton = new Button();
			progressBar2 = new ProgressBar();
			comboBox1 = new ComboBox();
			panel4 = new Panel();
			label2 = new Label();
			pictureBox2 = new PictureBox();
			button3 = new Button();
			tabPage1 = new TabPage();
			panel2 = new Panel();
			resetData = new Button();
			PdfDirButton = new Button();
			panel1 = new Panel();
			label1 = new Label();
			pictureBox1 = new PictureBox();
			GerarPdf = new Button();
			statusLabel = new Label();
			progressBar1 = new ProgressBar();
			tabControl1 = new TabControl();
			tabPage5 = new TabPage();
			label6 = new Label();
			progressBar4 = new ProgressBar();
			panel5 = new Panel();
			button1 = new Button();
			button5 = new Button();
			panel7 = new Panel();
			label3 = new Label();
			pictureBox4 = new PictureBox();
			backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			label5 = new Label();
			((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
			tabPage2.SuspendLayout();
			panel3.SuspendLayout();
			panel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
			tabPage1.SuspendLayout();
			panel2.SuspendLayout();
			panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			tabControl1.SuspendLayout();
			tabPage5.SuspendLayout();
			panel5.SuspendLayout();
			panel7.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
			SuspendLayout();
			// 
			// tabPage2
			// 
			tabPage2.BackColor = Color.Gainsboro;
			tabPage2.BackgroundImageLayout = ImageLayout.None;
			tabPage2.Controls.Add(label5);
			tabPage2.Controls.Add(label4);
			tabPage2.Controls.Add(statusLabel2);
			tabPage2.Controls.Add(progressBar3);
			tabPage2.Controls.Add(panel3);
			tabPage2.Controls.Add(comboBox1);
			tabPage2.Controls.Add(panel4);
			tabPage2.Controls.Add(button3);
			tabPage2.Font = new Font("Segoe UI", 25F);
			tabPage2.Location = new Point(4, 24);
			tabPage2.Name = "tabPage2";
			tabPage2.Padding = new Padding(3);
			tabPage2.Size = new Size(905, 503);
			tabPage2.TabIndex = 1;
			tabPage2.Text = "KPI'S Diários";
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Font = new Font("Segoe UI", 17F);
			label4.Location = new Point(278, 98);
			label4.Name = "label4";
			label4.Size = new Size(268, 31);
			label4.TabIndex = 14;
			label4.Text = "Selecionar Mês para NPS";
			// 
			// statusLabel2
			// 
			statusLabel2.AutoSize = true;
			statusLabel2.Font = new Font("Segoe UI", 23F);
			statusLabel2.Location = new Point(497, 330);
			statusLabel2.Name = "statusLabel2";
			statusLabel2.Size = new Size(0, 42);
			statusLabel2.TabIndex = 8;
			statusLabel2.TextAlign = ContentAlignment.MiddleCenter;
			statusLabel2.Visible = false;
			// 
			// progressBar3
			// 
			progressBar3.Location = new Point(404, 434);
			progressBar3.Name = "progressBar3";
			progressBar3.Size = new Size(328, 59);
			progressBar3.TabIndex = 9;
			progressBar3.Visible = false;
			// 
			// panel3
			// 
			panel3.BorderStyle = BorderStyle.FixedSingle;
			panel3.Controls.Add(button4);
			panel3.Controls.Add(deleteButton);
			panel3.Controls.Add(progressBar2);
			panel3.Dock = DockStyle.Left;
			panel3.Location = new Point(3, 83);
			panel3.Name = "panel3";
			panel3.Size = new Size(230, 417);
			panel3.TabIndex = 7;
			// 
			// button4
			// 
			button4.BackColor = Color.LightGray;
			button4.FlatAppearance.BorderSize = 0;
			button4.FlatAppearance.MouseOverBackColor = Color.Black;
			button4.FlatStyle = FlatStyle.Flat;
			button4.Font = new Font("Segoe UI", 13F);
			button4.Location = new Point(37, 23);
			button4.Name = "button4";
			button4.Size = new Size(160, 150);
			button4.TabIndex = 14;
			button4.Text = "Atualizar Dados da API";
			button4.UseVisualStyleBackColor = false;
			button4.Click += resetData_Click;
			button4.MouseEnter += GerarPdf_MouseEnter;
			button4.MouseLeave += GerarPdf_MouseLeave;
			// 
			// deleteButton
			// 
			deleteButton.BackColor = Color.LightGray;
			deleteButton.FlatAppearance.BorderSize = 0;
			deleteButton.FlatAppearance.MouseOverBackColor = Color.Black;
			deleteButton.FlatStyle = FlatStyle.Flat;
			deleteButton.Font = new Font("Segoe UI", 17F);
			deleteButton.Location = new Point(37, 204);
			deleteButton.Name = "deleteButton";
			deleteButton.Size = new Size(160, 121);
			deleteButton.TabIndex = 13;
			deleteButton.Text = "Eliminar Mês";
			deleteButton.UseVisualStyleBackColor = false;
			deleteButton.Click += button4_Click;
			deleteButton.Enter += GerarPdf_MouseEnter;
			deleteButton.Leave += GerarPdf_MouseLeave;
			deleteButton.MouseEnter += GerarPdf_MouseEnter;
			deleteButton.MouseLeave += GerarPdf_MouseLeave;
			// 
			// progressBar2
			// 
			progressBar2.Location = new Point(37, 344);
			progressBar2.Name = "progressBar2";
			progressBar2.Size = new Size(160, 46);
			progressBar2.TabIndex = 12;
			progressBar2.Visible = false;
			// 
			// comboBox1
			// 
			comboBox1.FormattingEnabled = true;
			comboBox1.Items.AddRange(new object[] { "Janeiro", "Fevereiro", "Março", "Abril", "Junho", "Julho", "Agosto", "Setembro", "Novembro", "Dezembro" });
			comboBox1.Location = new Point(278, 166);
			comboBox1.Name = "comboBox1";
			comboBox1.Size = new Size(281, 53);
			comboBox1.TabIndex = 8;
			// 
			// panel4
			// 
			panel4.BackColor = SystemColors.HotTrack;
			panel4.Controls.Add(label2);
			panel4.Controls.Add(pictureBox2);
			panel4.Dock = DockStyle.Top;
			panel4.Location = new Point(3, 3);
			panel4.Name = "panel4";
			panel4.Size = new Size(899, 80);
			panel4.TabIndex = 6;
			// 
			// label2
			// 
			label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			label2.AutoSize = true;
			label2.BackColor = Color.FromArgb(7, 43, 101);
			label2.Font = new Font("Segoe UI", 24.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
			label2.ForeColor = Color.Transparent;
			label2.Location = new Point(454, 17);
			label2.Name = "label2";
			label2.Size = new Size(324, 45);
			label2.TabIndex = 7;
			label2.Text = "ExpressGlass - KPI´s";
			// 
			// pictureBox2
			// 
			pictureBox2.BackColor = Color.FromArgb(7, 43, 101);
			pictureBox2.Dock = DockStyle.Fill;
			pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
			pictureBox2.Location = new Point(0, 0);
			pictureBox2.Name = "pictureBox2";
			pictureBox2.Size = new Size(899, 80);
			pictureBox2.TabIndex = 0;
			pictureBox2.TabStop = false;
			// 
			// button3
			// 
			button3.BackColor = Color.Transparent;
			button3.Font = new Font("Segoe UI", 15F);
			button3.Location = new Point(569, 166);
			button3.Name = "button3";
			button3.Size = new Size(328, 53);
			button3.TabIndex = 0;
			button3.Text = "Importar Excel";
			button3.UseVisualStyleBackColor = false;
			button3.Click += button3_Click;
			// 
			// tabPage1
			// 
			tabPage1.BackColor = Color.Gainsboro;
			tabPage1.BackgroundImageLayout = ImageLayout.None;
			tabPage1.Controls.Add(panel2);
			tabPage1.Controls.Add(panel1);
			tabPage1.Controls.Add(GerarPdf);
			tabPage1.Controls.Add(statusLabel);
			tabPage1.Controls.Add(progressBar1);
			tabPage1.Font = new Font("Segoe UI", 25F);
			tabPage1.Location = new Point(4, 24);
			tabPage1.Name = "tabPage1";
			tabPage1.Padding = new Padding(3);
			tabPage1.Size = new Size(905, 503);
			tabPage1.TabIndex = 0;
			tabPage1.Text = "Mapa KLM";
			// 
			// panel2
			// 
			panel2.BorderStyle = BorderStyle.FixedSingle;
			panel2.Controls.Add(resetData);
			panel2.Controls.Add(PdfDirButton);
			panel2.Dock = DockStyle.Left;
			panel2.Location = new Point(3, 83);
			panel2.Name = "panel2";
			panel2.Size = new Size(201, 417);
			panel2.TabIndex = 7;
			// 
			// resetData
			// 
			resetData.BackColor = Color.LightGray;
			resetData.FlatAppearance.BorderSize = 0;
			resetData.FlatAppearance.MouseOverBackColor = Color.Black;
			resetData.FlatStyle = FlatStyle.Flat;
			resetData.Font = new Font("Segoe UI", 13F);
			resetData.Location = new Point(24, 18);
			resetData.Name = "resetData";
			resetData.Size = new Size(150, 150);
			resetData.TabIndex = 6;
			resetData.Text = "Atualizar Dados da API";
			resetData.UseVisualStyleBackColor = false;
			resetData.Click += resetData_Click;
			resetData.MouseEnter += GerarPdf_MouseEnter;
			resetData.MouseLeave += GerarPdf_MouseLeave;
			// 
			// PdfDirButton
			// 
			PdfDirButton.BackColor = Color.LightGray;
			PdfDirButton.FlatAppearance.BorderSize = 0;
			PdfDirButton.FlatAppearance.MouseOverBackColor = Color.Black;
			PdfDirButton.FlatStyle = FlatStyle.Flat;
			PdfDirButton.Font = new Font("Segoe UI", 13F);
			PdfDirButton.Location = new Point(24, 249);
			PdfDirButton.Name = "PdfDirButton";
			PdfDirButton.Size = new Size(150, 150);
			PdfDirButton.TabIndex = 5;
			PdfDirButton.Text = "Abrir Pasta Dos PDF´S";
			PdfDirButton.UseVisualStyleBackColor = false;
			PdfDirButton.Click += button1_Click;
			PdfDirButton.MouseEnter += GerarPdf_MouseEnter;
			PdfDirButton.MouseLeave += GerarPdf_MouseLeave;
			// 
			// panel1
			// 
			panel1.BackColor = SystemColors.HotTrack;
			panel1.Controls.Add(label1);
			panel1.Controls.Add(pictureBox1);
			panel1.Dock = DockStyle.Top;
			panel1.Location = new Point(3, 3);
			panel1.Name = "panel1";
			panel1.Size = new Size(899, 80);
			panel1.TabIndex = 6;
			// 
			// label1
			// 
			label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			label1.AutoSize = true;
			label1.BackColor = Color.FromArgb(7, 43, 101);
			label1.Font = new Font("Segoe UI", 24.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
			label1.ForeColor = Color.Transparent;
			label1.Location = new Point(380, 13);
			label1.Name = "label1";
			label1.Size = new Size(477, 45);
			label1.TabIndex = 7;
			label1.Text = "ExpressGlass - Mapas de KLM";
			// 
			// pictureBox1
			// 
			pictureBox1.BackColor = Color.FromArgb(7, 43, 101);
			pictureBox1.Dock = DockStyle.Fill;
			pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
			pictureBox1.Location = new Point(0, 0);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new Size(899, 80);
			pictureBox1.TabIndex = 0;
			pictureBox1.TabStop = false;
			// 
			// GerarPdf
			// 
			GerarPdf.BackColor = Color.Transparent;
			GerarPdf.Font = new Font("Segoe UI", 15F);
			GerarPdf.Location = new Point(383, 142);
			GerarPdf.Name = "GerarPdf";
			GerarPdf.Size = new Size(328, 66);
			GerarPdf.TabIndex = 0;
			GerarPdf.Text = "Gerar pdf";
			GerarPdf.UseVisualStyleBackColor = false;
			GerarPdf.Click += GerarPdf_Click;
			GerarPdf.MouseEnter += GerarPdf_MouseEnter;
			GerarPdf.MouseLeave += GerarPdf_MouseLeave;
			// 
			// statusLabel
			// 
			statusLabel.AutoSize = true;
			statusLabel.Font = new Font("Segoe UI", 23F);
			statusLabel.Location = new Point(403, 304);
			statusLabel.Name = "statusLabel";
			statusLabel.Size = new Size(0, 42);
			statusLabel.TabIndex = 3;
			statusLabel.TextAlign = ContentAlignment.MiddleCenter;
			statusLabel.Visible = false;
			// 
			// progressBar1
			// 
			progressBar1.Location = new Point(383, 408);
			progressBar1.Name = "progressBar1";
			progressBar1.Size = new Size(328, 59);
			progressBar1.TabIndex = 4;
			progressBar1.Visible = false;
			// 
			// tabControl1
			// 
			tabControl1.Controls.Add(tabPage1);
			tabControl1.Controls.Add(tabPage2);
			tabControl1.Controls.Add(tabPage5);
			tabControl1.Dock = DockStyle.Fill;
			tabControl1.Location = new Point(0, 0);
			tabControl1.Name = "tabControl1";
			tabControl1.SelectedIndex = 0;
			tabControl1.Size = new Size(913, 531);
			tabControl1.TabIndex = 5;
			// 
			// tabPage5
			// 
			tabPage5.BackColor = Color.Gainsboro;
			tabPage5.BackgroundImageLayout = ImageLayout.None;
			tabPage5.Controls.Add(label6);
			tabPage5.Controls.Add(progressBar4);
			tabPage5.Controls.Add(panel5);
			tabPage5.Controls.Add(button5);
			tabPage5.Controls.Add(panel7);
			tabPage5.Font = new Font("Segoe UI", 25F);
			tabPage5.Location = new Point(4, 24);
			tabPage5.Name = "tabPage5";
			tabPage5.Padding = new Padding(3);
			tabPage5.Size = new Size(905, 503);
			tabPage5.TabIndex = 4;
			tabPage5.Text = "Stock Parado";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Font = new Font("Segoe UI", 23F);
			label6.Location = new Point(499, 330);
			label6.Name = "label6";
			label6.Size = new Size(0, 42);
			label6.TabIndex = 17;
			label6.TextAlign = ContentAlignment.MiddleCenter;
			label6.Visible = false;
			// 
			// progressBar4
			// 
			progressBar4.Location = new Point(406, 434);
			progressBar4.Name = "progressBar4";
			progressBar4.Size = new Size(328, 59);
			progressBar4.TabIndex = 19;
			progressBar4.Visible = false;
			// 
			// panel5
			// 
			panel5.BorderStyle = BorderStyle.FixedSingle;
			panel5.Controls.Add(button1);
			panel5.Dock = DockStyle.Left;
			panel5.Location = new Point(3, 83);
			panel5.Name = "panel5";
			panel5.Size = new Size(230, 417);
			panel5.TabIndex = 16;
			// 
			// button1
			// 
			button1.BackColor = Color.LightGray;
			button1.FlatAppearance.BorderSize = 0;
			button1.FlatAppearance.MouseOverBackColor = Color.Black;
			button1.FlatStyle = FlatStyle.Flat;
			button1.Font = new Font("Segoe UI", 13F);
			button1.Location = new Point(30, 23);
			button1.Name = "button1";
			button1.Size = new Size(160, 150);
			button1.TabIndex = 14;
			button1.Text = "Atualizar Dados da API";
			button1.UseVisualStyleBackColor = false;
			button1.MouseEnter += GerarPdf_MouseEnter;
			button1.MouseLeave += GerarPdf_MouseLeave;
			// 
			// button5
			// 
			button5.BackColor = Color.Transparent;
			button5.Font = new Font("Segoe UI", 15F);
			button5.Location = new Point(406, 163);
			button5.Name = "button5";
			button5.Size = new Size(328, 53);
			button5.TabIndex = 15;
			button5.Text = "Importar Excel";
			button5.UseVisualStyleBackColor = false;
			button5.Click += button5_Click;
			// 
			// panel7
			// 
			panel7.BackColor = SystemColors.HotTrack;
			panel7.Controls.Add(label3);
			panel7.Controls.Add(pictureBox4);
			panel7.Dock = DockStyle.Top;
			panel7.Location = new Point(3, 3);
			panel7.Name = "panel7";
			panel7.Size = new Size(899, 80);
			panel7.TabIndex = 6;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.BackColor = Color.FromArgb(7, 43, 101);
			label3.Font = new Font("Segoe UI", 24.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
			label3.ForeColor = Color.Transparent;
			label3.Location = new Point(385, 15);
			label3.Name = "label3";
			label3.Size = new Size(450, 45);
			label3.TabIndex = 7;
			label3.Text = "ExpressGlass - Stock Parado";
			// 
			// pictureBox4
			// 
			pictureBox4.BackColor = Color.FromArgb(7, 43, 101);
			pictureBox4.Dock = DockStyle.Fill;
			pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
			pictureBox4.Location = new Point(0, 0);
			pictureBox4.Name = "pictureBox4";
			pictureBox4.Size = new Size(899, 80);
			pictureBox4.TabIndex = 0;
			pictureBox4.TabStop = false;
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Font = new Font("Segoe UI", 12F);
			label5.Location = new Point(299, 129);
			label5.Name = "label5";
			label5.Size = new Size(223, 21);
			label5.TabIndex = 15;
			label5.Text = "(Apenas para atualizar os NPS)";
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(913, 531);
			Controls.Add(tabControl1);
			Icon = (Icon)resources.GetObject("$this.Icon");
			Name = "Form1";
			Text = "PDF Generator";
			((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
			tabPage2.ResumeLayout(false);
			tabPage2.PerformLayout();
			panel3.ResumeLayout(false);
			panel4.ResumeLayout(false);
			panel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
			tabPage1.ResumeLayout(false);
			tabPage1.PerformLayout();
			panel2.ResumeLayout(false);
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			tabControl1.ResumeLayout(false);
			tabPage5.ResumeLayout(false);
			tabPage5.PerformLayout();
			panel5.ResumeLayout(false);
			panel7.ResumeLayout(false);
			panel7.PerformLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
			ResumeLayout(false);
		}

		#endregion
		private BindingSource bindingSource1;
		private Button buttonFilter;
		private CheckedListBox listBoxStores;
		private DataGridView dataGridView1;
		private TabPage tabPage2;
		private Label label4;
		private Label statusLabel2;
		private ProgressBar progressBar3;
		private Panel panel3;
		private Button button4;
		private Button deleteButton;
		private ProgressBar progressBar2;
		private ComboBox comboBox1;
		private Panel panel4;
		private Label label2;
		private PictureBox pictureBox2;
		private Button button3;
		private TabPage tabPage1;
		private Panel panel2;
		private Button resetData;
		private Button PdfDirButton;
		private Panel panel1;
		private Label label1;
		private PictureBox pictureBox1;
		private Button GerarPdf;
		private Label statusLabel;
		private ProgressBar progressBar1;
		private TabControl tabControl1;
		private TabPage tabPage5;
		private Panel panel7;
		private Label label3;
		private PictureBox pictureBox4;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private GroupBox groupBox6;
		private PictureBox pictureBox8;
		private GroupBox groupBox5;
		private PictureBox pictureBox7;
		private GroupBox groupBox4;
		private PictureBox pictureBox6;
		private GroupBox groupBox3;
		private PictureBox pictureBox5;
		private Label label6;
		private ProgressBar progressBar4;
		private Panel panel5;
		private Button button1;
		private Button button5;
		private Label label5;
	}
}
