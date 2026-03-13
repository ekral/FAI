using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<LibraryContext>()
                    .UseSqlite("Data Source=library.db")
                    .Options;

using var context = new LibraryContext(options);

await context.Database.EnsureDeletedAsync();
await context.Database.EnsureCreatedAsync();

Author capek = new Author() { Name = "Karel Čapek" };
Author nemcova = new Author() { Name = "Božena Němcová" };
Author orwel = new Author() { Name = "George Orwel" };

Book babicka = new Book() { Title = "Babička", Authors = [nemcova] };
Book rur = new Book() { Title = "R.U.R.", Authors = [capek] };
Book kniha1984 = new Book() { Title = "1984", Authors = [orwel] };

Reader karel = new Reader() { Name = "Karel Čech" };
Reader honza = new Reader() { Name = "Honza Svoboda" };
Reader dominik = new Reader() { Name = "Dominik Veselý" };

Loan loanKarelBabicka = new Loan() { LoanDate = new DateOnly(2026, 3, 6), Reader = karel, Book = babicka };
Loan loanKarelRur = new Loan() { LoanDate = new DateOnly(2026, 3, 13), Reader = karel, Book = rur };
Loan loanHonzaRur = new Loan() { LoanDate = new DateOnly(2026, 2, 25), Reader = honza, Book = rur };
Loan loanDominik1984 = new Loan() { LoanDate = new DateOnly(2026, 2, 25),ReturnDate  =new DateOnly(2026,03,10), Reader = dominik, Book = kniha1984 };

context.Authors.AddRange(capek, nemcova, orwel);
context.Books.AddRange(babicka, rur, kniha1984);
context.Readers.AddRange(karel, honza, dominik);
context.Loans.AddRange(loanKarelBabicka, loanHonzaRur, loanKarelRur, loanDominik1984);

await context.SaveChangesAsync();

Console.WriteLine("Knihy a jejich autori");

var books = context.Books.Include(b => b.Authors);

foreach (var book in books)
{
    Console.WriteLine($"Kniha - {book.Title}");
    foreach (var author in book.Authors)
    {
        Console.WriteLine($"  Autor - {author.Name}");
    }
}

var nevypujcene = context.Books.Where(b => !b.Loans.Any());

Console.WriteLine("Nevypůjčené knihy");

foreach (var book in nevypujcene)
{
    Console.WriteLine($"Kniha - {book.Title}");
}

Console.WriteLine("Knihy vypůjčeny Karlem:");

var karelsBooks = context.Books.Where(r => r.Loans.Any(l => l.Reader!.Name == "Karel Čech"));

foreach (var book in karelsBooks)
{
    Console.WriteLine($"- {book.Title}");
}

Console.WriteLine("Čtenáři alespoň s jednou výpujčkou");
var readers1 = context.Readers.Where(r => r.Loans.Any());

foreach (var reader in readers1)
{
    Console.WriteLine(reader.Name );
}

Console.WriteLine("Čtenáři s nevrácenou výpujčkou");
var activeReaders = context.Readers.Where(r => r.Loans.Any(l=>l.ReturnDate == null));

foreach (var reader in activeReaders)
{
    Console.WriteLine(reader.Name);
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