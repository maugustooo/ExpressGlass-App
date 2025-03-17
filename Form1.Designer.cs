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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			GerarPdf = new Button();
			panelPDF = new Panel();
			button1 = new Button();
			SuspendLayout();
			// 
			// GerarPdf
			// 
			GerarPdf.Location = new Point(12, 12);
			GerarPdf.Name = "GerarPdf";
			GerarPdf.Size = new Size(304, 23);
			GerarPdf.TabIndex = 0;
			GerarPdf.Text = "Gerar pdf";
			GerarPdf.UseVisualStyleBackColor = true;
			GerarPdf.Click += GerarPdf_Click;
			// 
			// panelPDF
			// 
			panelPDF.Location = new Point(322, 41);
			panelPDF.Name = "panelPDF";
			panelPDF.Size = new Size(630, 891);
			panelPDF.TabIndex = 1;
			// 
			// button1
			// 
			button1.Location = new Point(322, 12);
			button1.Name = "button1";
			button1.Size = new Size(630, 23);
			button1.TabIndex = 2;
			button1.Text = "Preview";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1151, 706);
			Controls.Add(button1);
			Controls.Add(panelPDF);
			Controls.Add(GerarPdf);
			Icon = (Icon)resources.GetObject("$this.Icon");
			Name = "Form1";
			Text = "PDF Generator";
			ResumeLayout(false);
		}

		#endregion

		private Button GerarPdf;
		private Panel panelPDF;
		private Button button1;
	}
}
