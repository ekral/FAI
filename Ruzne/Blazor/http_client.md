# Http klient v Blazor Webassemly

Pro použití třídy ```HttpClient``` ve WebAssembly projektu (s příponou .Client) použijeme například následující kód, který přidá třídu ```HttpClient``` do IoC kontejneru a nakonfiguruje přitom```BaseAdress```:

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

Z důvodu prerenderingu, kdy je komponenta renderovaná nejprve na straně serveru a teprve poté ve WebAssembly, musí být ```HttpClient``` přidaný do IoC kontejneru v obou projektech. V serverovém projektu (bez přípony .Client) můžeme použít předchozí kód včetně použití v razor souboru.

## Named client v serverovém projektu

Také ale můžeme použít metodu ```AddHttpClient``` kdy serverový projekt referencuje frameworky ```Microsoft.AspNetCore.App``` (obsahující ```AddHttpClient```) a ```Microsoft.NETCore.App``` zatímco WebAssembly má referenci pouze na framework ```Microsoft.NETCore.App```.

V následujicím příkladě používáme jednu z možností a to *Named client* a ```IHttpClientFactory```.

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
V předchozích příkladech jsme kvůli zjednodušení zadávali BaseAdress zadávali přímo v kódu, ale většinou jej ukládáme do souboru ```appsettings.json``` a nebo ```appsettings.Development.json```:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "BackendUrl": "https://localhost:7047"
}
```

A v kódu jej pak můžeme použít následujícím způsobem:
```csharp
string backedUrl = builder.Configuration["BackendUrl"] ?? "https://localhost:7047";

builder.Services.AddHttpClient("default", client => client.BaseAddress = new Uri(backedUrl));
```

---
Více se dá zjistit v odkazech:

1. [Call a web API from ASP.NET Core Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/call-web-api?view=aspnetcore-8.0)
2. [Make HTTP requests using IHttpClientFactory in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-8.0)