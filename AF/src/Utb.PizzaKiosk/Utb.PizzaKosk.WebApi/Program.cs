using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Utb.PizzaKiosk.Data;
using Utb.PizzaKiosk.Models;
using Utb.PizzaKiosk.Models.DTO;
using Utb.PizzaKosk.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Kolik MockEmailSenderu bude vytvoreno po dvou requestech?
builder.Services.AddScoped<IEmailSender, MockEmailSender>();
builder.Services.AddTransient<MyService>();
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

    public static async Task<Created<Ingredient>> AddIngredient(Ingredient ingredient, MyService service, IEmailSender emailSender, PizzaKioskContext context)
    {
        context.Add(ingredient);

        await context.SaveChangesAsync();

        emailSender.SendEmail();

        return TypedResults.Created($"Ingredients/{ingredient.Id}", ingredient);
    }

    // CreateOrder

    public async static Task<Results<BadRequest, Created<Order>>> CreateOrder(OrderDTO clientOrder, PizzaKioskContext context)
    {

        // Primarni i cizi klice doplni Entity Framework

        Order order = new Order()
        {
            FullfilmentOption = clientOrder.FullfilmentOption,
            OrderStatus = OrderStatusType.Processing,
            TotalPrice = 0.0m
        };

        foreach (var clientOrderPizza in clientOrder.Pizzas)
        {
            Pizza? pizzaEntity = context.Pizzas.Find(clientOrderPizza.PizzaId);

            if (pizzaEntity is null)
            {
                return TypedResults.BadRequest();
            }

            OrderedPizza orderedPizza = new OrderedPizza()
            {
                Id = 0,
                OrderId = 0,
                Name = pizzaEntity.Name,
                UnitPrice = pizzaEntity.Price,
                TotalPrice = pizzaEntity.Price
            };

            foreach (IngredientDTO clientOrderIngredient in clientOrderPizza.Ingredients)
            {
                PizzaIngredient? pizzaIngredientEntity = context.PizzaIngredients.Find(clientOrderPizza.PizzaId, clientOrderIngredient.IngredientId);

                if (pizzaIngredientEntity is null)
                {
                    return TypedResults.BadRequest();
                }

                context.Entry(pizzaIngredientEntity).Reference(p => p.Ingredient).Load(); // Explicit loading

                if (pizzaIngredientEntity.Ingredient is null)
                {
                    return TypedResults.BadRequest();
                }

                if (clientOrderIngredient.Quantity < pizzaIngredientEntity.MinimalQuantity)
                {
                    return TypedResults.BadRequest();
                }

                int paidQuantity = clientOrderIngredient.Quantity - pizzaIngredientEntity.FreeQuantity;

                if (paidQuantity < 0)
                {
                    paidQuantity = 0;
                }

                decimal ingredientTotalPrice = paidQuantity * pizzaIngredientEntity.Ingredient.UnitPrice;

                OrderedIngredient orderedIngredient = new OrderedIngredient()
                {
                    Id = 0,
                    OrderedPizzaId = 0,
                    PaidQuantity = paidQuantity,
                    FreeQuantity = pizzaIngredientEntity.FreeQuantity,
                    Name = pizzaIngredientEntity.Ingredient.Name,
                    QuantityDescription = pizzaIngredientEntity.Ingredient.QuantityDescription,
                    UnitPrice = pizzaIngredientEntity.Ingredient.UnitPrice,
                    TotalPrice = ingredientTotalPrice
                };

                orderedPizza.OrderedIngredients.Add(orderedIngredient);

                orderedPizza.TotalPrice += orderedIngredient.TotalPrice;

            }

            order.OrderedPizzas.Add(orderedPizza);

            order.TotalPrice += orderedPizza.TotalPrice;
        }

        context.Add(order);

        await context.SaveChangesAsync();

        return TypedResults.Created($"Order/{order.Id}", order);
    }
}