// TODO AddDbContext in services
// https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
// https://learn.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key
// https://learn.microsoft.com/en-us/ef/core/querying/related-data/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

using SkolaContext db = new();
db.Database.EnsureCreated();
// eager
foreach (Skupina skupina in db.Skupiny.Include(skupina => skupina.Studenti))
{
    Console.WriteLine($"{skupina.Id, 3} {skupina.Nazev, 5} {skupina?.Studenti.Count ?? 0}");

    if (skupina is not null)
    {
        foreach (Student student in skupina.Studenti)
        {
            Console.WriteLine($"- {student.Id, 3} {student.Jmeno, 7} {student.SkupinaId, 3}");
        }
    }
    
}

foreach (Skupina skupina in db.Skupiny)
{
    // explicit
    db.Skupiny.Entry(skupina).Collection(skupina => skupina.Studenti).Load();

    Console.WriteLine($"{skupina.Id,3} {skupina.Nazev,5} {skupina?.Studenti.Count ?? 0}");

    if (skupina is not null)
    {
        foreach (Student student in skupina.Studenti)
        {
            Console.WriteLine($"- {student.Id,3} {student.Jmeno,7} {student.SkupinaId,3}");
        }
    }

}

class SkolaContext : DbContext
{
    public DbSet<Student> Studenti { get; set; } = null!;
    public DbSet<Skupina> Skupiny { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlite("Data Source=database.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().HasOne(skupina => skupina.Skupina).WithMany(skupina => skupina.Studenti).HasForeignKey(student => student.SkupinaId);

        modelBuilder.Entity<Skupina>().HasData(new Skupina() { Id = 1, Nazev = "swi1" });
        modelBuilder.Entity<Student>().HasData(
            new Student() { Id = 1, Jmeno = "Otmar", SkupinaId = 1 },
            new Student() { Id = 2, Jmeno = "Agnesa", SkupinaId = 1 });
    }
}

class Student
{
    public int Id { get; set; }
    public string Jmeno { get; set; }
    public int SkupinaId { get; set; }
    public Skupina Skupina { get; set; } // navigation property
}

class Skupina
{
    public int Id { get; set; }
    public string Nazev { get; set; }
    public List<Student> Studenti { get; set; } // navigation property
}
