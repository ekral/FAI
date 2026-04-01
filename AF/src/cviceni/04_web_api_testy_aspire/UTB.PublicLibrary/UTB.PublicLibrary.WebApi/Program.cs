using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UTB.PublicLibrary.Contracts;
using UTB.PublicLibrary.Db;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<PublicLibraryContext>("database");

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.MapPost("/dev/seed", Seed);
app.MapGet("/books", GetBooks);
app.MapGet("/books/{id:int}", GetBook);
app.MapPost("/books", CreateBook);
app.MapPut("/books/{id:int}", UpdateBook);
app.MapDelete("/books/{id:int}", DeleteBook);
app.MapPatch("/books/{id:int}", PatchBookArchiveState);

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

static async Task<Ok<BookDto[]>> GetBooks(bool? isArchived, PublicLibraryContext context)
{
    var query = context.Books.AsQueryable();

    if (isArchived.HasValue)
    {
        query = query.Where(book => book.IsArchived == isArchived);
    }

    BookDto[] books = await query.Select(book => new BookDto(book.Id, book.Title, book.IsArchived))
                                 .ToArrayAsync();

    return TypedResults.Ok(books);
}

static async Task<Results<Ok<BookDto>, NotFound>> GetBook(int id, PublicLibraryContext context)
{
    if (await context.Books.FindAsync(id) is Book book)
    {
        return TypedResults.Ok(new BookDto(book.Id, book.Title, book.IsArchived));
    }

    return TypedResults.NotFound();
}

static async Task<Created<BookDto>> CreateBook(BookRequestDto request, PublicLibraryContext context)
{
    var book = new Book { Title = request.Title, IsArchived = request.IsArchived };

    context.Add(book);

    await context.SaveChangesAsync();

    return TypedResults.Created($"/books/{book.Id}", new BookDto(book.Id, book.Title, book.IsArchived));
}

static async Task<Results<NoContent, NotFound>> UpdateBook(int id, BookRequestDto request, PublicLibraryContext context)
{
    if (await context.Books.FindAsync(id) is Book book)
    {
        book.Title = request.Title;
        book.IsArchived = request.IsArchived;

        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}

static async Task<Results<NoContent, NotFound>> DeleteBook(int id, PublicLibraryContext context)
{
    if (await context.Books.FindAsync(id) is Book book)
    {
        context.Books.Remove(book);

        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}

static async Task<Results<NoContent, NotFound>> PatchBookArchiveState(int id, BookPatchRequestDto request, PublicLibraryContext context)
{
    if (await context.Books.FindAsync(id) is Book book)
    {
        book.IsArchived = request.IsArchived;

        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}