using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UTB.Library.Contracts;
using UTB.Library.Db;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// 🚀 vložení LibraryContextu do IoC kontejneru.

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
        // 🚀 1. Přidání nového autora do databáze.

        AuthorDto resultDto = new(0, "nikdo");

        return TypedResults.Created($"/authors/{resultDto.Id}", resultDto);
    }

    public static async Task<Ok<AuthorDto[]>> GetAllAuthors(LibraryContext context)
    {
        // 🚀 2.Vrácení všech autorů z databáze.

        AuthorDto[] authors = [];

        return TypedResults.Ok(authors);
    }

    public static async Task<Results<NotFound, Ok<AuthorDto>>> GetAuthor(int id,  LibraryContext context)
    {
        // 📖 3. Vrácení jednoho autora podle id (už je implementováno, jen ho zkontrolujte).

        if (await context.Authors.FindAsync(id) is Author author)
        {
            AuthorDto authorDto = new(author.Id, author.Name);

            return TypedResults.Ok(authorDto);
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
            // 🚀 4. Změna autora v databázi.

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
            // 🚀 5. Odstranění autora z databáze.

            return TypedResults.NoContent();
        }
        else
        {
            return TypedResults.NotFound();
        }
    }
}