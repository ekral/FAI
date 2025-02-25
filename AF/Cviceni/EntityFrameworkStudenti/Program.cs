using Microsoft.EntityFrameworkCore;

namespace MojeDevataAplikace
{
    class Student
    {
        public int StudentId { get; set; }
        public required string Jmeno { get; set; }
        public required bool Studuje { get; set; }
    }

    class StudentContext(DbContextOptions<StudentContext> options) : DbContext(options)
    {
        public DbSet<Student> Studenti { get; set; }
    }


    internal class Program
    {
        public static StudentContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<StudentContext>()
                .UseSqlite("DataSource=studenti.db")
                //.EnableSensitiveDataLogging(true)
                //.LogTo(Console.WriteLine)
                .Options;

            return new StudentContext(options);
        }

        public static async Task Seed()
        {
            await using StudentContext context = CreateContext();
            
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Studenti.AddRangeAsync(
                new Student() { Jmeno = "Matej", Studuje = true},
                new Student() { Jmeno = "Jiri", Studuje = true},
                new Student() { Jmeno = "Alena", Studuje = false}
                );

            await context.SaveChangesAsync();
        }

        public static async Task AddStudent(Student student)
        {
            await using StudentContext context = CreateContext();

            await context.AddAsync(student);
            
            Console.WriteLine($"StudentId: {student.StudentId}");

            await context.SaveChangesAsync();

            Console.WriteLine($"StudentId: {student.StudentId}");

        }

        public static async Task UpdateStudent(int id, Student input)
        {
            await using StudentContext context = CreateContext();

            Student? student = await context.Studenti.FindAsync(id);

            if(student is not null)
            {
                student.Jmeno = input.Jmeno;
                student.Studuje = input.Studuje;

                context.Update(student);

                await context.SaveChangesAsync();
            }
        }

        public static async Task DeleteStudent(int id)
        {
            await using StudentContext context = CreateContext();

            Student? student = await context.Studenti.FindAsync(id);

            if (student is not null)
            {
                context.Remove(student);

                await context.SaveChangesAsync();
            }
        }

        public static async Task<List<Student>> GetAllStudents()
        {
            await using StudentContext context = CreateContext();

            return await context.Studenti.OrderBy(s => s.Jmeno).ToListAsync();
        }

        public static async Task<List<Student>> GetActiveStudents()
        {
            await using StudentContext context = CreateContext();

            return await context.Studenti.Where(s => s.Studuje).ToListAsync();
        }

        // Ukol: S pomoci EF vytvorte databazi s tabulkou Produkty, ktera budem mit
        // ProduktId
        // Nazev
        // Cena decimal
        // Vytvorte Seed, AddProdukt a GetAllProducts

        static async Task Main(string[] args)
        {
            await Seed();
            Student novy = new() { Jmeno = "Novy", Studuje = true };

            await AddStudent(novy);

            await UpdateStudent(2, new Student() { Jmeno = "Jine", Studuje = false });

            await DeleteStudent(3);

            List<Student> studenti = await GetAllStudents();

            Console.WriteLine("Vsichni studenti:");

            foreach(Student student in studenti)
            {
                Console.WriteLine($"{student.StudentId} {student.Jmeno} {student.Studuje}");
            }

            List<Student> studujici = await GetActiveStudents();

            Console.WriteLine("Studujici studenti:");

            foreach (Student student in studujici)
            {
                Console.WriteLine($"{student.StudentId} {student.Jmeno} {student.Studuje}");
            }
        }
    }
}
