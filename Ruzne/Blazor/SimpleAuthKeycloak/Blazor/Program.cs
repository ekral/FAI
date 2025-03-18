using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazor;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services
    .AddHttpClient<v1Client>()
    .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>().ConfigureHandler(["https://localhost:7114"]));

builder.Services.AddScoped<ClientWrapper>();

builder.Services.AddOidcAuthentication(c =>
{
    c.ProviderOptions.MetadataUrl = "http://localhost:8080/realms/my-realm/.well-known/openid-configuration";
    c.ProviderOptions.Authority = "http://localhost:8080/realms/my-realm";
    c.ProviderOptions.ClientId = "my-blazor";
    c.ProviderOptions.ResponseType = "id_token token";
    c.ProviderOptions.DefaultScopes.Add("some_scope");
});
builder.Services.AddCascadingAuthenticationState();

await builder.Build().RunAsync(); 


class ClientWrapper(v1Client client, NavigationManager nav)
{
    public void Login() => nav.NavigateToLogin("authentication/login");

    public Task<string> GetData() => client.DataAsync();
}

