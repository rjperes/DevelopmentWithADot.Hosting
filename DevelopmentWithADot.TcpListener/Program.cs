using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DevelopmentWithADot.TcpListener
{
	class Program
	{
		static void Main(String[] args)
		{
			var listener = System.Net.Sockets.TcpListener.Create(2000);
			listener.Start();

			Task.Factory.StartNew(() =>
			{
				using (var server = new TcpClient("localhost", 2000))
				{
					using (var writer = new StreamWriter(server.GetStream()))
					using (var reader = new StreamReader(server.GetStream()))
					{
						writer.WriteLine("GET / HTTP/1.1");
						writer.WriteLine();
						writer.Flush();

						var response = reader.ReadLine();

						Console.WriteLine("Received from server: " + response);
					}
				}
			});

			using (var client = listener.AcceptTcpClient())
			{
				using (var reader = new StreamReader(client.GetStream()))
				using (var writer = new StreamWriter(client.GetStream()))
				{
					var request = reader.ReadLine();

					writer.WriteLine("HTTP/1.1 200 OK");
					writer.WriteLine("Content-type: text/plain");
					//writer.WriteLine("Status: 200");
					//writer.WriteLine("Version: HTTP/1.1");
					writer.WriteLine();
					writer.WriteLine("Processing " + request);
					Console.WriteLine("Received from client: " + request);
					writer.Flush();
				}
			}

			listener.Stop();
		}
	}
}
