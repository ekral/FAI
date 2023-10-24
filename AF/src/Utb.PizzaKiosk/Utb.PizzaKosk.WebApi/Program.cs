using Microsoft.EntityFrameworkCore;
using Utb.PizzaKiosk.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PizzaKioskContext>();
builder.Services.AddSingleton<IMojeSluzba, MojeSluzba>();

var app = builder.Build();

app.MapGet("/", (PizzaKioskContext context, IMojeSluzba sluzba) => context.PizzaIngredients.Include(p => p.Ingredient));

app.Run();

interface IMojeSluzba
{
    int VratCislo();
}

class MojeSluzba : IMojeSluzba
{
    public int VratCislo()
    {
        return Random.Shared.Next(1, 99);
    }
}