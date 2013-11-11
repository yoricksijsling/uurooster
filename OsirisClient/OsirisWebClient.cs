using System;
using System.Net;
using System.Text.RegularExpressions;

namespace OsirisClient
{
	public class OsirisWebClient : WebClient
	{
		public readonly CookieContainer Cookies = new CookieContainer();
		private readonly string userAgent;

		public OsirisWebClient(string baseAddress, string userAgent)
		{
			BaseAddress = baseAddress;
			this.userAgent = userAgent;
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			WebRequest request = base.GetWebRequest(address);

			Headers.Add(HttpRequestHeader.UserAgent, userAgent);

			HttpWebRequest webRequest = request as HttpWebRequest;
			if (webRequest != null)
			{
				webRequest.CookieContainer = Cookies;
			}
			return request;
		}

		protected override WebResponse GetWebResponse(WebRequest request)
		{
			var resp = base.GetWebResponse(request);

			return resp;
		}
	}
}

