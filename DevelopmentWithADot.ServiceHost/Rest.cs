using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace DevelopmentWithADot.ServiceHost
{
	[ServiceContract]
	public interface IRest
	{
		[WebGet(ResponseFormat = WebMessageFormat.Json)]
		[OperationContract]
		String ProcessRequest(String url);
	}

	public class Rest : IRest
	{
		public String ProcessRequest(String url)
		{
			//WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
			return String.Concat("Hello: ", url);
		}
	}
}
