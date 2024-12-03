using Microsoft.EntityFrameworkCore;
using WEB_253502_Chertok.Domain.Entities;

namespace WEB_253502_Chertok.API.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
	}
}
