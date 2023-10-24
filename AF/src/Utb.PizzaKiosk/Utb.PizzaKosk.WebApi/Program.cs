using Utb.PizzaKiosk.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PizzaKioskContext>();

var app = builder.Build();

app.MapGet("/", (PizzaKioskContext context) => context.Pizzas);

app.Run();
