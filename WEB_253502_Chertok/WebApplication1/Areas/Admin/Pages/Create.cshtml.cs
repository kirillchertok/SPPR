using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253502_Chertok.Domain.Entities;
using WEB_253502_Chertok.Services.CategoryService;
using WEB_253502_Chertok.Services.ProductService;

namespace WEB_253502_Chertok.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
		private readonly IProductService _productService;
		private readonly ICategoryService _categoryService;

		public CreateModel(IProductService constructorService, ICategoryService categoryService)
		{
			_productService = constructorService;
			_categoryService = categoryService;

			Categories = new SelectList(_categoryService.GetCategoryListAsync().Result.Data, "Id", "Name");
		}
		public async Task<IActionResult> OnGetAsync()
		{
			return Page();
		}

		[BindProperty]
		public IFormFile? Image { get; set; }

		[BindProperty]
		public Product Product { get; set; } = default!;

		public SelectList Categories { get; set; }
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var response = await _productService.CreateProductAsync(Product, Image);

			if (!response.Successfull)
			{
				return Page();
			}

			return RedirectToPage("./Index");
		}
	}
}
