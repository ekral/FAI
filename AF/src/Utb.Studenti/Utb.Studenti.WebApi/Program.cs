using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Utb.Studenti.Models;

// Jen pro vyvoj, pouzit migrace!
using (StudentContext context = new())
{
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<StudentContext>();

var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/studenti", StudentEndpointsV1.GetAllStudents);

app.Run();

public static class StudentEndpointsV1
{
    public static async Task<Ok<Student[]>> GetAllStudents(StudentContext context)
    {
        var studenti = await context.Studenti.ToArrayAsync();

        return TypedResults.Ok(studenti);
    }
}