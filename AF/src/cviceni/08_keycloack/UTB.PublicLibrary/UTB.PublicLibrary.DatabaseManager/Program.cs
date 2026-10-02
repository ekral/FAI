using Microsoft.AspNetCore.Http.HttpResults;
using UTB.PublicLibrary.Db;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<PublicLibraryContext>("database");

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.MapPost("/dev/seed", Seed);

app.Run();

static async Task<NoContent> Seed(PublicLibraryContext context)
{
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();

    context.Books.AddRange(
        new Book { Title = "Kytice", IsArchived = false },
        new Book { Title = "Bila Nemoc", IsArchived = false },
        new Book { Title = "Babicka", IsArchived = true }
    );

    await context.SaveChangesAsync();

    return TypedResults.NoContent();
}
