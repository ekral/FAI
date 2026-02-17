using Microsoft.AspNetCore.Authorization;
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

app.MapPost("/authors", WebApiVersion1.CreateAuthor);
app.MapGet("/authors", WebApiVersion1.GetAllAuthors);
app.MapGet("/authors/{id:int}", WebApiVersion1.GetAuthor);
app.MapPut("/authors/{id:int}", WebApiVersion1.UpdateAuthor);
app.MapDelete("/authors/{id:int}", WebApiVersion1.DeleteAuthor);

app.Run();

public static class WebApiVersion1
{
    public static async Task<Created<AuthorDto>> CreateAuthor(AuthorDto authorDto, LibraryContext context)
    {
        Author author = new() { Name = authorDto.Name };

        context.Authors.Add(author);

        await context.SaveChangesAsync();

        AuthorDto resultDto = new(author.Id, author.Name);

        return TypedResults.Created($"/authors/{resultDto.Id}", resultDto);
    }

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

    public static async Task<Results<NoContent, NotFound>> UpdateAuthor(int id, AuthorDto authorDto, LibraryContext context)
    {
        if (await context.Authors.FindAsync(id) is Author author)
        {
            author.Name = authorDto.Name;

            await context.SaveChangesAsync();

            return TypedResults.NoContent();
        }
        else
        {
            return TypedResults.NotFound();
        }
    }

    public static async Task<Results<NoContent, NotFound>> DeleteAuthor(int id, LibraryContext context)
    {
        if (await context.Authors.FindAsync(id) is Author author)
        {
            context.Authors.Remove(author);

            await context.SaveChangesAsync();

            return TypedResults.NoContent();
        }
        else
        {
            return TypedResults.NotFound();
        }
    }
}