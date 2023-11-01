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

    public async static Task<Results<BadRequest, Created<Order>>> CreateOrder(OrderDTO orderDTO, PizzaKioskContext context)
    {
        // TODO add code to compute price and create order according to the orderDTO

        Order order = new Order()
        {
            FullfilmentOption = orderDTO.FullfilmentOption,
            OrderStatus = OrderStatusType.Processing,
            TotalPrice = 0.0m
        };

        foreach (var pizzaDto in orderDTO.Pizzas)
        {
            Pizza? pizza = context.Pizzas.Find(pizzaDto.PizzaId);

            if(pizza is null)
            {
                return TypedResults.BadRequest();
            }

            OrderedPizza orderedPizza = new OrderedPizza()
            {
                Id = 0,
                OrderId = 0,
                Name = pizza.Name,
                TotalPrice = pizza.Price
            };

            foreach (IngredientDTO ingredientDto in pizzaDto.Ingredients)
            {
                PizzaIngredient? pizzaIngredient = context.PizzaIngredients.Find(pizzaDto.PizzaId, ingredientDto.IngredientId);

                if (pizzaIngredient is null)
                {
                    return TypedResults.BadRequest();
                }

                context.Entry(pizzaIngredient).Reference(p => p.Ingredient).Load(); // Explicit loading

                if (pizzaIngredient.Ingredient is null)
                {
                    return TypedResults.BadRequest();
                }

                // TODO calculate minimal quantity
                int paidQuantity = ingredientDto.Quantity - pizzaIngredient.MinimalQuantity;

                if(paidQuantity < 0)
                {
                    paidQuantity = 0;
                }

                OrderedIngredient orderedIngredient = new OrderedIngredient()
                {
                     Id = 0,
                     OrderedPizzaId = 0,
                     FreeQuantity = pizzaIngredient.FreeQuantity,
                     PaidQuantity = paidQuantity,
                     Name = pizzaIngredient.Ingredient.Name,
                       
                };
            }
        }

        //Order order = new Order()
        //{
        //    Id = 0,
        //    FullfilmentOption = FullfilmentOptionType.DineIn,
        //    OrderStatus = OrderStatusType.Processing,
        //    TotalPrice = 200.0m,

        //    OrderedPizzas = new List<OrderedPizza>()
        //    {
        //        new OrderedPizza()
        //        {
        //            Id = 0,
        //            Name = "Nova",
        //            OrderId = 0,
        //            TotalPrice = 300,
        //            OrderedIngredients = new List<OrderedIngredient>()
        //            {
        //                new OrderedIngredient()
        //                {
        //                    Id = 0,
        //                    Name = "Neco",
        //                    FreeQuantity = 0,
        //                    OrderedPizzaId = 0,
        //                    PaidQuantity = 1,
        //                    QuantityDescription = "10 g",
        //                    TotalPrice = 20.0m,
        //                    UnitPrice = 20.0m
        //                }
        //            }
        //        }
        //    }
        //};

        context.Add(order);

        await context.SaveChangesAsync();

        return TypedResults.Created($"Order/{order.Id}", order);
    }
}
