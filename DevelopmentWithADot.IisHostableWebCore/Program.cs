using System;
using System.Net;

namespace DevelopmentWithADot.IisHostableWebCore
{
	class Program
	{
		static void Main(String[] args)
		{
			var port = 2000;
			var file = "Default.aspx";
			var path = Environment.CurrentDirectory.Replace(@"\bin\Debug", String.Empty);

			using (var host = new Host(path, port))
			{
				host.Start();

				var webClient = new WebClient();
				var html = webClient.DownloadString(String.Concat("http://localhost:", port, "/", file));

				Console.ReadLine();
			}
		}
	}
}
