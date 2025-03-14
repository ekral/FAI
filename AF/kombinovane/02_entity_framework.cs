using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Threading.Tasks;

namespace ConsoleAppPrednaska2
{
    class Student
    {
        public int StudentId { get; set; }
        public required string Jmeno { get; set; }
        public required int Kredity { get; set; }
        public required int SkupinaId { get; set; } // Cizi klic
        public Skupina? Skupina { get; set; } // Navigation Property
        public Karta? Karta { get; set; } // Navigation Property
        public List<Predmet> Predmety { get; set; } = []; // Collection Navigation Property
    }

    class Predmet
    {
        public int PredmetId { get; set; }
        public required string Nazev { get; set; }
        public List<Student> Studenti { get; set; } = []; // Collection Navigation Property
    }

    class PredmetStudent
    {
        public int PredmetId { get; set; }
        public int StudentId { get; set; }
    }

    class Karta
    {
        public int KartaId { get; set; }
        public required decimal Zustatek { get; set; }
        public required int StudentId { get; set; }
        public Student? Student { get; set; }
    }

    class Skupina
    {
        public int SkupinaId { get; set; }
        public required string Nazev { get; set; }
        public List<Student> Studenti { get; set; } = []; // Collection Navigation Property

    }

    class StudentContext(DbContextOptions<StudentContext> options) : DbContext(options)
    {
        public DbSet<Student> Studenti { get; set; }
        public DbSet<Skupina> Skupiny { get; set; }
        public DbSet<Karta> Karty { get; set; }
        public DbSet<Predmet> Predmety { get; set; }
        public DbSet<PredmetStudent> PredmetyStudenta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Predmety)
                .WithMany(p => p.Studenti)
                .UsingEntity<PredmetStudent>();
        }
    }

    internal class Program
    {
        public static StudentContext CreateStudentContext()
        {
            var options = new DbContextOptionsBuilder<StudentContext>()
                .UseSqlite("DataSource=studentief.db")
                //.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                .EnableSensitiveDataLogging(true)
                .Options;

            return new StudentContext(options);
        }
        public static async Task Seed()
        {
            await using StudentContext context = CreateStudentContext();

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            Skupina skupina1 = new() { SkupinaId = 1, Nazev = "KSWI1" };
            Skupina skupina2 = new() { SkupinaId = 2, Nazev = "KSWI2" };


            Student student1 = new() { StudentId = 1, Jmeno = "Matylda", Kredity = 12, SkupinaId = 1 };
            Student student2 = new() { StudentId = 2, Jmeno = "Petr", Kredity = 15, SkupinaId = 1 };
            Student student3 = new() { StudentId = 3, Jmeno = "Jiri", Kredity = 16, SkupinaId = 2 };

            Predmet predmet1 = new() { PredmetId = 1, Nazev = "Pocitacove Site"};
            Predmet predmet2 = new() { PredmetId = 2, Nazev = "Testovani SW" };

            PredmetStudent ps1 = new() { PredmetId = 1, StudentId = 1 };
            PredmetStudent ps2 = new() { PredmetId = 2, StudentId = 2 };
            PredmetStudent ps3 = new() { PredmetId = 2, StudentId = 3 };

            Karta k1 = new() { KartaId = 1, Zustatek = 400.0m, StudentId = 1 };
            Karta k2 = new() { KartaId = 2, Zustatek = 200.0m, StudentId = 2 };
            Karta k3 = new() { KartaId = 3, Zustatek = 100.0m, StudentId = 3 };

            await context.Skupiny.AddRangeAsync(skupina1, skupina2);
            await context.Studenti.AddRangeAsync(student1, student2, student3);
            await context.Karty.AddRangeAsync(k1, k2, k3);
            await context.Predmety.AddRangeAsync(predmet1, predmet2);
            await context.PredmetyStudenta.AddRangeAsync(ps1, ps2, ps3);

            await context.SaveChangesAsync();
        }

        public static async Task InsertStudent(string jmeno, int kredity, int skupinaId)
        {
            await using StudentContext context = CreateStudentContext();

            Student novy = new() { StudentId = 0, Jmeno = jmeno, Kredity = kredity, SkupinaId = skupinaId };

            await context.Studenti.AddAsync(novy);

            Console.WriteLine($"student id: {novy.StudentId}");
            await context.SaveChangesAsync();
            Console.WriteLine($"student id: {novy.StudentId}");
        }

        public static async Task VypisStudenty()
        {
            await using StudentContext context = CreateStudentContext();

            foreach (Student student in await context.Studenti.ToListAsync())
            {
                Console.WriteLine($"{student.StudentId} {student.Jmeno} {student.Kredity}");
            }
        }

        public static async Task VypisSkupinyEager()
        {
            await using StudentContext context = CreateStudentContext();

            foreach (Skupina skupina in await context.Skupiny
                .Include(sk => sk.Studenti)
                .ThenInclude(s => s.Karta)
                .ToListAsync())
            {
                Console.WriteLine($"{skupina.SkupinaId} {skupina.Nazev} pocet studentu: {skupina.Studenti.Count}");

                foreach (Student student in skupina.Studenti)
                {
                    Console.WriteLine($"  {student.StudentId} {student.Jmeno} {student.Kredity} {student.Karta?.Zustatek}");
                }
            }
        }

        public static async Task VypisSkupinyExplicit()
        {
            await using StudentContext context = CreateStudentContext();

            foreach (Skupina skupina in await context.Skupiny.ToListAsync())
            {
                await context.Entry(skupina).Collection(sk => sk.Studenti).LoadAsync();

                Console.WriteLine($"{skupina.SkupinaId} {skupina.Nazev} pocet studentu: {skupina.Studenti.Count}");

                foreach (Student student in skupina.Studenti)
                {
                    Console.WriteLine($"  {student.StudentId} {student.Jmeno} {student.Kredity}");

                    await context.Entry(student).Collection(st => st.Predmety).LoadAsync();

                    foreach(Predmet predmet in student.Predmety)
                    {
                        Console.WriteLine($"    {predmet.Nazev}");
                    }
                }
            }
        }

        static async Task Main(string[] args)
        {
            await Seed();
            await InsertStudent("Novotny", 20, 2);
            await VypisStudenty();
            await VypisSkupinyEager();
            await VypisSkupinyExplicit();
        }
    }
}
