using Microsoft.EntityFrameworkCore;

namespace MojeDesataAplikace
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
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                .EnableSensitiveDataLogging()
                .Options;

            return new StudentContext(options);
        }

        public static async Task Seed()
        {
            await using StudentContext context = CreateContext();

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Studenti.AddRangeAsync(
                new Student() { Jmeno = "Lumir", Studuje = true},
                new Student() { Jmeno = "Zlatica", Studuje = true},
                new Student() { Jmeno = "Karel", Studuje = false}
                );

            await context.SaveChangesAsync();
        }

        public static async Task AddStudent(Student student)
        {
            await using StudentContext context = CreateContext();

            context.Studenti.Add(student);

            Console.WriteLine($"pred save id: {student.StudentId}");
            await context.SaveChangesAsync();
            Console.WriteLine($"po save id: {student.StudentId}");

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

        public static async Task<Student?> GetStudentById(int id)
        {
            await using StudentContext context = CreateContext();

            Student? student = await context.Studenti.FindAsync(id);

            return student;
        }

        public static async Task UpdateStudent(int id, Student novy)
        {
            await using StudentContext context = CreateContext();

            Student? student = await context.Studenti.FindAsync(id);

            if(student is not null)
            {
                student.Jmeno = novy.Jmeno;
                student.Studuje = novy.Studuje;

                await context.SaveChangesAsync();
            }
        }

        public static async Task DeleteStudent(int id)
        {
            await using StudentContext context = CreateContext();

            Student? student = await context.Studenti.FindAsync(id);

            if (student is not null)
            {
                context.Studenti.Remove(student);

                await context.SaveChangesAsync();
            }
        }

        static async Task Main(string[] args)
        {
            await Seed();

            await AddStudent(new Student() { StudentId = 0, Jmeno = "Novotny", Studuje = true });

            await UpdateStudent(1, new Student() { Jmeno = "Power", Studuje = true });

            await DeleteStudent(3);

            List<Student> studenti = await GetAllStudents();

            Console.WriteLine("Vsichni studenti");

            foreach (Student student in studenti)
            {
                Console.WriteLine($"{student.StudentId} {student.Jmeno} {student.Studuje}");
            }

            List<Student> studujici = await GetActiveStudents();

            Console.WriteLine("Studujici studenti");

            foreach (Student student in studujici)
            {
                Console.WriteLine($"{student.StudentId} {student.Jmeno} {student.Studuje}");
            }

            Student? studentById = await GetStudentById(1);
            
            if (studentById is not null)
            {
                Console.WriteLine("Student by id");
                Console.WriteLine($"{studentById.StudentId} {studentById.Jmeno} {studentById.Studuje}");
            }
        }
    }
}
