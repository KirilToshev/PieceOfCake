using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace PieceOfCake.BlazorApp.Services.MessageHandlers
{
    public class PieceOfCakeApiMessageHandler : AuthorizationMessageHandler
    {
        public PieceOfCakeApiMessageHandler(
            IAccessTokenProvider provider, NavigationManager navigation)
            : base(provider, navigation)
        {
            ConfigureHandler(
                  authorizedUrls: new[] { "https://localhost:44312/" });
        }
    }
}
