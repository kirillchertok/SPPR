using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WEB_253502_Chertok.API.Data;
using WEB_253502_Chertok.API.Services.ProductService;
using WEB_253502_Chertok.Domain.Entities;

namespace WEB_253502_Chertok.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
		private readonly IProductService _productService;

		public ProductsController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet]
		[Route("{category?}")]
		public async Task<IActionResult> GetConstructors(string? category, int pageNo = 1, int pageSize = 3)
		{
			return Ok(await _productService.GetProductListAsync(
										category,
										pageNo,
										pageSize));
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> Details(int? id)
		{
			var data = await _productService.GetProductByIdAsync(id.Value);

			if (!data.Successfull)
			{
				return Problem(data.ErrorMessage);
			}

			return Ok(data.Data);
		}

		[HttpPost]
		[Authorize(Policy = "admin")]
		public async Task<IActionResult> Create(Product product)
		{
			var response = await _productService.CreateProductAsync(product);

			if (!response.Successfull)
			{
				return Problem(response.ErrorMessage);
			}

			return Ok(response.Data);
		}

		[HttpPut("{id:int}")]
		[Authorize(Policy = "admin")]
		public async Task<IActionResult> Edit(int? id)
		{
			throw new NotImplementedException();
		}

		[HttpDelete("{id:int}")]
		[Authorize(Policy = "admin")]
		public async Task<IActionResult> Delete(int? id)
		{
			await _productService.DeleteProductAsync(id.Value);
			return NoContent();
		}
	}
}
