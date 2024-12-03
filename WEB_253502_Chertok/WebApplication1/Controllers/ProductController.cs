using Microsoft.AspNetCore.Mvc;
using WEB_253502_Chertok.Extensions;
using WEB_253502_Chertok.Services.CategoryService;
using WEB_253502_Chertok.Services.ProductService;

namespace WEB_253502_Chertok.Controllers
{
	[Route("Catalog")]
	public class ProductController : Controller
    {
		private readonly ICategoryService _categoryService;
		private readonly IProductService _productService;
		public ProductController(IProductService productService, ICategoryService categoryService)
		{
			_categoryService = categoryService;
			_productService = productService;
		}

		[HttpGet("{category?}")]
		public async Task<IActionResult> Index(string? category, int pageNo = 1)
		{
			var productResponse =
				await _productService.GetProductListAsync(category, pageNo);
			if (!productResponse.Successfull)
			{
				return NotFound(productResponse.ErrorMessage);
			}

			var allCategories =
				await _categoryService.GetCategoryListAsync();
			if (!allCategories.Successfull)
			{
				return NotFound(allCategories.ErrorMessage);
			}

			var currentCategory = category == null ? "Все" : allCategories.Data.FirstOrDefault(c => c.NormalizedName.Equals(category)).Name;

			ViewData["Categories"] = allCategories.Data;
			ViewData["CurrentCategory"] = currentCategory;

			if (Request.IsAjaxRequest())
			{
				return PartialView("_PagerAndCardsPartial", new
				{
					CurrentCategory = category,
					Categories = allCategories.Data,
					Products = productResponse.Data!.Items,
					ReturnUrl = Request.Path + Request.QueryString.ToUriComponent(),
					CurrentPage = productResponse.Data.CurrentPage,
					TotalPages = productResponse.Data.TotalPages,
					Admin = false
				});
			}

			return View(productResponse.Data);
		}
	}
}
