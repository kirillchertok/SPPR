using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WEB_253502_Chertok.BlazorWasm;
using WEB_253502_Chertok.BlazorWasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetSection("DataApi").Value) });

builder.Services.AddOidcAuthentication(options =>
{
    //builder.Configuration.Bind("Local", options.ProviderOptions);
    builder.Configuration.Bind("Keycloak", options.ProviderOptions);
    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("email");
});

builder.Services.AddScoped<IDataService, DataService>();

await builder.Build().RunAsync();
