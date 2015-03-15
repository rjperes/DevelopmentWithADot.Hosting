using System;
using System.Net;
using System.Web.Http;

namespace DevelopmentWithADot.WebApi
{
	public class DummyController : ApiController
	{
		[HttpGet]
		public IHttpActionResult Action()
		{
			return this.Content(HttpStatusCode.OK, "Hello, World!");
		}
	}
}
