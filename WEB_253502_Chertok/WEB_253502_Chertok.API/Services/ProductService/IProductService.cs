﻿using Newtonsoft.Json.Linq;
using WEB_253502_Chertok.Domain.Entities;
using WEB_253502_Chertok.Domain.Models;

namespace WEB_253502_Chertok.API.Services.ProductService
{
	public interface IProductService
	{
		public Task<ResponseData<ProductListModel<Product>>> GetProductListAsync(string?
		categoryNormalizedName, int pageNo = 1, int pageSize = 3);
		public Task<ResponseData<Product>> GetProductByIdAsync(int id);
		public Task UpdateProductAsync(int id, Product product, IFormFile? formFile);
		public Task DeleteProductAsync(int id);
		public Task<ResponseData<Product>> CreateProductAsync(Product product);
		public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
	}
}
