# Http klient v Blazor Webassemly

Ve Webassemly projektu (s příponou .Client) použijeme následující kód, kdy adresa je jen ukázková a změníme ji na konkrétní adresu nebo načteme z konfigurace.

```csharp
builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri("https://localhost:5002")
    });
```

