using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Utb.PizzaKiosk.Models;

var builder = WebApplication.CreateBuilder(args);

// Zaregistrujte EmailSender jako singleton

builder.Services.AddDbContext<PizzaKioskContext>();

var app = builder.Build();

app.MapGet("/", WebApiVersion1.AllPizzas);
app.MapGet("/Pizza/{id}", WebApiVersion1.GetPizza);

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
            .ThenInclude(pi => pi.Ingredient)
            .ToArrayAsync();


        return TypedResults.Ok(pizzas);
    }

    public static async Task<Results<NotFound, Ok<Pizza>>> GetPizza(int id, PizzaKioskContext context)
    {
        Pizza? pizza = await context.Pizzas.FindAsync(id);

        if(pizza is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(pizza);
    }
}
