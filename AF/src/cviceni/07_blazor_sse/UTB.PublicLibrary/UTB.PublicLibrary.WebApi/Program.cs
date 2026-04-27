using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UTB.PublicLibrary.Contracts;
using UTB.PublicLibrary.Db;
using UTB.PublicLibrary.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<PublicLibraryContext>("database");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7263")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddSingleton<ServerSentEventsService>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();
app.UseCors();

app.MapGet("/sse", GetUpdates);
app.MapGet("/books", GetBooks);
app.MapGet("/books/{id:int}", GetBook);
app.MapPost("/books", CreateBook);
app.MapPut("/books/{id:int}", UpdateBook);
app.MapDelete("/books/{id:int}", DeleteBook);
app.MapPatch("/books/{id:int}", PatchBookArchiveState);

app.Run();

static async Task<ServerSentEventsResult<BookDto[]>> GetUpdates(PublicLibraryContext context, ServerSentEventsService eventsService, CancellationToken cancellationToken)
{
    var students = await context.Books.Select(s => new BookDto(s.Id, s.Title, s.IsArchived)).ToArrayAsync(cancellationToken);

    var values = eventsService.InitAndGetStream(students, cancellationToken);

    return TypedResults.ServerSentEvents(values);
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

static async Task<Created<BookDto>> CreateBook(BookRequestDto request, PublicLibraryContext context, ServerSentEventsService eventsService)
{
    var book = new Book { Title = request.Title, IsArchived = request.IsArchived };

    context.Add(book);

    await context.SaveChangesAsync();

    await SendSse(context, eventsService);

    return TypedResults.Created($"/books/{book.Id}", new BookDto(book.Id, book.Title, book.IsArchived));
}

static async Task<Results<NoContent, NotFound>> UpdateBook(int id, BookRequestDto request, PublicLibraryContext context, ServerSentEventsService eventsService)
{
    if (await context.Books.FindAsync(id) is Book book)
    {
        book.Title = request.Title;
        book.IsArchived = request.IsArchived;

        await context.SaveChangesAsync();

        await SendSse(context, eventsService);

        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}

static async Task<Results<NoContent, NotFound>> DeleteBook(int id, PublicLibraryContext context, ServerSentEventsService eventsService)
{
    if (await context.Books.FindAsync(id) is Book book)
    {
        context.Books.Remove(book);

        await context.SaveChangesAsync();

        await SendSse(context, eventsService);

        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}

static async Task<Results<NoContent, NotFound>> PatchBookArchiveState(int id, BookPatchRequestDto request, PublicLibraryContext context, ServerSentEventsService eventsService)
{
    if (await context.Books.FindAsync(id) is Book book)
    {
        book.IsArchived = request.IsArchived;

        await context.SaveChangesAsync();

        await SendSse(context, eventsService);

        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}

static async Task SendSse(PublicLibraryContext context, ServerSentEventsService eventService)
{
    // Pošli SSE zprávu s novým studentem všem klientům
    var books = await context.Books.Select(s => new BookDto(s.Id, s.Title, s.IsArchived)).ToArrayAsync();

    await eventService.WriteAsync(books);
}