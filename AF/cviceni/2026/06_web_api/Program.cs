using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LibraryContext>(options => options.UseSqlite("Data Source=library.db"));
var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", () => "Ahoj");
app.MapGet("/seed", Seed);

app.Run();

async Task<NoContent> Seed(LibraryContext context)
{
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();

    var capek = new Author() { Name = "Karel Čapek" };
    var nemcova = new Author() { Name = "Božena Němcová" };
    var orwel = new Author() { Name = "George Orwel" };

    var babicka = new Book() { Title = "Babička", Authors = [nemcova] };
    var rur = new Book() { Title = "R.U.R.", Authors = [capek] };
    var kniha1984 = new Book() { Title = "1984", Authors = [orwel] };

    var karel = new Reader() { Name = "Karel Čech" };
    var honza = new Reader() { Name = "Honza Svoboda" };
    var dominik = new Reader() { Name = "Dominik Veselý" };

    var loanKarelBabicka = new Loan() { LoanDate = new DateOnly(2026, 3, 6), Reader = karel, Book = babicka };
    var loanKarelRur = new Loan() { LoanDate = new DateOnly(2026, 3, 13), Reader = karel, Book = rur };
    var loanHonzaRur = new Loan() { LoanDate = new DateOnly(2026, 2, 25), Reader = honza, Book = rur };
    var loanDominik1984 = new Loan() { LoanDate = new DateOnly(2026, 2, 25), ReturnDate = new DateOnly(2026, 03, 10), Reader = dominik, Book = kniha1984 };

    context.Authors.AddRange(capek, nemcova, orwel);
    context.Books.AddRange(babicka, rur, kniha1984);
    context.Readers.AddRange(karel, honza, dominik);
    context.Loans.AddRange(loanKarelBabicka, loanHonzaRur, loanKarelRur, loanDominik1984);

    await context.SaveChangesAsync();

    return TypedResults.NoContent();
}

class LibraryContext(DbContextOptions<LibraryContext> options)
    : DbContext(options)
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Reader> Readers { get; set; }
}

class Author
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Book> Books { get; set; } = [];
}

class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public List<Author> Authors { get; set; } = [];
    public List<Loan> Loans { get; set; } = [];
}

class Loan
{
    public int Id { get; set; }
    public required DateOnly LoanDate { get; set; }
    public DateOnly? ReturnDate { get; set; }
    public int BookId { get; set; }
    public Book? Book { get; set; }
    public int ReaderId { get; set; }
    public Reader? Reader { get; set; }
}

class Reader
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Loan> Loans { get; set; } = [];
}
