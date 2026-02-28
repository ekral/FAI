using UTB.Library.Db;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<LibraryContext>("database");

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapPost("/reset-db", async (LibraryContext context) =>
{
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();

    Author a1 = new() { Name = "Karel Capek" };
    Author a2 = new() { Name = "Jaroslav Hasek" };
    Author a3 = new() { Name = "Bohumil Hrabal" };

    context.Authors.AddRange(a1, a2, a3);

    await context.SaveChangesAsync();
});

app.UseHttpsRedirection();

app.Run();
