using Microsoft.EntityFrameworkCore;
using Students.WebApi.Models;

namespace Students.WebApi.Data
{
    public class StudentContext(DbContextOptions<StudentContext> options) 
        : DbContext(options)
    {
        public DbSet<Student> Studenti { get; set; }
    }
}
