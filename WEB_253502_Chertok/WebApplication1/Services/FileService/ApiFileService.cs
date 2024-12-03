using WEB_253502_Chertok.Services.Authentication;

namespace WEB_253502_Chertok.Services.FileService
{
	public class ApiFileService : IFileService
	{
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenAccessor _tokenAccessor;
        public ApiFileService(IHttpClientFactory httpClientFactory, ITokenAccessor tokenAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _tokenAccessor = tokenAccessor;
        }
        public async Task DeleteFileAsync(string fileName)
        {
            var client = _httpClientFactory.CreateClient("filesapi");

            await _tokenAccessor.SetAuthorizationHeaderAsync(client);

            var response = await client.DeleteAsync($"{client.BaseAddress}/{fileName}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Cannot delete file");
            }
        }

        public async Task<string> SaveFileAsync(IFormFile formFile)
        {
            var client = _httpClientFactory.CreateClient("filesapi");

            await _tokenAccessor.SetAuthorizationHeaderAsync(client);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post
            };

            Console.WriteLine("----------");
            Console.WriteLine(formFile.FileName);
            Console.WriteLine("----------");

            var extension = Path.GetExtension(formFile.FileName);
            var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);

            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(formFile.OpenReadStream());
            content.Add(streamContent, "file", newName);

            request.Content = content;

            Console.WriteLine(content);

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return string.Empty;
        }
    }
}
