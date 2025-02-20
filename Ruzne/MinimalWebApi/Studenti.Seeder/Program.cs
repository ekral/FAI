using Microsoft.EntityFrameworkCore;
using Studenti.Models;

namespace Studenti.Seeder
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using StudentContext context = new(new DbContextOptionsBuilder<StudentContext>().UseSqlite(Settings.GetConnectionString("database.db")).Options);

            await context.Database.EnsureCreatedAsync();

            await context.Studenti.AddRangeAsync(
                new Student() { Jmeno = "Jiri", Studuje = true},
                new Student() { Jmeno = "Karel", Studuje = true},
                new Student() { Jmeno = "Alena", Studuje = false},
                new Student() { Jmeno = "Petr", Studuje = true});

            await context.SaveChangesAsync();
        }           
    }
}
