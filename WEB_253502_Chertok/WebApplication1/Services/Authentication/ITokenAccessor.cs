namespace WEB_253502_Chertok.Services.Authentication
{
	public interface ITokenAccessor
	{
		Task<string> GetAccessTokenAsync();
		Task SetAuthorizationHeaderAsync(HttpClient httpClient);
	}
}
