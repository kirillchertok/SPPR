using System.Text.Json;
using System.Text;
using WEB_253502_Chertok.Domain.Models;
using WEB_253502_Chertok.Domain.Entities;
using WEB_253502_Chertok.Services.FileService;
using WEB_253502_Chertok.Services.Authentication;

namespace WEB_253502_Chertok.Services.ProductService
{
	public class ApiProductService : IProductService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IFileService _fileService;
		private readonly ITokenAccessor _tokenAccessor;
		private readonly JsonSerializerOptions _serializerOptions;
		private readonly ILogger<ApiProductService> _logger;
		private readonly string _pageSize;
		public ApiProductService(IConfiguration configuration,
									IHttpClientFactory httpClientFactory,
									ILogger<ApiProductService> logger,
									IFileService fileService,
									ITokenAccessor tokenAccessor)
		{
			_httpClientFactory = httpClientFactory;
			_serializerOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};
			_logger = logger;
			_pageSize = configuration.GetSection("ItemsPerPage").Value;
			_fileService = fileService;
			_tokenAccessor = tokenAccessor;
		}

		public async Task<ResponseData<Product>> CreateProductAsync(Product product, IFormFile? formFile)
		{
			product.Image = "images/noimage.jpg";

			if (formFile != null)
			{
				var imageUrl = await _fileService.SaveFileAsync(formFile);

				if (!string.IsNullOrEmpty(imageUrl))
					product.Image = imageUrl;
			}

			var client = _httpClientFactory.CreateClient("api");

			await _tokenAccessor.SetAuthorizationHeaderAsync(client);

			var uri = new Uri(client.BaseAddress.AbsoluteUri + "Constructors");

			var response = await client.PostAsJsonAsync(uri, product, _serializerOptions);
			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError($"-----> object not created. Error:{response.StatusCode.ToString()}");
				return ResponseData<Product>.Error($"Object not added. Error:{response.StatusCode.ToString()}");
			}

			return await response.Content.ReadFromJsonAsync<ResponseData<Product>>(_serializerOptions);
		}

		public async Task DeleteProductAsync(int id)
		{
			var client = _httpClientFactory.CreateClient("api");

			await _tokenAccessor.SetAuthorizationHeaderAsync(client);

			var response = await client.DeleteAsync($"{client.BaseAddress}Products/{id}");
			if (!response.IsSuccessStatusCode)
			{
				throw new Exception("Delete operation failed");
			}
			return;
		}

		public async Task<ResponseData<Product>> GetProductByIdAsync(int id)
		{
			var client = _httpClientFactory.CreateClient("api");

			var response = await client.GetAsync($"{client.BaseAddress}Products/{id}");
			if (!response.IsSuccessStatusCode)
			{
				return ResponseData<Product>.Error($"Error: {response.StatusCode.ToString()}");
			}

			var product = await response.Content.ReadFromJsonAsync<Product>();

			return ResponseData<Product>.Success(product);
		}

		public async Task<ResponseData<ProductListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
		{
			var client = _httpClientFactory.CreateClient("api");

			var urlString = new StringBuilder($"{client.BaseAddress.AbsoluteUri}Products");

			if (categoryNormalizedName != null)
			{
				urlString.Append($"/{categoryNormalizedName}/");
			}
			if (pageNo >= 1)
			{
				urlString.Append($"?pageNo={pageNo}");
			}
			if (!_pageSize.Equals("3"))
			{
				urlString.Append(QueryString.Create("&pageSize", _pageSize));
			}

			var response = await client.GetAsync(new Uri(urlString.ToString()));

			if (response.IsSuccessStatusCode)
			{
				try
				{
					return await response
					.Content
					.ReadFromJsonAsync<ResponseData<ProductListModel<Product>>>
																(_serializerOptions);
				}
				catch (JsonException ex)
				{
					_logger.LogError($"-----> Error: {ex.Message}");
					return ResponseData<ProductListModel<Product>>.Error($"Error: {ex.Message}");
				}
			}

			_logger.LogError($"-----> Error in fetching data from server. Error:{response.StatusCode.ToString()}");

			return ResponseData<ProductListModel<Product>>.Error($"Data not fetched. Error:{response.StatusCode.ToString()}");
		}

		public async Task UpdateProductAsync(int id, Product product, IFormFile? formFile)
		{
			var client = _httpClientFactory.CreateClient("api");

			await _tokenAccessor.SetAuthorizationHeaderAsync(client);

			if (formFile != null)
			{
				try
				{
					await _fileService.DeleteFileAsync(product.Image);
				}
				catch (Exception ex)
				{
					throw;
				}

				var imageUrl = await _fileService.SaveFileAsync(formFile);

				if (!string.IsNullOrEmpty(imageUrl))
					product.Image = imageUrl;
			}

		}
	}
}
