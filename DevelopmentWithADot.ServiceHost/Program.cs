using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Threading;
using System.Threading.Tasks;

namespace DevelopmentWithADot.ServiceHost
{
	class Program
	{
		static void Main(String[] args)
		{
			using (var @event = new ManualResetEvent(false))
			using (var host = new WebServiceHost(typeof (Rest)))
			{
				var url = new Uri(@"http://localhost:2000");
				var binding = new WebHttpBinding();

				/*Task.Factory.StartNew(() =>
				{
					@event.WaitOne();

					var factory = new ChannelFactory<IRest>(binding, new EndpointAddress(url));
					factory.Endpoint.Behaviors.Add(new WebHttpBehavior());

					var proxy = factory.CreateChannel();
					var response = proxy.ProcessRequest("Bla, bla");
				});*/

				host.AddServiceEndpoint(typeof(IRest), binding, url);
				host.Open();

				@event.Set();

				Console.ReadLine();
			}
		}
	}
}
