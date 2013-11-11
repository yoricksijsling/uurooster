using System;
using System.Collections.Specialized;
using System.IO;
using System.Text.RegularExpressions;

namespace OsirisClient
{
	public class OsirisSession
	{
		private readonly OsirisWebClient client;
		private string requestToken;

		public OsirisSession(OsirisWebClient client)
		{
			this.client = client;
		}

		public void Authenticate(string username, string password)
		{
			DownloadRequestToken();

			// Authenticate
			var formData = new NameValueCollection() {
				{ "startUrl", "Personalia.do" },
				{ "requestToken", requestToken },
				{ "gebruikersNaam", username },
				{ "wachtWoord", password },
				{ "event", "login" }
			};
//			client.UploadValues("AuthenticateUser.do", formData);
			var authResp = new StreamReader(new MemoryStream(client.UploadValues("AuthenticateUser.do", formData))).ReadToEnd();
			if (authResp.Contains("F00001")) {
				throw new NotAuthenticatedException();
			}
		}

		private void DownloadRequestToken()
		{
			var requestTokenResp = client.DownloadString("AuthenticateUser.do");
			this.requestToken = Regex.Match(requestTokenResp, "<input id=\"requestToken\"[^/]*value=\"([0-9a-f]*)\"").Groups[1].Value;
		}

		public Schedule DownloadSchedule()
		{
			// Get uixState for rooster form
			var uixStateResp = client.DownloadString("KiesRooster.do");
			var uixState = Regex.Match(uixStateResp, "<input id=\"_uixState\"[^/]*value=\"([0-9A-F]*)\"").Groups[1].Value;
			// uixState seems to be static, so we can skip the request
			//			var uixState = "789C73720E0EB1B23508CACF2F2E492D0A4FCD4ECDF30432124BF28B6C755D740D80400700E5000BCD";

			// Get weekrooster
			var formData = new NameValueCollection() {
				{ "startUrl", "KiesRooster.do" },
				{ "requestToken", requestToken },
				{ "WEB_INF_page_KiesRoosterUIModelState__", uixState },
				{ "event", "toonTotaalrooster" }
			};
			var resp = client.UploadValues("KiesRooster.do", formData);
			//var roosterResp = new StreamReader(new MemoryStream(client.UploadValues("KiesRooster.do", roosterValues))).ReadToEnd();

			return Schedule.Parse(new MemoryStream(resp));
		}


	}
}

