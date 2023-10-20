using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Utb.PizzaKiosk.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PizzaContext>();
var app = builder.Build();

app.MapGet("/", WebApiV1.GetAllPizzas);

app.Run();

public static class WebApiV1
{
    public static async Task<Ok<Pizza[]>> GetAllPizzas(PizzaContext context)
    {
        Pizza[] pizzas = await context.Pizzas.ToArrayAsync();

        return TypedResults.Ok(pizzas);
    }
}
