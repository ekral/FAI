using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            //builder.Services.AddHttpClient("WebAPI", client => client.BaseAddress = new Uri("https://localhost:7274"))
            //                .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>().ConfigureHandler(["https://localhost:7274"])); ;

            //builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
            //    .CreateClient("WebAPI"));

            builder.Services.AddScoped(sp =>
            {
                AuthorizationMessageHandler handler = sp.GetRequiredService<AuthorizationMessageHandler>();
                handler.InnerHandler = new HttpClientHandler();
                handler.ConfigureHandler(["https://localhost:7274"]);

                return new HttpClient(handler) { BaseAddress = new Uri("https://localhost:7274") };
            });
            
            builder.Services.AddOidcAuthentication(c =>
            {
                c.ProviderOptions.Authority = "http://127.0.0.1:8080/realms/myrealm";
                c.ProviderOptions.ClientId = "myclient";
                c.ProviderOptions.ResponseType = "code";
                //c.ProviderOptions.RedirectUri = "https://localhost:7221";

            });

            builder.Services.AddCascadingAuthenticationState();

            await builder.Build().RunAsync();
        }
    }
}
