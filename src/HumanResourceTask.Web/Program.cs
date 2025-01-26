using HumanResourceTask.Web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace HumanResourceTask.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddOptions<ApplicationOptions>();
            builder.Services.Configure<ApplicationOptions>(builder.Configuration.Bind);

            var apiUrl = builder.Configuration["ApiUrl"] ?? "https://localhost:7204";

            builder.Services.AddTransient<BackendAuthorizationMessageHandler>();

            builder.Services
                .AddHttpClient("WebAPI", client => client.BaseAddress = new Uri(apiUrl))
                .AddHttpMessageHandler<BackendAuthorizationMessageHandler>();

            builder.Services
                .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("WebAPI"));

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                options.ProviderOptions.LoginMode = "redirect";
                options.ProviderOptions.DefaultAccessTokenScopes.Add("openid");
            });

            builder.Services.AddAuthorizationCore();

            builder.Services.AddMudServices();

            builder.Services.AddScoped<IApiClient, ApiClient>();

            await builder.Build().RunAsync();
        }
    }
}
