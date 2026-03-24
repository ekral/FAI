// Pridat nuget Microsoft.EntityFrameworkCore.Sqlite

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();

builder.Services.AddDbContext<LibraryContext>(opt => opt.UseSqlite("Data Source=library.db"));

var app = builder.Build();

app.MapGet("/", () => "Ahoj");
app.MapPost("/dev/seed", Seed);
app.MapGet("/books", GetBooks);
app.MapGet("/books/{id:int}", GetBookById);
app.MapPost("/books", CreateBook);
app.MapDelete("/books/{id:int}", DeleteBook);
app.Run();

async Task<Results<NoContent, NotFound>> DeleteBook(int id, LibraryContext context)
{
    if (await context.Books.FindAsync(id) is Book book)
    {
        context.Books.Remove(book);

        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }
    else
    {
        return TypedResults.NotFound();
    }
}

static async Task<Created<BookDto>> CreateBook(BookRequestDto request, LibraryContext context)
{
    Book book = new Book { Title = request.Title, IsArchived = false };
    context.Books.Add(book);
    await context.SaveChangesAsync();

    BookDto resultDto = new BookDto(book.Id, book.Title);
    return TypedResults.Created($"/books/{book.Id}", resultDto);
}

static async Task<Results<Ok<BookDto>, NotFound>> GetBookById(int id, LibraryContext context)
{
    var book = await context.Books.FindAsync(id);

    if (book != null)
    {
        return TypedResults.Ok(new BookDto(book.Id, book.Title));
    }

    return TypedResults.NotFound();
}

static async Task<Ok<BookDto[]>> GetBooks (bool? isArchived, LibraryContext context)
{
    var query = context.Books.AsQueryable();

    if (isArchived.HasValue)
    {
        query = query.Where(b => b.IsArchived == isArchived);
    }

    var books = await query.Select(b => new BookDto(b.Id, b.Title)).ToArrayAsync();

    return TypedResults.Ok(books);
}

static async Task<NoContent> Seed(LibraryContext context)
{
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();

    var babicka = new Book { Title = "Babicka", IsArchived = false };
    var rur = new Book { Title = "R.U.R.", IsArchived = false };
    var maj = new Book { Title = "Maj", IsArchived = true };

    var loanBabicka = new Loan { Book = babicka, LoanDate = new DateOnly(2026, 3, 18) };
    var loanRur = new Loan { Book = rur, LoanDate = new DateOnly(2026, 3, 17) };

    context.Books.AddRange(babicka, rur, maj);
    context.Loans.AddRange(loanBabicka, loanRur);

    await context.SaveChangesAsync();

    return TypedResults.NoContent();
}

public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Loan> Loans { get; set; }
}

public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public bool IsArchived { get; set; }
    public List<Loan> Loans { get; set; } = [];
}

public class Loan
{
    public int Id { get; set; }
    public required DateOnly LoanDate { get; set; }
    public DateOnly? ReturnDate { get; set; }
    public int BookId { get; set; }
    public Book? Book { get; set; }
}

public record BookDto(int Id, string Title );

public record BookRequestDto(string Title);