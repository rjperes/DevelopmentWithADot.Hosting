using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace DevelopmentWithADot.WebApi
{
	class Program
	{
		static void Main(String[] args)
		{
			var url = "http://localhost:2000";
			var config = new HttpSelfHostConfiguration(url);
			config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}");

			using (var server = new HttpSelfHostServer(config))
			{
				server.OpenAsync().Wait();

				var client2 = new WebClient();
				var response2 = client2.DownloadString(String.Concat(url, "/api/Dummy/Action"));

				var client = new HttpClient(server);
				var response = client.GetAsync(String.Concat(url, "/api/Dummy/Action")).Result.Content.ReadAsStringAsync().Result;
			}
		}
	}
}
