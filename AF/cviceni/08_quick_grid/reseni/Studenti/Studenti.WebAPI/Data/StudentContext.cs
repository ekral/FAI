using Microsoft.EntityFrameworkCore;
using Studenti.WebAPI.Models;

namespace Studenti.WebAPI.Data
{
    public class StudentContext(DbContextOptions<StudentContext> options) : DbContext(options)
    {
        public DbSet<Student> Studenti { get; set; }
    }
}
