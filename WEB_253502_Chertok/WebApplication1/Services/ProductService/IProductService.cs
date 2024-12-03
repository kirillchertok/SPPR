using WEB_253502_Chertok.Domain.Entities;
using WEB_253502_Chertok.Domain.Models;

namespace WEB_253502_Chertok.Services.ProductService
{
    public interface IProductService
    {
        public Task<ResponseData<ProductListModel<Product>>> GetProductListAsync(string?
        categoryNormalizedName, int pageNo = 1);
        public Task<ResponseData<Product>> GetProductByIdAsync(int id);
        public Task UpdateProductAsync(int id, Product constructor, IFormFile? formFile);
        public Task DeleteProductAsync(int id);
        public Task<ResponseData<Product>> CreateProductAsync(Product constructor, IFormFile? formFile);
    }
}
