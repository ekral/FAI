using Microsoft.EntityFrameworkCore;

namespace ConsoleAppEFManyToMany
{
    class Student
    {
        public int StudentId { get; set; }
        public required string Jmeno { get; set; }
        public List<Subject>? Subjects { get; set; }
    }

    class Subject
    {
        public int SubjectId { get; set; }
        public required string Name { get; set; }

        public List<Student>? Students { get; set; }
    }

    class StudentSubject
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
    }

    class StudentContext : DbContext
    {
        public DbSet<Student> Studenti { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        public StudentContext(DbContextOptions<StudentContext> options) : base(options)
        {
            
        }
    }


    internal class Program
    {
        static StudentContext CreateContext()
        {
            StudentContext context = new(new DbContextOptionsBuilder<StudentContext>()
                                                    .UseSqlite("Data Source=studenti.db")
                                                    .Options);
            return context;
        }

        static void Main(string[] args)
        {
            using StudentContext context = CreateContext();

            if (context.Database.EnsureCreated())
            {
                Student student = new Student() { StudentId = 1, Jmeno = "Karl" };
                Subject subject = new Subject() { SubjectId = 1, Name = "Math" };

                context.Add(student);
                context.Add(subject);

                student.Subjects = [subject];
                subject.Students = [student];

                int count = context.SaveChanges();
            }
        }
    }
}
