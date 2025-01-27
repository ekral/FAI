using Microsoft.EntityFrameworkCore;

namespace ConsoleAppEF
{
    public class StudentContext(DbContextOptions<StudentContext> options) : DbContext(options)
    {
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student() { StudentId = 1, Jmeno = "Andrea", Prijmeni = "Nova" },
                new Student() { StudentId = 2, Jmeno = "Jiri", Prijmeni = "Novotny" },
                new Student() { StudentId = 3, Jmeno = "Karel", Prijmeni = "Vesely" }
            );
        }
    }

}
