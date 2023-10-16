using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Utb.Pizzerie.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PizzaContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.MapGet("/pizzerie", PizzerieEndpointsV1.GetAllPizzas);

app.Run();

public static class PizzerieEndpointsV1
{
    public static async Task<Ok<Pizza[]>> GetAllPizzas(PizzaContext context)
    {
        var pizzas = await context.Pizzas.ToArrayAsync();

        return TypedResults.Ok(pizzas);
    }
}