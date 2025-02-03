using Microsoft.EntityFrameworkCore;

namespace ConsoleAppEF
{
    public class StudentContext(DbContextOptions<StudentContext> options) : DbContext(options)
    {
        public DbSet<Student> Studenti { get; set; }
        public DbSet<Skupina> Skupiny { get; set; }

        public DbSet< MyProperty { get; set; }
    }

}
