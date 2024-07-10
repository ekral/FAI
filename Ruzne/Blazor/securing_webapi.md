# Securing WebApi

Pokud chceme zabezpečit WebApi, tak nejprve musíme přidat Identity nuget balíček:

```powershell
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```

A také Entity Framework provider pro konkrétní databázi, například:

```powershell
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

Dále vytvoříme DbContext:

```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationSecure
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<IdentityUser>(options)
    {
    }
}
```

A potom nakonfigurejme služby, connection string můžeme uložit do appsettings.json:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=aspnet-BlazorAppSSRSecurity-e4bca3b5-fe3b-4653-a233-0b79ae265fd4;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
Nejprve přidáme dbcontext:

```csharp
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
```

Potom přidáme identity služby do IoC kontejneru (pokud už nejsou zaregistrované):

```csharp
builder.Services.AddAuthorization();
```

Dále aktivujeme Identity APIs:

```csharp
builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();
```

A nakonec namapujeme Identity routes:

```csharp
 app.MapIdentityApi<IdentityUser>();
```

A označíme vybrané API jako autorizované:

```csharp
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (HttpContext httpContext) =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        })
        .ToArray();
    return forecast;
})
.RequireAuthorization()
.WithName("GetWeatherForecast")
.WithOpenApi();
```

## WebApi a Blazor Webassembly

Pokud používáme Webassembly, tak musíme ve WebAPI povolit Cross-origin resource sharing (CORS), z důvodu bezpečnosti je v prohlížeči zakázáno provádět dotazy na jinou doménu (nebo jiný port atd.) než je doména která vytvořila stránku ze které dotaz provádíme (origin). Pokud ale chceme volat WebApi z prohlížeče, tedy typicky volat jinou doménu, tak musíme CORS povolit. Konkrétně povolíme doménu od které dotaz přichází.

Ve WebApi povolíme CORS následujícím způsobem, nejprve přidáme službu a nakonfigurujeme ji. V následujícím příkladu povolujeme i Credentials. Adresa ```https://localhost:7027``` představuje adresu na které běží Blazor Webassemly klient.

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
                      builder =>
                      {
                          builder.WithOrigins("https://localhost:7027")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                      });
});
```

Potom tuto pojmenovanou policy použijeme, kdy ```UseCors``` musí být volané v daném pořadí, například musí být volané dříve než ```UseResponseCaching```: 

```csharp
app.UseCors("MyAllowSpecificOrigins");
```

V Blazor WebAssembly projektu nejprve přidáme třídu ```HttpClient``` do IoC kontejneru a to jak ve WebAssembly projektu s příponou Client, tak do serverového projektu z důvodu prerenderingu. 

V serverovém projektu použijeme:

```csharp
 builder.Services.AddScoped(sp => new HttpClient() { BaseAddress = new Uri("https://localhost:7125/") });
```

Pro WebAssembly klienta musíme nakonfigurovat ```CookieHandler```, abychom povolili ```BrowserRequestCredentials.Include```. Nestačí volat ```request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);``` pro každý dotaz zvlášť, ale musíme použít zmíněný ```CookieHandler```:

```csharp
public class CookieHandler : DelegatingHandler
{
    public CookieHandler()
    {
        InnerHandler = new HttpClientHandler();
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        return base.SendAsync(request, cancellationToken);
    }
}
```

Který pak použijeme následujícím způsobem:

```csharp
  builder.Services.AddScoped(sp => new HttpClient(new CookieHandler()) { BaseAddress = new Uri("https://localhost:7125/") });
```

Poznámka, alternativně lze handler použít příkazy ```builder.Services.AddTransient<CookieHandler>();``` a ```builder.Services.AddHttpClient(...).AddHttpMessageHandler<CookieHandler>();```.

Nyní můžeme aplikaci otestovat, nesmíme ale předtím zapomenout vytvořit databází.

Kód pro login je následující, kdy pro autorizaci pomocí cookies je potřeba zadat query parameter ```useCookies=true```:

```csharp
string email = "ekral@utb.cz";
string password = "Passw0rd";

var result = await HttpClient.PostAsJsonAsync(
"login?useCookies=true", new
{
    email,
    password
});
```

A nakonec můžeme provést dotaz na zabezpečené WebApi:

```csharp
HttpResponseMessage? result = await HttpClient.GetAsync("weatherforecast");

if (result.IsSuccessStatusCode)
{
    WeatherForecast[]? forecasts = await result.Content.ReadFromJsonAsync<WeatherForecast[]>();
}
```

---
1. [How to use Identity to secure a Web API backend for SPAs](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-8.0)
2. [Enable Cross-Origin Requests (CORS) in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-8.0)
3. [Cookie-based request credentials](https://learn.microsoft.com/en-us/aspnet/core/blazor/call-web-api?view=aspnetcore-8.0#cookie-based-request-credentials)