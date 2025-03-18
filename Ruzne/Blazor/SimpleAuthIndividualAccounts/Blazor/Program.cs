using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazor;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();

builder.Services.AddHttpClient<v1Client>().AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped<X>();
builder.Services.AddScoped<CookieHandler>();

await builder.Build().RunAsync();


class X(v1Client client)
{
    public async Task DoStuf()
    {
        await client.AccountRegisterAsync(new RegisterRequest { Email = "tom@tom.com", Password = "Password123@" });
        try
        {
            var resp = await client.AccountLoginAsync(true, false, new LoginRequest { Email = "tom@tom.com", Password = "Password123@" });
        }
        catch { }

        var data = await client.DataAsync();

        Console.WriteLine($"data: {data}");
    }
}


public class CookieHandler : DelegatingHandler
{
    /// <summary>
    /// Main method to override for the handler.
    /// </summary>
    /// <param name="request">The original request.</param>
    /// <param name="cancellationToken">The token to handle cancellations.</param>
    /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // include cookies!
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        //request.Headers.Add("X-Requested-With", ["XMLHttpRequest"]);

        return base.SendAsync(request, cancellationToken);
    }
}