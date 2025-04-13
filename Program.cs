using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;

namespace Gerador_ecxel
{
    internal static class Program
    {
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		//[DllImport("kernel32.dll", SetLastError = true)]
		//[return: MarshalAs(UnmanagedType.Bool)]
		//static extern bool AllocConsole();
		static void Main()
		{
			string logPath = "log.txt";
			FileStream fs = new FileStream(logPath, FileMode.Append, FileAccess.Write);
			StreamWriter sw = new StreamWriter(fs);
			sw.AutoFlush = true;
			Console.SetOut(sw);
			Console.SetError(sw);

			try
			{
				Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
				ApplicationConfiguration.Initialize();
				var config = NotionConfigHelper.GetConfig();

				if (config == null)
				{
					MessageBox.Show("Erro: As API Keys n�o foram definidas.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				Application.Run(new Form1(config));
			}
			finally
			{
				sw.Close();
				fs.Close();

				try
				{
					if (File.Exists(logPath))
						File.Delete(logPath);
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Erro ao deletar o log: " + ex.Message);
				}
			}
		}
	}
}