using Knihovna.Data;
using Microsoft.EntityFrameworkCore;

namespace Knihovna.Seeder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string? variable = Environment.GetEnvironmentVariable("ConnectionStrings__knihovna-database");

            var options = new DbContextOptionsBuilder<KnihovnaDbContext>().UseSqlServer(variable).Options;
            using KnihovnaDbContext context = new(options);

            context.Database.EnsureCreated();

            context.Knihy.Add(new Kniha() { Autor = new() { Jmeno = "Karel", Prijmeni = "Hasek" }, Nazev = "Svejk" });
           
            context.SaveChanges();
        }
    }
}
