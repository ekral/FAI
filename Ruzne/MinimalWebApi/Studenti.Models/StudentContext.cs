using Microsoft.EntityFrameworkCore;

namespace Studenti.Models
{
    public class StudentContext : DbContext
    {
        public DbSet<Student> Studenti { get; set; }

        public StudentContext(DbContextOptions<StudentContext> options) : base(options)
        {

        }
    }

}
