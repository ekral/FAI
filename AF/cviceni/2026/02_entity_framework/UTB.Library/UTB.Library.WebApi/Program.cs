using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UTB.Library.Contracts;
using UTB.Library.Db;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<LibraryContext>("database");

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", () =>
{
    
});

app.Run();

public static class WebApiVersion1
{
    public static async Task<Ok<AuthorDto[]>> GetAllAuthors(LibraryContext context)
    {
        AuthorDto[] authors = await context.Authors.Select(a => new AuthorDto(a.Id, a.Name)).ToArrayAsync();

        return TypedResults.Ok(authors);
    }

    public static async Task<Results<NotFound, Ok<AuthorDto>>> GetAuthor(int id,  LibraryContext context)
    {
        if(await context.Authors.FindAsync(id) is Author author)
        {
            return TypedResults.Ok(new AuthorDto(author.Id, author.Name));
        }
        else
        {
            return TypedResults.NotFound();
        }
    }

}