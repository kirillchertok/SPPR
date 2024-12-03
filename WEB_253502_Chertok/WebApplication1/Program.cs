using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Serilog;
using WEB_253502_Chertok.Domain.Models;
using WEB_253502_Chertok.Extensions;
using WEB_253502_Chertok.Helpers;
using WEB_253502_Chertok.Middleware;
using WEB_253502_Chertok.Services.Authentication;
using WEB_253502_Chertok.Services.CartService;
using WEB_253502_Chertok.Services.CategoryService;
using WEB_253502_Chertok.Services.FileService;
using WEB_253502_Chertok.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.RegisterCustomServices();

builder.Services.Configure<UriData>(builder.Configuration.GetSection("UriData"));

builder.Services.AddScoped<ICategoryService, ApiCategoryService>()
				.AddScoped<IProductService, ApiProductService>()
				.AddScoped<IFileService, ApiFileService>();

builder.Services.AddHttpClient("api", client =>
{
	var uriData = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<UriData>>().Value;
	client.BaseAddress = new Uri(uriData.ApiUri);
});

builder.Services.AddHttpClient("filesapi", client =>
{
	var uriData = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<UriData>>().Value;
	client.BaseAddress = new Uri($"{uriData.ApiUri}Files");
});

var keyclokData = builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();

builder.Services.AddAuthentication(opt =>
{
	opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
	.AddCookie()
	.AddJwtBearer()
	.AddOpenIdConnect(opt =>
	{
		opt.Authority = $"{keyclokData.Host}/auth/realms/{keyclokData.Realm}";
		opt.ClientId = keyclokData.ClientId;
		opt.ClientSecret = keyclokData.ClientSecret;
		opt.ResponseType = OpenIdConnectResponseType.Code;
		opt.Scope.Add("openid");
		opt.SaveTokens = true;
		opt.RequireHttpsMetadata = false;
		opt.MetadataAddress = $"{keyclokData.Host}/realms/{keyclokData.Realm}/.well-known/openid-configuration";
	});

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();

builder.Services.AddHttpClient<IAuthService, KeycloakAuthService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession();

builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));

builder.Host.UseSerilog((context, configuration) =>
{
	configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLogginMiddleware();

app.MapRazorPages();

//app.MapControllerRoute(
//	name: "areas",
//	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
