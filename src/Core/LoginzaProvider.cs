namespace LoginzaProvider.Core
{
	using System;
	using System.IO;
	using System.Net;
	using System.Security.Cryptography;
	using System.Text;
	using Newtonsoft.Json;

	public class LoginzaProvider : ILoginzaProvider
	{
		private readonly Uri serviceUri = new Uri("http://loginza.ru/api/authinfo");
		private string _token;
		private readonly int widgetId;
		private readonly string secureKey;
		private readonly bool isSecureCheck;

		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="widgetId">Your Loginza Winget ID</param>
		/// <param name="secureKey">Get security key for your widget on Loginza settings page</param>
		public LoginzaProvider(int widgetId, string secureKey)
		{
			this.widgetId = widgetId;
			this.secureKey = secureKey;
		}

		public LoginzaProvider(int widgetId, string secureKey, bool isSecureCheck) : this(widgetId, secureKey)
		{
			this.isSecureCheck = isSecureCheck;
		}

		public AuthenticationData GetAuthenticationData(string token)
		{
			_token = token;

			string data = GetResponseFromLoginzaService();

			return JsonConvert.DeserializeObject<AuthenticationData>(data);
		}

		private string CreateRequestUrl()
		{
			string requestParams = string.Format("?token={0}", _token);

			if (isSecureCheck)
			{
				string sign = ComputeMd5Hash();
				requestParams += string.Format("&id={0}&sig={1}", widgetId, sign);
			}

			return requestParams;
		}

		private string GetResponseFromLoginzaService()
		{
			Uri requestUrl;

			if (Uri.TryCreate(serviceUri + CreateRequestUrl(), UriKind.Absolute, out requestUrl) == false)
				throw new UriFormatException();

			var request = (HttpWebRequest) WebRequest.Create(requestUrl);
			request.Method = "GET";

			using (WebResponse response = request.GetResponse())
			using (Stream responseStream = response.GetResponseStream())
			{
				if (responseStream == null)
					throw new WebException("Response is empty");

				using (var dataStream = new StreamReader(responseStream))
				{
					return dataStream.ReadToEnd();
				}
			}
		}

		private string ComputeMd5Hash()
		{
			using (MD5 md5 = MD5.Create())
			{
				byte[] buf = Encoding.ASCII.GetBytes(_token + secureKey);
				byte[] hash = md5.ComputeHash(buf);

				var sb = new StringBuilder();

				for (int i = 0; i < hash.Length; i++)
				{
					sb.Append(hash[i].ToString("x2"));
				}

				return sb.ToString();
			}
		}
	}
}