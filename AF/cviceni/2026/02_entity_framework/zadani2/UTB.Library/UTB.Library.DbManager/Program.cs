using UTB.Library.Db;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// 🚀 vložení LibraryContextu do IoC kontejneru.

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapPost("/reset-db", async (LibraryContext context) =>
{
    // 🚀 smazání databáze pokud existuje,
    // 🚀 vytvoření databáze pokdu neexistuje,
    // 🚀 vložení tří autorů do databáze.
});

app.UseHttpsRedirection();

app.Run();
