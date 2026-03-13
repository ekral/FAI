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

Loan loanKarelBabicka = new Loan() { LoanDate = new DateOnly(2026, 3, 6), Reader = karel, Book = babicka };
Loan loanHonzaRur = new Loan() { LoanDate = new DateOnly(2026, 2, 25), Reader = honza, Book = rur };

context.Authors.AddRange(capek, nemcova, orwel);
context.Books.AddRange(babicka, rur, kniha1984);
context.Readers.AddRange(karel, honza);
context.Loans.AddRange(loanKarelBabicka, loanHonzaRur);

await context.SaveChangesAsync();

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
