using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253502_Chertok.API.Data;
using WEB_253502_Chertok.API.Services.ProductService;
using WEB_253502_Chertok.Domain.Entities;
using WEB_253502_Chertok.Domain.Models;

namespace WEB_253502_Chertok.Tests
{
	public class ProductServiceTests
	{
		private readonly DbContextOptions<AppDbContext> _dbContextOptions;
		public ProductServiceTests()
		{
			_dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
										.UseInMemoryDatabase("ProductServiceTests")
										.Options;
		}

		[Fact]
		public void Handle_ValidRequest_ShouldReturnPaginatedListWith3ItemsAndCorrectlyCountTotalPages()
		{
			// Arrange
			using var context = CreateContext();
			var service = new ProductService(context);

			// Act
			var result = service.GetProductListAsync(null).Result;

			// Assert
			Assert.IsType<ResponseData<ProductListModel<Product>>>(result);
			Assert.True(result.Successfull);
			Assert.Equal(1, result.Data.CurrentPage);
			Assert.Equal(3, result.Data.Items.Count);
			Assert.Equal(2, result.Data.TotalPages);
			Assert.Equal(context.Products.First(), result.Data.Items[0]);
		}

		[Fact]
		public void Handle_ValidReuqest_ShouldCorrectlyChooseGivenPage()
		{
			// Arrange
			using var context = CreateContext();
			var service = new ProductService(context);
			int pageNo = 2;

			// Act
			var result = service.GetProductListAsync(null, pageNo: 2).Result;

			// Assert
			Assert.IsType<ResponseData<ProductListModel<Product>>>(result);
			Assert.True(result.Successfull);
			Assert.Equal(2, result.Data.CurrentPage);
		}

		[Fact]
		public void Handle_ValidRequest_ShouldCorrectlyFilterByCategory()
		{
			// Arrange
			using var context = CreateContext();
			var service = new ProductService(context);
			string category = "name-1";

			// Act
			var result = service.GetProductListAsync(category).Result;

			// Assert
			Assert.IsType<ResponseData<ProductListModel<Product>>>(result);
			Assert.True(result.Successfull);
			Assert.Equal(1, result.Data.Items.Count);
		}

		[Fact]
		public void Handle_SetPageSizeGreaterThanMaximum_ShouldNotAllowSet()
		{
			// Arrange
			using var context = CreateContext();
			var service = new ProductService(context);
			int pageSize = 54;

			// Act
			var result = service.GetProductListAsync(null, pageSize: pageSize).Result;

			// Assert
			Assert.IsType<ResponseData<ProductListModel<Product>>>(result);
			Assert.True(result.Successfull);
			Assert.True((int)Math.Ceiling(result.Data.Items.Count / (double)result.Data.TotalPages) != pageSize);
		}

		[Fact]
		public void Handle_PageNoGreaterThanMaximumRequest_ReturnsSuccesfullIsFalse()
		{
			// Arrange
			using var context = CreateContext();
			var service = new ProductService(context);
			int pageNo = 54;

			// Act
			var result = service.GetProductListAsync(null, pageNo: pageNo).Result;

			// Assert
			Assert.IsType<ResponseData<ProductListModel<Product>>>(result);
			Assert.False(result.Successfull);
		}

		private AppDbContext CreateContext()
		{
			var context = new AppDbContext(_dbContextOptions);

			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			context.Products.AddRange(
				new Product { Description = "Name1", Price = 1, CategoryId = 1 },
				new Product { Description = "Name2", Price = 2, CategoryId = 2 },
				new Product { Description = "Name3", Price = 3, CategoryId = 3 },
				new Product { Description = "Name3", Price = 3, CategoryId = 3 },
				new Product { Description = "Name3", Price = 3, CategoryId = 3 },
				new Product { Description = "Name3", Price = 3, CategoryId = 3 }
			);

			context.Categories.AddRange(
				new Category { Id = 1, Name = "Name1", NormalizedName = "name-1" },
				new Category { Id = 2, Name = "Name2", NormalizedName = "name-2" },
				new Category { Id = 3, Name = "Name3", NormalizedName = "name-3" });

			context.SaveChanges();

			return context;
		}
	}
}
