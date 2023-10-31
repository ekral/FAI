using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Utb.PizzaKiosk.Models;

var builder = WebApplication.CreateBuilder(args);

// Zaregistrujte EmailSender jako singleton

builder.Services.AddDbContext<PizzaKioskContext>();

var app = builder.Build();

app.MapGet("/", WebApiVersion1.AllPizzas);

app.MapGet("/Orders", (PizzaKioskContext context) => 
context
    .Orders
    .Include(o => o.OrderedPizzas)
    .ThenInclude(op => op.OrderedIngredients));

app.Run();

public static class WebApiVersion1
{
    public static async Task<Ok<Pizza[]>> AllPizzas(PizzaKioskContext context)
    {
        Pizza[] pizzas = await context
            .Pizzas
            .Include(p => p.PizzaIngredients)
            .ThenInclude(pi => pi.Ingredient).ToArrayAsync();


        return TypedResults.Ok(pizzas);
    }
}
