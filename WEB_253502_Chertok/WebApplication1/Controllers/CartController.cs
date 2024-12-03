using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_253502_Chertok.Domain.Models;
using WEB_253502_Chertok.Services.ProductService;

namespace WEB_253502_Chertok.Controllers
{
	[Authorize]
	public class CartController : Controller
	{
		private readonly IProductService _productService;
		private readonly Cart _cart;
		public CartController(IProductService productService, Cart cart)
		{
			_productService = productService;
			_cart = cart;
		}

		public IActionResult Index()
		{
			return View(_cart);
		}

		[Route("[controller]/add/{id:int}")]
		public async Task<IActionResult> AddItem(int id, string returnUrl)
		{
			var data = await _productService.GetProductByIdAsync(id);
			if (data.Successfull)
			{
				_cart.AddToCart(data.Data);
			}

			return Redirect(returnUrl);
		}

		[Route("[controller]/remove/{id:int}")]
		public async Task<IActionResult> RemoveItem(int id, string returnUrl)
		{
			var data = await _productService.GetProductByIdAsync(id);
			if (data.Successfull)
			{
				_cart.RemoveItems(id);
			}

			return Redirect(returnUrl);
		}
	}
}
