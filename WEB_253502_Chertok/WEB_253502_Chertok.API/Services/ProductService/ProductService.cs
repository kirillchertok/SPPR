using Microsoft.EntityFrameworkCore;
using WEB_253502_Chertok.Domain.Entities;
using WEB_253502_Chertok.API.Data;
using WEB_253502_Chertok.Domain.Models;
using Newtonsoft.Json.Linq;

namespace WEB_253502_Chertok.API.Services.ProductService
{
	public class ProductService : IProductService
	{
		private readonly int _maxPageSize = 20;
		private readonly AppDbContext _context;
		public ProductService(AppDbContext context)
		{
			_context = context;
		}
		public async Task<ResponseData<Product>> CreateProductAsync(Product product)
		{
			var newProduct = await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();
			return ResponseData<Product>.Success(newProduct.Entity);
		}

		public async Task DeleteProductAsync(int id)
		{
			var product = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
			if (product is null)
			{
				return;
			}

			_context.Entry(product).State = EntityState.Deleted;
		}

		public async Task<ResponseData<Product>> GetProductByIdAsync(int id)
		{
			var product = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
			if (product is null)
			{
				return ResponseData<Product>.Error($"No such object with id : {id}");
			}
			return ResponseData<Product>.Success(product);
		}

		public async Task<ResponseData<ProductListModel<Product>>> GetProductListAsync(
		string? categoryNormalizedName,
									int pageNo = 1,
									int pageSize = 3)
		{
			var query = _context.Products.AsQueryable();
			var dataList = new ProductListModel<Product>();

			if (pageSize > _maxPageSize)
				pageSize = _maxPageSize;

			query = query
				.Where(d => categoryNormalizedName == null || d.Category.NormalizedName.Equals(categoryNormalizedName));

			var count = await query.CountAsync();
			if (count == 0)
			{
				return ResponseData<ProductListModel<Product>>.Success(dataList);
			}

			int totalPages = (int)Math.Ceiling(count / (double)pageSize);

			if (pageNo > totalPages)
			{
				return ResponseData<ProductListModel<Product>>.Error("No such page");
			}

			dataList.Items = await query
									.OrderBy(c => c.Id)
									.Skip((pageNo - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync();

			dataList.CurrentPage = pageNo;
			dataList.TotalPages = totalPages;

			return ResponseData<ProductListModel<Product>>.Success(dataList);
		}

		public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
		{
			throw new NotImplementedException();
		}

		public async Task UpdateProductAsync(int id, Product constructor, IFormFile? formFile)
		{
			var product = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
			if (product is null)
				return;

			product.Price = constructor.Price;
			product.Description = constructor.Description;
			product.Category = constructor.Category;
			product.CategoryId = constructor.CategoryId;
			product.Image = constructor.Image;

			_context.Entry(product).State = EntityState.Modified;
		}
	}
}
