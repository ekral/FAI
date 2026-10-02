using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var options = new DbContextOptionsBuilder<LibraryContext>()
                    .UseSqlite("Data Source=library.db")
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging(true)
                    .Options;

using (var context = new LibraryContext(options))
{
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();

    Author capek = new Author() { Name = "Karel Čapek" };
    Author nemcova = new Author() { Name = "Božena Němcová" };
    Author orwel = new Author() { Name = "George Orwel" };
    Author pratchett = new Author() { Name = "Terry Pratchett" };
    Author gaiman = new Author() { Name = "Neil Gaiman" };

    Book babicka = new Book() { Title = "Babička", Authors = [nemcova] };
    Book rur = new Book() { Title = "R.U.R.", Authors = [capek] };
    Book kniha1984 = new Book() { Title = "1984", Authors = [orwel] };
    Book znameni = new Book() { Title = "Dobrá znamení", Authors = [pratchett, gaiman] };

    Reader karel = new Reader() { Name = "Karel Čech" };
    Reader honza = new Reader() { Name = "Honza Svoboda" };
    Reader tereza = new Reader() { Name = "Tereza Nová" };

    Loan loanKarelBabicka = new Loan() { Reader = karel, Book = babicka, LoanDate = new DateOnly(2026, 3, 6),  };
    Loan loanHonzaRur = new Loan() { Reader = honza, Book = rur, LoanDate = new DateOnly(2026, 2, 25), ReturnDate = new DateOnly(2026, 3, 16) };

    context.Authors.AddRange(capek, nemcova, orwel, pratchett, gaiman);
    context.Books.AddRange(babicka, rur, kniha1984, znameni);
    context.Readers.AddRange(karel, honza, tereza);
    context.Loans.AddRange(loanKarelBabicka, loanHonzaRur);

    await context.SaveChangesAsync();
}

using (var context = new LibraryContext(options))
{

    Console.WriteLine("Knihy a jejich autori:");

    var books = context.Books.Include(b => b.Authors);

    foreach (var book in books)
    {
        Console.WriteLine($"{book.Title} pocet autoru: {book.Authors.Count}");

        foreach (var authors in book.Authors)
        {
            Console.WriteLine($"Autor knihy: {authors.Name}");
        }
    }

    Console.WriteLine("Ctenari s aspon jednou vypujckou:");

    var readers = context.Readers.Where(s => s.Loans.Any());
    
    foreach (var reader in readers)
    {
        Console.WriteLine($"{reader.Name}");
    }

    Console.WriteLine("Najděte a vypište čtenáře aspoň s jednou aktivní výpůjčkou:");

    var readersWithActiveLoans = context.Readers
        .Where(s => s.Loans.Any(l => l.ReturnDate == null)); 

    foreach (var reader in readersWithActiveLoans)
    {
        Console.WriteLine($"{reader.Name}");
    }

    //var readersWithActiveLoansGroup = context.Loans
    //.Where(l => l.ReturnDate == null)
    //.GroupBy(l => l.Reader)
    //.Select(g => g.Key);

    //foreach (var reader in readersWithActiveLoansGroup)
    //{
    //    Console.WriteLine($"{reader.Name}");
    //}

    Console.WriteLine("Vypište knihy, které nikdy nebyly půjčeny");

    var neverLoanedBooks = context.Books.Where(s => !s.Loans.Any());

    foreach (var book in neverLoanedBooks)
    {
        Console.WriteLine($"{book.Title}");
    }

    Console.WriteLine("Vypište názvy všech knih vypůjčených (vrácených i nevrácených) konkrétním čtenářem");

    IQueryable<string> bookTitles = context.Books
                            .Where(b => b.Loans.Any(l => l.ReaderId == 1))
                            .Select(b => b.Title);
    // Chceme jen nazvy

    foreach (string title in bookTitles)
    {
        Console.WriteLine(title);
    }
}

Console.WriteLine("Konec programu!");

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
