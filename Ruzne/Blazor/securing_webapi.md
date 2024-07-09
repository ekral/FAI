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

Nyní můžeme aplikaci otestovat, nesmíme zapomenout vytvořit databází a pokud používáme OpenApi tak je potřeba zadat UseCokies.

V Blazor WebAssemlby projektu nejprve přidáme třídu ```HttpClient``` do IoC kontejneru a to jak ve Webassembly projektu s příponou Client, tak do serverového projektu z důvodu prerenderingu:

```csharp
 builder.Services.AddScoped(sp => new HttpClient() { BaseAddress = new Uri("https://localhost:7125/") });
```

Poté přidáme kód pro Login do WebApi:

```csharp
    private async void Login()
    {
        string email = "ekral@utb.cz";
        string password = "Passw0rd";

        var resultLogin = await HttpClient.PostAsJsonAsync(
        "login?useCookies=true", new
        {
            email,
            password
        });
    }
```

---
1. [How to use Identity to secure a Web API backend for SPAs](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-8.0)