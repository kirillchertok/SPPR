using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253502_Chertok.Domain.Entities;
using WEB_253502_Chertok.Services.CategoryService;
using WEB_253502_Chertok.Services.ProductService;

namespace WEB_253502_Chertok.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public DetailsModel(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _productService.GetProductByIdAsync(id.Value);

            if (!response.Successfull)
            {
                return NotFound();
            }

            var categoryResponse = await _categoryService.GetCategoryListAsync();
            if (!categoryResponse.Successfull)
            {
                return NotFound();
            }

            Product = response.Data!;
            CategoryName = categoryResponse.Data.FirstOrDefault(c => c.Id == Product.CategoryId).Name;

            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default;

        [BindProperty]
        public string CategoryName { get; set; } = string.Empty;
    }
}
