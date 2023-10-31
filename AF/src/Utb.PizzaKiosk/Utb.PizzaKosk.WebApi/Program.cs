using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Utb.PizzaKiosk.Models;
using Utb.PizzaKosk.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IEmailSender, MockEmailSender>();
builder.Services.AddDbContext<PizzaKioskContext>();

var app = builder.Build();

app.MapGet("/", WebApiVersion1.AllPizzas);
app.MapGet("/Pizza/{id}", WebApiVersion1.GetPizza);
app.MapPost("/AddIngredient", WebApiVersion1.AddIngredient);
app.MapPost("/CreateOrder", WebApiVersion1.CreateOrder);
app.MapGet("/Orders", WebApiVersion1.AllOrders);

app.Run();

public static class WebApiVersion1
{
    public static async Task<Ok<Order[]>> AllOrders(PizzaKioskContext context)
    {
        Order[] orders = await context
            .Orders
            .Include(o => o.OrderedPizzas)
            .ThenInclude(op => op.OrderedIngredients)
            .ToArrayAsync();


        return TypedResults.Ok(orders);
    }

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

        if (pizza is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(pizza);
    }

    public static async Task<Created<Ingredient>> AddIngredient(Ingredient ingredient, IEmailSender emailSender, PizzaKioskContext context)
    {
        OrderDTO order = new OrderDTO(
            FullfilmentOptionType.DineIn, 
            new PizzaDTO[] 
            { 
                new PizzaDTO(1, new IngredientDTO[]
                {
                    new IngredientDTO(1, 3),
                    new IngredientDTO(2, 1)
                }) 
            
            });

        string json = JsonSerializer.Serialize(order);

        context.Add(ingredient);

        await context.SaveChangesAsync();

        emailSender.SendEmail();

        return TypedResults.Created($"Ingredients/{ingredient.Id}", ingredient);
    }

    // CreateOrder

    public static void CreateOrder(OrderDTO order, PizzaKioskContext context)
    {
        // TODO Business Code
    }
}
