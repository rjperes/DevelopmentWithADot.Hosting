using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DevelopmentWithADot.HttpListener
{
	class Program
	{
		static void Main(String[] args)
		{
			using (var listener = new System.Net.HttpListener())
			using (var @event = new ManualResetEvent(false))
			{
				var url = "http://*:2000/";

				Task.Factory.StartNew(() =>
				{
					@event.WaitOne();

					var client = new WebClient();
					var response = client.DownloadString("http://localhost:2000/");
					response.ToString();
				});

				listener.Prefixes.Add(url);
				listener.Start();

				@event.Set();

				var ctx = listener.GetContext();

				var message = "Hello, World!";

				ctx.Response.StatusCode = (Int32) HttpStatusCode.OK;
				ctx.Response.ContentType = "text/plain";
				ctx.Response.ContentLength64 = message.Length;

				using (var writer = new StreamWriter(ctx.Response.OutputStream))
				{
					writer.Write(message);
				}

				Console.ReadLine();
			}
		}
	}
}
