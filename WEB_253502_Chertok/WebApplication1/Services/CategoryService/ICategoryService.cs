using WEB_253502_Chertok.Domain.Entities;
using WEB_253502_Chertok.Domain.Models;

namespace WEB_253502_Chertok.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
