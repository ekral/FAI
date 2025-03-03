using Microsoft.EntityFrameworkCore;

namespace MujDvanactyProjekt
{
    // 1 skupina : n studentu

    class Student
    {
        public int StudentId { get; set; }
        public required string Jmeno { get; set; }
        public int SkupinaId { get; set; } // Cizi klic
        public Skupina? Skupina { get; set; } // Navigation property
        public Karta? Karta { get; set; }
        public List<Predmet> Predmety { get; set; } = [];
    }

    class Predmet
    {
        public int PredmetId { get; set; }
        public required string Nazev { get; set; }
        public List<Student> Studenti { get; set; } = [];
    }

    class PredmetStudent
    {
        public int StudentId { get; set; }
        public int PredmetId { get; set; }
    }

    class Karta
    {
        public int KartaId { get; set; }
        public decimal Zustatek { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
    }

    class Skupina
    {
        public int SkupinaId { get; set; }
        public required string Nazev { get; set; }
        public List<Student> Studenti { get; set; } = []; // Collection Navigation property
    }

    class SkolaContext(DbContextOptions<SkolaContext> options) : DbContext(options)
    {
        public DbSet<Student> Studenti { get; set; }
        public DbSet<Skupina> Skupiny { get; set; }
        public DbSet<Karta> Karty { get; set; }
        public DbSet<Predmet> Predmety { get; set; }
        public DbSet<PredmetStudent> PredmetyStudenta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasMany(st => st.Predmety)
                .WithMany(pr => pr.Studenti)
                .UsingEntity<PredmetStudent>();
        }
    }

    internal class Program
    {
        public static SkolaContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<SkolaContext>()
                .UseSqlite("DataSource=skola.db")
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                .EnableSensitiveDataLogging(true)
                .Options;

            return new SkolaContext(options);
        }

        public static async Task Seed()
        {
            await using SkolaContext context = CreateContext();

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            Skupina skupina1 = new() { SkupinaId = 1, Nazev = "SWI1" };
            Skupina skupina2 = new() { SkupinaId = 2, Nazev = "SWI2" };

            Student student1 = new() { StudentId = 1, Jmeno = "Martin", SkupinaId = 1 };
            Student student2 = new() { StudentId = 2, Jmeno = "Alena", SkupinaId = 2 };
            Student student3 = new() { StudentId = 3, Jmeno = "Petr", SkupinaId = 2 };

            Karta karta1 = new() { KartaId = 1, Zustatek = 200.0m, StudentId = 1 };
            Karta karta2 = new() { KartaId = 2, Zustatek = 600.0m, StudentId = 2 };
            Karta karta3 = new() { KartaId = 3, Zustatek = 400.0m, StudentId = 3 };

            Predmet predmet1 = new() { PredmetId = 1, Nazev = "Elektro"};
            Predmet predmet2 = new() { PredmetId = 2, Nazev = "Tenis"};

            PredmetStudent ps1 = new() { PredmetId = 1, StudentId = 1 };
            PredmetStudent ps2 = new() { PredmetId = 2, StudentId = 1 };
            PredmetStudent ps3 = new() { PredmetId = 2, StudentId = 2 };

            await context.Skupiny.AddRangeAsync(skupina1, skupina2);
            await context.Studenti.AddRangeAsync(student1, student2, student3);
            await context.Karty.AddRangeAsync(karta1, karta2, karta3);
            await context.Predmety.AddRangeAsync(predmet1, predmet2);
            await context.PredmetyStudenta.AddRangeAsync(ps1, ps2, ps3);

            await context.SaveChangesAsync();
        }

        public static async Task VypisSkupinyEager()
        {
            await using SkolaContext context = CreateContext();

            Console.WriteLine("Skupiny:");

            foreach (Skupina skupina in await context.Skupiny
                .Include(sk => sk.Studenti)
                .ThenInclude(st => st.Karta)
                .ToListAsync())
            {
                Console.WriteLine($"{skupina.SkupinaId} {skupina.Nazev}");

                foreach (Student student in skupina.Studenti)
                {
                    Console.WriteLine($"  {student.Jmeno}");

                    if(student.Karta is not null)
                    {
                        Console.WriteLine($"    zustatek: {student.Karta.Zustatek}");
                    }
                }
            }
        }

        public static async Task VypisSkupinyExplicit()
        {
            await using SkolaContext context = CreateContext();

            Console.WriteLine("Skupiny Explicit:");

            foreach (Skupina skupina in await context.Skupiny.ToListAsync())
            {
                Console.WriteLine($"{skupina.SkupinaId} {skupina.Nazev}");

                await context.Entry(skupina).Collection(sk => sk.Studenti).LoadAsync();

                foreach (Student student in skupina.Studenti)
                {
                    Console.WriteLine($"  {student.Jmeno}");

                    await context.Entry(student).Reference(st => st.Karta).LoadAsync();

                    if (student.Karta is not null)
                    {
                        Console.WriteLine($"    zustatek: {student.Karta.Zustatek}");
                    }
                }
            }
        }

        static async Task Main(string[] args)
        {
            await Seed();
            await VypisSkupinyEager();
            await VypisSkupinyExplicit();
        }
    }
}
