using UTB.Library.Db;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<LibraryContext>("database");

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapPost("/reset-db", async (LibraryContext context) =>
{
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();

    Author a1 = new() { Name = "Karel Capek" };
    Author a2 = new() { Name = "Jaroslav Hasek" };
    Author a3 = new() { Name = "Bohumil Hrabal" };

    Book b1 = new() { Name = "R.U.R.", Authors = [ a1 ] };
    Book b2 = new() { Name = "Svejk", Authors = [ a2 ] };
    Book b3 = new() { Name = "Postriziny", Authors = [ a3 ] };

    Member m1 = new() { Name = "Jiri" };
    Member m2 = new() { Name = "Alena" };
    Member m3 = new() { Name = "Karel" };

    Loan l1 = new() { Book = b1, Member = m1, Date = DateOnly.FromDateTime(DateTime.Now), LoanStatus = LoanStatus.Borrowed };
    Loan l2 = new() { Book = b2, Member = m2, Date = DateOnly.FromDateTime(DateTime.Now), LoanStatus = LoanStatus.Borrowed };
    Loan l3 = new() { Book = b3, Member = m3, Date = DateOnly.FromDateTime(DateTime.Now), LoanStatus = LoanStatus.Borrowed };

    context.Authors.AddRange(a1, a2, a3);
    context.Books.AddRange(b1, b2, b3);
    context.Members.AddRange(m1, m2, m3);
    context.Loans.AddRange(l1, l2, l3);

    await context.SaveChangesAsync();
});

app.UseHttpsRedirection();

app.Run();
