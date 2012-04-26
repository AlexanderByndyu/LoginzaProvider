namespace LoginzaProvider.Core
{
	internal interface ILoginzaProvider
	{
		AuthenticationData GetAuthenticationData(string token);
	}
}