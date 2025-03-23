using System;
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
			//AllocConsole();
			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			ApplicationConfiguration.Initialize();
			var config = NotionConfigHelper.GetConfig();

			if (config == null)
			{
				MessageBox.Show("Erro: As API Keys não foram definidas.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			Application.Run(new Form1(config));
		}
	}
}