
using WEB_253502_Chertok.Helpers;
using WEB_253502_Chertok.Services.CategoryService;
using WEB_253502_Chertok.Services.ProductService;

namespace WEB_253502_Chertok.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryService, MemoryCategoryService>()
                            .AddScoped<IProductService, MemoryProductService>();

			builder.Services.Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));
		}
    }
}
