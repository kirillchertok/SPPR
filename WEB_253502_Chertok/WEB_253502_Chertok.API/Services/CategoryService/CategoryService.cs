using Microsoft.EntityFrameworkCore;
using WEB_253502_Chertok.API.Data;
using WEB_253502_Chertok.Domain.Entities;
using WEB_253502_Chertok.Domain.Models;

namespace WEB_253502_Chertok.API.Services.CategoryService
{
	public class CategoryService : ICategoryService
	{
		private readonly AppDbContext _context;
		public CategoryService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
		{
			var categories = await _context.Categories.ToListAsync();
			if (!categories.Any() || categories is null)
			{
				return ResponseData<List<Category>>.Error("No categories in db");
			}

			return ResponseData<List<Category>>.Success(categories);
		}
	}
}
