using System;
using System.IO;
using System.Net;
using Microsoft.Owin.Hosting;
using Owin;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<System.String, System.Object>, System.Threading.Tasks.Task>;

namespace DevelopmentWithADot.Owin
{
	class Program
	{
		public static void Configuration(IAppBuilder app)
		{
			app.Use(new Func<AppFunc, AppFunc>(next => (async ctx =>
			{
				using (var writer = new StreamWriter(ctx["owin.ResponseBody"] as Stream))
				{
					await writer.WriteAsync("Hello World!");
				}
			})));
		}

		static void Main(String[] args)
		{
			var url = "http://*:2000";

			using (WebApp.Start<Program>(url))
			{
				var client = new WebClient();
				var response = client.DownloadString("http://localhost:2000");

				Console.ReadLine();
			}
		}
	}
}
