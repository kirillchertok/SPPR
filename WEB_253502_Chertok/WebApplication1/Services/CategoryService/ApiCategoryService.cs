using System.Text.Json;
using System.Text;
using WEB_253502_Chertok.Domain.Entities;
using WEB_253502_Chertok.Domain.Models;

namespace WEB_253502_Chertok.Services.CategoryService
{
	public class ApiCategoryService : ICategoryService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly JsonSerializerOptions _serializerOptions;
		private readonly ILogger<ApiCategoryService> _logger;
		public ApiCategoryService(IHttpClientFactory httpClientFactory,
									ILogger<ApiCategoryService> logger)
		{
			_httpClientFactory = httpClientFactory;
			_serializerOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};
			_logger = logger;
		}
		public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
		{
			var client = _httpClientFactory.CreateClient("api");

			var uriString = new StringBuilder($"{client.BaseAddress.AbsoluteUri}Categories/");

			var response = await client.GetAsync(new Uri(uriString.ToString()));
			if (!response.IsSuccessStatusCode)
			{
				return ResponseData<List<Category>>.Error($"Error in fetching data: {response.StatusCode.ToString()}");
			}

			try
			{
				return await response
								.Content
								.ReadFromJsonAsync<ResponseData<List<Category>>>
															(_serializerOptions);
			}
			catch (Exception ex)
			{
				_logger.LogError($"-----> Error: {ex.Message}");
				return ResponseData<List<Category>>.Error($"Error: {ex.Message}");
			}
		}
	}
}
