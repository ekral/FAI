using Microsoft.EntityFrameworkCore;

namespace ConsoleAppEF2
{
    public class Student
    {
        public int StudentId { get; set; }
        public required string Name { get; set; } 
        public StudentCart? StudentCart { get; set; } // Navigation Property to dependent

    }
    public class StudentCart
    {
        public required int StudentId { get; set; } // Cizí klíč s unique indexem
        public required DateTime PlatnostDo { get; set; }
        public Student? Student { get; set; }
    }

    class StudentContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCart> Carts { get; set; }

        public StudentContext(DbContextOptions<StudentContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasOne(s => s.StudentCart)
                .WithOne(sc => sc.Student)
                .HasForeignKey<StudentCart>(sc => sc.StudentId)
                .IsRequired();

            //modelBuilder.Entity<StudentCart>().HasIndex(sc => sc.StudentId).IsUnique();
            modelBuilder.Entity<StudentCart>().HasKey(sc => sc.StudentId);
        }


    }
    internal class Program
    {
        static void Main(string[] args)
        {
            using StudentContext context = new StudentContext(new DbContextOptionsBuilder<StudentContext>()
                                        .UseSqlite("Data Source=studenti2.db")
                                        .LogTo(Console.WriteLine)
                                        .Options);

            if(context.Database.EnsureCreated())
            {
                context.Add(new Student() { StudentId = 1, Name = "Jiri" });
                context.Add(new Student() { StudentId = 2, Name = "Karel" });
                context.Add(new StudentCart() { PlatnostDo = DateTime.Now, StudentId = 1 });
                context.Add(new StudentCart() { PlatnostDo = DateTime.Now, StudentId = 2 });

                context.SaveChanges();

            }
        }
    }
}
