using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253502_Chertok.API.Data;
using WEB_253502_Chertok.API.Services.CategoryService;
using WEB_253502_Chertok.Domain.Entities;

namespace WEB_253502_Chertok.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
		private readonly ICategoryService _categoryService;

		public CategoriesController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		public async Task<IActionResult> GetCategories()
		{
			return Ok(await _categoryService.GetCategoryListAsync());
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> Details(int? id)
		{
			throw new NotImplementedException();
		}
		[HttpPost]
		public IActionResult Create()
		{
			throw new NotImplementedException();
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> Edit(int? id)
		{
			throw new NotImplementedException();
		}
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete(int? id)
		{
			throw new NotImplementedException();
		}
	}
}
