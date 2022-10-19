// See https://aka.ms/new-console-template for more information
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

using ModelContext db = new ModelContext();
db.Database.EnsureCreated(); // nahradime dotnet ef migrations

var vybrane = db.Models.Where(m => m.LoanAmount > 5000000.0).OrderByDescending(m => m.LoanTerm);

foreach (Model model in vybrane)
{
    Console.WriteLine($"{model.Id, 3} {model.LoanAmount, 16:C1} {model.InterestRate, 3:F1} {model.LoanTerm, 3}");
}

Model novy = new Model() { LoanAmount = 5000000.0, InterestRate = 6.0, LoanTerm = 15 };
db.Models.Add(novy);

db.SaveChanges();

Console.WriteLine($"Prirazene Id: { novy.Id}");

foreach (Model m in db.Models)
{
    Console.WriteLine($"{m.Id,3} {m.LoanAmount,16:C1} {m.InterestRate,3:F1} {m.LoanTerm,3}");
}

class ModelContext : DbContext
{
    public DbSet<Model> Models { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string databasePath = Path.Join(path, "database.db");

        SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder();
        connectionStringBuilder.DataSource = databasePath;

        optionsBuilder
            .UseSqlite(connectionStringBuilder.ConnectionString)
            .LogTo(log => Debug.WriteLine(log));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Model>().HasKey(x => x.Id);
        modelBuilder.Entity<Model>().Property(x => x.LoanTerm).HasColumnType("real");

        modelBuilder.Entity<Model>().HasData(
            new Model() { Id = 1, LoanAmount = 8000000.0, InterestRate=6.0, LoanTerm = 30 },
            new Model() { Id = 2, LoanAmount = 4000000.0, InterestRate=5.0, LoanTerm = 20 },
            new Model() { Id = 3, LoanAmount = 108000000.0, InterestRate=5.7, LoanTerm = 25 }
        );
    }

}
class Model
{
    public int Id { get; set; }
    public double LoanAmount { get; set; }
    public double InterestRate { get; set; }
    public int LoanTerm { get; set; }
}
