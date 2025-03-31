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
			GerarPdf = new Button();
			statusLabel = new Label();
			progressBar1 = new ProgressBar();
			tabControl1 = new TabControl();
			tabPage1 = new TabPage();
			panel2 = new Panel();
			resetData = new Button();
			PdfDirButton = new Button();
			panel1 = new Panel();
			label1 = new Label();
			pictureBox1 = new PictureBox();
			tabPage2 = new TabPage();
			listBoxStores = new CheckedListBox();
			dataGridView1 = new DataGridView();
			buttonFilter = new Button();
			panel3 = new Panel();
			button1 = new Button();
			button2 = new Button();
			panel4 = new Panel();
			label2 = new Label();
			pictureBox2 = new PictureBox();
			button3 = new Button();
			bindingSource1 = new BindingSource(components);
			tabControl1.SuspendLayout();
			tabPage1.SuspendLayout();
			panel2.SuspendLayout();
			panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
			panel3.SuspendLayout();
			panel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
			((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
			SuspendLayout();
			// 
			// GerarPdf
			// 
			GerarPdf.BackColor = Color.Transparent;
			GerarPdf.Font = new Font("Segoe UI", 15F);
			GerarPdf.Location = new Point(450, 137);
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
			statusLabel.Location = new Point(470, 299);
			statusLabel.Name = "statusLabel";
			statusLabel.Size = new Size(0, 42);
			statusLabel.TabIndex = 3;
			statusLabel.TextAlign = ContentAlignment.MiddleCenter;
			statusLabel.Visible = false;
			// 
			// progressBar1
			// 
			progressBar1.Location = new Point(450, 403);
			progressBar1.Name = "progressBar1";
			progressBar1.Size = new Size(328, 59);
			progressBar1.TabIndex = 4;
			progressBar1.Visible = false;
			// 
			// tabControl1
			// 
			tabControl1.Controls.Add(tabPage1);
			tabControl1.Controls.Add(tabPage2);
			tabControl1.Dock = DockStyle.Fill;
			tabControl1.Location = new Point(0, 0);
			tabControl1.Name = "tabControl1";
			tabControl1.SelectedIndex = 0;
			tabControl1.Size = new Size(1279, 550);
			tabControl1.TabIndex = 5;
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
			tabPage1.Size = new Size(1271, 522);
			tabPage1.TabIndex = 0;
			tabPage1.Text = "GERAR PDF";
			// 
			// panel2
			// 
			panel2.BorderStyle = BorderStyle.FixedSingle;
			panel2.Controls.Add(resetData);
			panel2.Controls.Add(PdfDirButton);
			panel2.Location = new Point(6, 89);
			panel2.Name = "panel2";
			panel2.Size = new Size(201, 425);
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
			PdfDirButton.Location = new Point(24, 245);
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
			panel1.Size = new Size(1265, 80);
			panel1.TabIndex = 6;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.BackColor = Color.FromArgb(7, 43, 101);
			label1.Font = new Font("Segoe UI", 24.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
			label1.ForeColor = Color.Transparent;
			label1.Location = new Point(432, 13);
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
			pictureBox1.Size = new Size(1265, 80);
			pictureBox1.TabIndex = 0;
			pictureBox1.TabStop = false;
			// 
			// tabPage2
			// 
			tabPage2.BackColor = Color.Gainsboro;
			tabPage2.BackgroundImageLayout = ImageLayout.None;
			tabPage2.Controls.Add(listBoxStores);
			tabPage2.Controls.Add(dataGridView1);
			tabPage2.Controls.Add(buttonFilter);
			tabPage2.Controls.Add(panel3);
			tabPage2.Controls.Add(panel4);
			tabPage2.Controls.Add(button3);
			tabPage2.Font = new Font("Segoe UI", 25F);
			tabPage2.Location = new Point(4, 24);
			tabPage2.Name = "tabPage2";
			tabPage2.Padding = new Padding(3);
			tabPage2.Size = new Size(1271, 522);
			tabPage2.TabIndex = 1;
			tabPage2.Text = "Ler Ecxel";
			// 
			// listBoxStores
			// 
			listBoxStores.CheckOnClick = true;
			listBoxStores.FormattingEnabled = true;
			listBoxStores.Location = new Point(224, 134);
			listBoxStores.Name = "listBoxStores";
			listBoxStores.Size = new Size(351, 98);
			listBoxStores.TabIndex = 11;
			// 
			// dataGridView1
			// 
			dataGridView1.AllowUserToAddRows = false;
			dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView1.Location = new Point(224, 237);
			dataGridView1.Name = "dataGridView1";
			dataGridView1.Size = new Size(1014, 259);
			dataGridView1.TabIndex = 10;
			// 
			// buttonFilter
			// 
			buttonFilter.Location = new Point(581, 134);
			buttonFilter.Name = "buttonFilter";
			buttonFilter.Size = new Size(140, 51);
			buttonFilter.TabIndex = 9;
			buttonFilter.Text = "Filtrar";
			buttonFilter.UseVisualStyleBackColor = true;
			buttonFilter.Click += buttonFilter_Click;
			// 
			// panel3
			// 
			panel3.BorderStyle = BorderStyle.FixedSingle;
			panel3.Controls.Add(button1);
			panel3.Controls.Add(button2);
			panel3.Location = new Point(6, 89);
			panel3.Name = "panel3";
			panel3.Size = new Size(201, 425);
			panel3.TabIndex = 7;
			// 
			// button1
			// 
			button1.BackColor = Color.LightGray;
			button1.FlatAppearance.BorderSize = 0;
			button1.FlatAppearance.MouseOverBackColor = Color.Black;
			button1.FlatStyle = FlatStyle.Flat;
			button1.Font = new Font("Segoe UI", 13F);
			button1.Location = new Point(24, 18);
			button1.Name = "button1";
			button1.Size = new Size(150, 150);
			button1.TabIndex = 6;
			button1.Text = "Atualizar Dados da API";
			button1.UseVisualStyleBackColor = false;
			// 
			// button2
			// 
			button2.BackColor = Color.LightGray;
			button2.FlatAppearance.BorderSize = 0;
			button2.FlatAppearance.MouseOverBackColor = Color.Black;
			button2.FlatStyle = FlatStyle.Flat;
			button2.Font = new Font("Segoe UI", 13F);
			button2.Location = new Point(24, 245);
			button2.Name = "button2";
			button2.Size = new Size(150, 150);
			button2.TabIndex = 5;
			button2.Text = "Abrir Pasta Dos PDF´S";
			button2.UseVisualStyleBackColor = false;
			// 
			// panel4
			// 
			panel4.BackColor = SystemColors.HotTrack;
			panel4.Controls.Add(label2);
			panel4.Controls.Add(pictureBox2);
			panel4.Dock = DockStyle.Top;
			panel4.Location = new Point(3, 3);
			panel4.Name = "panel4";
			panel4.Size = new Size(1265, 80);
			panel4.TabIndex = 6;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.BackColor = Color.FromArgb(7, 43, 101);
			label2.Font = new Font("Segoe UI", 24.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
			label2.ForeColor = Color.Transparent;
			label2.Location = new Point(432, 13);
			label2.Name = "label2";
			label2.Size = new Size(477, 45);
			label2.TabIndex = 7;
			label2.Text = "ExpressGlass - Mapas de KLM";
			// 
			// pictureBox2
			// 
			pictureBox2.BackColor = Color.FromArgb(7, 43, 101);
			pictureBox2.Dock = DockStyle.Fill;
			pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
			pictureBox2.Location = new Point(0, 0);
			pictureBox2.Name = "pictureBox2";
			pictureBox2.Size = new Size(1265, 80);
			pictureBox2.TabIndex = 0;
			pictureBox2.TabStop = false;
			// 
			// button3
			// 
			button3.BackColor = Color.Transparent;
			button3.Font = new Font("Segoe UI", 15F);
			button3.Location = new Point(866, 108);
			button3.Name = "button3";
			button3.Size = new Size(328, 66);
			button3.TabIndex = 0;
			button3.Text = "Importar Excel";
			button3.UseVisualStyleBackColor = false;
			button3.Click += button3_Click;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1279, 550);
			Controls.Add(tabControl1);
			Icon = (Icon)resources.GetObject("$this.Icon");
			Name = "Form1";
			Text = "PDF Generator";
			tabControl1.ResumeLayout(false);
			tabPage1.ResumeLayout(false);
			tabPage1.PerformLayout();
			panel2.ResumeLayout(false);
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			tabPage2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
			panel3.ResumeLayout(false);
			panel4.ResumeLayout(false);
			panel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
			((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private Button GerarPdf;
		private Label statusLabel;
		private ProgressBar progressBar1;
		private TabControl tabControl1;
		private TabPage tabPage1;
		private Button PdfDirButton;
		private Panel panel1;
		private Label label1;
		private PictureBox pictureBox1;
		private BindingSource bindingSource1;
		private Panel panel2;
		private Button resetData;
		private TabPage tabPage2;
		private Panel panel3;
		private Button button1;
		private Button button2;
		private Panel panel4;
		private Label label2;
		private PictureBox pictureBox2;
		private Button button3;
		private Button buttonFilter;
		private CheckedListBox listBoxStores;
		private DataGridView dataGridView1;
	}
}
