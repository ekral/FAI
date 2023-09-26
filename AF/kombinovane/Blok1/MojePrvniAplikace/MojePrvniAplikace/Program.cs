using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;


using PizzerieKontext pizzerieKontext = new PizzerieKontext();

//Pizza nova = new Pizza()
//{
//    Name = "Margharita",
//    Price = 110.0m
//};

//pizzerieKontext.Pizza.Add(nova);
//pizzerieKontext.SaveChanges();

//Console.WriteLine(nova.Id);

var levne = pizzerieKontext.Pizza.Where(p => p.Price < 160);

foreach (Pizza pizza in levne)
{
    Console.WriteLine($"{pizza.Name} cena: {pizza.Price}");
}

class Pizza
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
}

class PizzerieKontext : DbContext
{
    public DbSet<Pizza> Pizza { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var folder = Environment.SpecialFolder.MyDocuments;
        string folderPath = Environment.GetFolderPath(folder);
        string filePath = System.IO.Path.Join(folderPath, "pizzeria.db");
        SqliteConnectionStringBuilder csb = new SqliteConnectionStringBuilder
        {
            DataSource = filePath
        };

        optionsBuilder
            .UseSqlite(csb.ConnectionString)
            .LogTo(log => System.Diagnostics.Debug.WriteLine(log));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Pizza pizza1 = new Pizza()
        {
            Id = 1,
            Name = "Hawaii",
            Price = 150.0m
        };

        Pizza pizza2 = new Pizza()
        {
            Id = 2,
            Name = "Milano",
            Price = 175.0m
        };

        modelBuilder.Entity<Pizza>().HasData(pizza1, pizza2);
    }
}