namespace LoginzaProvider.Core
{
	public class AuthenticationData
	{
		public string Error_Type { get; set; }
		public string Error_Message { get; set; }
		public string Identity { get; set; }
		public string Provider { get; set; }
		public string Email { get; set; }
		public string Uid { get; set; }
		public LoginzaName Name { get; set; }
		public string Photo { get; set; }

		public class LoginzaName
		{
			public string First_Name { get; set; }
			public string Last_Name { get; set; }
		}

		public bool IsValid()
		{
			return string.IsNullOrEmpty(Error_Type) && string.IsNullOrEmpty(Error_Message);
		}
	}
}