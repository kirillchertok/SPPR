using WEB_253502_Chertok.Domain.Entities;
using WEB_253502_Chertok.Domain.Models;

namespace WEB_253502_Chertok.Services.CategoryService
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            List<Category> productCategories = new List<Category>
        {
            new Category { Name = "Phone", NormalizedName = "phone" },
            new Category { Name = "Headphones", NormalizedName = "headphones" },
            new Category { Name = "Laptop", NormalizedName = "laptop" },
            new Category { Name = "TV", NormalizedName = "tv" },
        };

            var result = ResponseData<List<Category>>.Success(productCategories);

            return Task.FromResult(result);
        }
    }
}
