using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UTB.ConcurrencyTest.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<MenuContext>("database");

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapPost("/dev/seed", Seed);
app.MapPost("/dev/test", Test);

app.UseHttpsRedirection();


app.Run();

static async Task<NoContent> Seed(MenuContext context)
{
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();

    var menu = new Menu() { Id = 1, Title = "smazak", Quantity = 1 };

    context.Menus.Add(menu);

    await context.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task Test(IServiceScopeFactory scopeFactory)
{
    await using var scopeA = scopeFactory.CreateAsyncScope();
    await using var scopeB = scopeFactory.CreateAsyncScope();

    var contextA = scopeA.ServiceProvider.GetRequiredService<MenuContext>();
    var contextB = scopeB.ServiceProvider.GetRequiredService<MenuContext>();

    var menuA = await contextA.Menus.FirstAsync();
    var menuB = await contextB.Menus.FirstAsync();

    if (menuA.Quantity > 0)
    {
        --menuA.Quantity;
        await contextA.SaveChangesAsync();
    }

    if (menuB.Quantity > 0)
    {
        --menuB.Quantity;
        await contextB.SaveChangesAsync();
    }
}



