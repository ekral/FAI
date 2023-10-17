# Úkol na cvičení: Testy pro Model Utb.Kiosk

## Minimal WebApi

Do projektu Utb.Kiosk přidejte projekt Minimal Web Api a vytvořet metodu, která vrátí všechny pizzy v pizzerie. Využijte model vytvořený v minulém projektu.

Pokud nemáte vlastní řešení minulých úkolů, tak můžete použít projekt: 

[Utb.PizzaKiosk.UkazkaTestu](https://github.com/ekral/FAI/blob/master/AF/src/Utb.PizzaKiosk.UkazkaTestu)

1) Pomocí migrací vytvořte databázi.
2) Přidejte do Solution nový projekt *Asp.net core Empty* s názvem *Utb.PizzaKiosk.WebApi*.
3) Do projektu *Utb.PizzaKiosk.WebApi* přidejte referenci na projekt obsahující DbContext, například *Utb.PizzaKiosk.Models*.
4) Zaregistrujte `PizzaContext` do IoC kontejneru dle vzoru v následující kódu.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PizzaContext>();

var app = builder.Build();
```

5) Přidejte parametr typu PizzaContext do metody obsluhující dotaz *GET*:

```csharp
app.MapGet("/", (PizzaContext context) => context.Pizzas);
```

## Unit Testy pro WebApi.

Otestujte Unit Testu pomocí frameworku xUnit.

1) Pro potřeby testování a dokumentace je vhodné si definovat přesný typ návratové hodnoty a kvůli přehlednosti a možnosti testování v unit testu můžeme použít statickou metodu místo lambdy.

```csharp
app.MapGet("/", WebApiV1.GetAllPizzas);

public static class WebApiV1
{
    public static async Task<Ok<Pizza[]>> GetAllPizzas(PizzaContext context)
    {
        Pizza[] pizzas = await context.Pizzas.ToArrayAsync();

        return TypedResults.Ok(pizzas);
    }
}
```
2) Do projektu *Utb.PizzaKiosk.Test* přidejte referenci na projekt *Utb.PizzaKiosk.WebApi*.
3) Do projektu *Utb.PizzaKiosk.Test* přidejte novou testovací třídu `UnitTestPizzaWebApi`:

 
---
Tutoriály a materiály k vypracování

- [Tutorial: Create a minimal API with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-7.0&tabs=visual-studio)
- [Příprava: Entity framework základy](https://github.com/ekral/FAI/blob/master/AF/Priprava/01_EF_zaklady.md)
- [Vzorové řešení Utb.Studenti](https://github.com/ekral/FAI/tree/master/AF/src/Utb.Studenti).
- [Getting Started with EF Core](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli).
- [Testing against your production database system](https://learn.microsoft.com/en-us/ef/core/testing/testing-with-the-database).
- [Utb.PizzaKiosk](https://github.com/ekral/FAI/tree/master/AF/src/Utb.PizzaKiosk).
---
  
