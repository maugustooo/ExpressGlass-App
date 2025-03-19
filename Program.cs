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
		static async Task Main()
        {
			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}