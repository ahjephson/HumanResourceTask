using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace HumanResourceTask.Web.Services
{
    public class BackendAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public BackendAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation, IConfiguration configuration) : base(provider, navigation)
        {
            ConfigureHandler(new[] { configuration["ApiUrl"] ?? "https://localhost:7204" });
        }
    }
}
