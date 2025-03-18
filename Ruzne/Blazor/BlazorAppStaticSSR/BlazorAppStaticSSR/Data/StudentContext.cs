using BlazorAppStaticSSR.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppStaticSSR.Data
{
    public class StudentContext(DbContextOptions<StudentContext> options) : DbContext(options)
    {
        public DbSet<Student> Studenti { get; set; }
    }
}
