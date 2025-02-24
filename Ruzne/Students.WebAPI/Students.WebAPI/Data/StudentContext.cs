using Microsoft.EntityFrameworkCore;
using Students.WebAPI.Models;

namespace Students.WebAPI.Data
{
    public class StudentContext(DbContextOptions<StudentContext> options) : DbContext(options)
    {
        public DbSet<Student> Studenti { get; set; }
    }
}
