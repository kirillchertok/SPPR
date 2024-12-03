using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253502_Chertok.Domain.Entities;
using WEB_253502_Chertok.Services.CategoryService;
using WEB_253502_Chertok.Services.ProductService;

namespace WEB_253502_Chertok.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
		private readonly IProductService _productService;
		private readonly ICategoryService _categoryService;
		public EditModel(IProductService productService, ICategoryService categoryService)
		{
			_productService = productService;
			_categoryService = categoryService;

			Categories = new SelectList(_categoryService.GetCategoryListAsync().Result.Data, "Id", "Name");
		}
		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				Console.WriteLine("Slam iz nulevogo prikola");
				return NotFound();
			}

			var response = await _productService.GetProductByIdAsync(id.Value);

			if (!response.Successfull)
			{
				Console.WriteLine("Slam iz pervogo prikola");
				return NotFound();
			}

			var categoryResponse = await _categoryService.GetCategoryListAsync();

			if (!categoryResponse.Successfull)
			{
				Console.WriteLine("Slam iz vtorogo prikola");
				return NotFound();
			}

			Product = response.Data!;

			CurrentImage = Product.Image;

			return Page();
		}

		[BindProperty]
		public IFormFile? Image { get; set; }

		[BindProperty]
		public string? CurrentImage { get; set; }

		[BindProperty]
		public Product Product { get; set; } = default!;
		public SelectList Categories { get; set; }

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			try
			{
				await _productService.UpdateProductAsync(Product.Id, Product, Image);
			}
			catch (Exception)
			{
				if (!await ConstructorsExists(Product.Id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return RedirectToPage("./Index");
		}

		private async Task<bool> ConstructorsExists(int id)
		{
			var response = await _productService.GetProductByIdAsync(id);
			return response.Successfull;
		}
	}
}
