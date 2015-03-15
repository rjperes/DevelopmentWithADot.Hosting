using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DevelopmentWithADot.ApplicationHost
{
	class Program
	{
		static void Main(String[] args)
		{
			//copy $(TargetPath) $(ProjectDir)\Bin\$(TargetFileName)
			var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace(@"\bin\Debug", String.Empty);
			File.Copy(Assembly.GetExecutingAssembly().Location, Path.Combine(path, "bin", Assembly.GetExecutingAssembly().CodeBase.Split('/').Last()), true);

			var host = System.Web.Hosting.ApplicationHost.CreateApplicationHost(typeof(Host), "/", path) as Host;
			host.ProcessPage("Default.aspx", null);
		}
	}
}
