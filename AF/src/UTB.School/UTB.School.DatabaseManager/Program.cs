using Microsoft.AspNetCore.Http.HttpResults;
using UTB.School.Db;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<SchoolContext>("database");

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.MapPost("/dev/seed", SeedDatabase);

app.Run();

static async Task<NoContent> SeedDatabase(SchoolContext context)
{
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();

    context.Students.AddRange(
        new Student { Name = "Jan", IsActive = true },
        new Student { Name = "Eva", IsActive = true },
        new Student { Name = "Petr", IsActive = false }
    );

    await context.SaveChangesAsync();

    return TypedResults.NoContent();
}