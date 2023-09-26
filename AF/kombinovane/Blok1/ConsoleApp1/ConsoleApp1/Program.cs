using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

using StudentContext context = new StudentContext();

var skupiny = context.Skupiny.Take(1);

foreach(Skupina skupina in skupiny.Include(s => s.Studenti))
{
    Console.WriteLine($"skupina: {skupina.Id} {skupina.Nazev} {skupina.Studenti?.Count}");

    if (skupina.Studenti is not null)
    {
        foreach (Student student in skupina.Studenti)
        {
            Console.WriteLine(value: $"student: {student.Id} {student.Jmeno} {student.SkupinaId}");
        }
    }
}
//var studenti = context.Students.OrderByDescending(s => s.Jmeno).Include(s => s.Skupina); // eager loading
var studenti = context.Students.OrderByDescending(s => s.Jmeno);

foreach(Student student in studenti)
{
    if(student.Id == 2)
    {
        context.Entry(student).Reference(s => s.Skupina).Load(); // Explicit loading
    }

    Console.WriteLine(value: $"{student.Id} {student.Jmeno} {student.SkupinaId} {student.Skupina?.Nazev ?? "neni nactena"}");
}

class StudentContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Skupina> Skupiny { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var folder = Environment.SpecialFolder.MyDocuments;
        string folderPath = Environment.GetFolderPath(folder);
        string filePath = Path.Join(folderPath, "studenti3.db");

        SqliteConnectionStringBuilder csb = new SqliteConnectionStringBuilder
        {
            DataSource = filePath
        };

        optionsBuilder
            .UseSqlite(csb.ConnectionString)
            .LogTo(log => Debug.WriteLine(log));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().HasKey(s => s.Id);
        modelBuilder.Entity<Student>().HasOne(s => s.Skupina);

        modelBuilder.Entity<Skupina>().HasData(
            new Skupina() { Id = 1, Nazev = "swi1"},
            new Skupina() { Id = 2, Nazev = "swi2"}
            );

        modelBuilder.Entity<Student>().HasData(
            new Student() { Id = 1, Jmeno = "Andrea", Prijmeni = "Nova", SkupinaId = 1 },
            new Student() { Id = 2, Jmeno = "Jiri", Prijmeni = "Novotny", SkupinaId = 2 },
            new Student() { Id = 3, Jmeno = "Karel", Prijmeni = "Vesely", SkupinaId = 2 }
        );
    }
}

class Student
{
    public int Id { get; set; } 
    public required string Jmeno { get; set; }
    public required string Prijmeni { get; set; }

    public int SkupinaId { get; set; } // cizi klic je pro databazi

    public Skupina? Skupina { get; set; }   // navigacni properta je pro prochazeni v kodu
}

class Skupina
{
    public int Id { get; set; }
    public required string Nazev { get; set; }
    public ICollection<Student>? Studenti { get; set; } // navigation property, slouzi jen pro prochazeni v kodu
}
