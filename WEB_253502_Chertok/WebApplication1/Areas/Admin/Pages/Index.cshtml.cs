using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253502_Chertok.Domain.Entities;
using WEB_253502_Chertok.Extensions;
using WEB_253502_Chertok.Services.ProductService;

namespace WEB_253502_Chertok.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
		private readonly IProductService _productService;
		public IndexModel(IProductService productService)
		{
			_productService = productService;
		}
		public async Task<IActionResult> OnGetAsync(int pageNo = 1)
		{
			var response = await _productService.GetProductListAsync(null, pageNo);
			if (response.Successfull)
			{
				Products = response.Data.Items;
				TotalPages = response.Data.TotalPages;
				CurrentPage = pageNo;

				if (Request.IsAjaxRequest())
				{
					return Partial("_PagerAndCardsPartial", new
					{
						Admin = true,
						CurrentPage = CurrentPage,
						TotalPages = TotalPages,
						Products = Products
					});
				}

				return Page();
			}

			return RedirectToPage("/Error");
		}

		[BindProperty]
		public List<Product> Products { get; set; } = new();

		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
	}
}
