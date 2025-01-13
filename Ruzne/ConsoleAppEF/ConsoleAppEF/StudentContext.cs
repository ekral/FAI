using Microsoft.EntityFrameworkCore;

namespace ConsoleAppEF
{
    public class StudentContext(DbContextOptions<StudentContext> options) : DbContext(options)
    {
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity?
            modelBuilder.Entity<Student>().HasData(
                new Student() { Id = 1, Jmeno = "Andrea", Prijmeni = "Nova" },
                new Student() { Id = 2, Jmeno = "Jiri", Prijmeni = "Novotny" },
                new Student() { Id = 3, Jmeno = "Karel", Prijmeni = "Vesely" }
            );
        }
    }

}
