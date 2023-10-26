using Microsoft.EntityFrameworkCore;
using Utb.PizzaKiosk.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PizzaKioskContext>();

var app = builder.Build();

app.MapGet("/", (PizzaKioskContext context) => context.Pizzas.Include(p => p.PizzaIngredients).ThenInclude(pi => pi.Ingredient));
app.MapGet("/Orders", (PizzaKioskContext context) => context.Orders.Include(o => o.OrderedPizzas));

app.Run();
