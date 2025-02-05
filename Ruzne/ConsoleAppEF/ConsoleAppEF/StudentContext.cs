using Microsoft.EntityFrameworkCore;

namespace ConsoleAppEF
{
    public class StudentContext(DbContextOptions<StudentContext> options) : DbContext(options)
    {
        public DbSet<Student> Studenti { get; set; }
        public DbSet<Skupina> Skupiny { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasOne(s => s.Skupina).WithMany(sk => sk.Studenti).HasForeignKey(s => s.SkupinaId);
        }
    }

}
