using Microsoft.EntityFrameworkCore;
using WEB_253502_Chertok.API.Data;
using WEB_253502_Chertok.API.Models;
using WEB_253502_Chertok.API.Services.CategoryService;
using WEB_253502_Chertok.API.Services.ProductService;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
	opt.UseNpgsql(builder.Configuration.GetConnectionString("SPPRProjectDbContext")));

builder.Services.AddScoped<IProductService, ProductService>()
				.AddScoped<ICategoryService, CategoryService>();

var authService = builder.Configuration.GetSection("AuthServer").Get<AuthServerData>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
	{
		opt.Authority = $"{authService.Host}/realms/{authService.Realm}";

		opt.MetadataAddress = $"{authService.Host}/realms/{authService.Realm}/.well-known/openid-configuration";

		opt.Audience = "account";

		opt.RequireHttpsMetadata = false;
	});

builder.Services.AddAuthorization(opt =>
{
	opt.AddPolicy("admin", p => p.RequireRole("POWER-USER"));
});

builder.Services.AddCors(b =>
{
	b.AddPolicy("default", policy =>
	{
		policy.AllowAnyMethod()
			  .AllowAnyOrigin()
			  .AllowAnyHeader();
	});
});

var app = builder.Build();

using var scope = app.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

var pendingMigrations = context.Database.GetPendingMigrations();

if (pendingMigrations.Any())
{
	await context.Database.MigrateAsync();
}

//await DbInitializer.SeedData(app);

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();

app.UseCors("default");

app.Run();
