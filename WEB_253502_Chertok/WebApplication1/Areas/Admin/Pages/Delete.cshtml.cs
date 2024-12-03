using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253502_Chertok.Domain.Entities;
using WEB_253502_Chertok.Services.CategoryService;
using WEB_253502_Chertok.Services.ProductService;

namespace WEB_253502_Chertok.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
		private readonly IProductService _productService;
		private readonly ICategoryService _categoryService;
		public DeleteModel(ICategoryService categoryService, IProductService productService)
		{
			_categoryService = categoryService;
			_productService = productService;
		}
		[BindProperty]
		public Product Product { get; set; } = default!;

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

			Product = response.Data!;

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			try
			{
				await _productService.DeleteProductAsync(id.Value);
			}
			catch (Exception e)
			{
				return NotFound(e.Message);
			}

			return RedirectToPage("./Index");
		}
	}
}
