using System;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace DevelopmentWithADot.ApplicationHost
{
	public class Host : MarshalByRefObject
	{
		public void ProcessPage(String page, String query, TextWriter writer)
		{
			var worker = new SimpleWorkerRequest(page, query, writer);
			var context = new HttpContext(worker);
			HttpRuntime.ProcessRequest(worker);
		}

		public void ProcessPage(String page, String query)
		{
			this.ProcessPage(page, query, Console.Out);
		}
	}
}
