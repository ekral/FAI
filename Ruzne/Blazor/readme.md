# Http klient v Blazor Webassemly

Ve WebAssembly projektu (s příponou .Client) použijeme následující kód, kdy adresa je jen ukázková a změníme ji na konkrétní adresu nebo načteme z konfigurace.

```csharp
builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri("https://localhost:5002")
    });
```

Z důvodu prerenderingu, kdy je komponenta renderovaná nejprve na straně serveru a teprve poté ve WebAssembly, musí být Http client přidaný do IoC kontejneru v obou projektech. V SSR projektu (bez přípony .Client) můžeme použít stejný způsobe jako předchozí, tedy:

```csharp
builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri("https://localhost:5002")
    });
```

A potom injektujeme závislost v razor souboru:
```razor
@inject HttpClient HttpClient

@code {
    protected override async Task OnInitializedAsync()
    {
        var forecasts = await HttpClient.GetFromJsonAsync<WeatherForecast[]>("weatherforecast");
    }
}
```

Také ale můžeme použít ```AddHttpClient``` kdy serverový projekt referencuje frameworky ```Microsoft.AspNetCore.App``` (obsahující ```AddHttpClient```) a ```Microsoft.NETCore.App``` zatímco WebAssembly má referenci pouze na framework ```Microsoft.NETCore.App```.

V následujicím příkladě používáme jednu z možností a to **Named clienta* a ```IHttpClientFactory```.

```csharp
builder.Services.AddHttpClient("default", client => client.BaseAddress = new Uri("https://localhost:7047"));
```

A potom injektujeme závislost v razor souboru:
```razor
@inject IHttpClientFactory Factory

@code {
    protected override async Task OnInitializedAsync()
    {
        HttpClient httpClient = Factory.CreateClient("default");

        var forecasts = await httpClient.GetFromJsonAsync<WeatherForecast[]>("weatherforecast");
    }
}
```

---
Více se dá zjistit v odkazech:

1. [Call a web API from ASP.NET Core Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/call-web-api?view=aspnetcore-8.0)
2. [Make HTTP requests using IHttpClientFactory in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-8.0)