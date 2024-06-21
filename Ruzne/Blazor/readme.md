# Http klient v Blazor Webassemly

Ve WebAssembly projektu (s příponou .Client) použijeme následující kód, kdy adresa je jen ukázková a změníme ji na konkrétní adresu nebo načteme z konfigurace.

```csharp
builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri("https://localhost:5002")
    });
```

Z důvodu prerenderingu, kdy je komponenta renderovaná nejprve na straně serveru a teprve poté ve WebAssembly, musí být Http client přidaný do IoC kontejneru v obou projektech. V SSR projektu (bez přípony .Client) můžeme použít metodu ```AddHttpClient``` díky tomu, že tento projekt referencuje frameworky ```Microsoft.AspNetCore.App``` a ```Microsoft.NETCore.App```. Zatímco WebAssembly má referenci pouze na framework ```Microsoft.NETCore.App```. 

```csharp
builder.Services.AddHttpClient<HttpClient>(client => 
    client.BaseAddress = new Uri("https://localhost:5002"));
```