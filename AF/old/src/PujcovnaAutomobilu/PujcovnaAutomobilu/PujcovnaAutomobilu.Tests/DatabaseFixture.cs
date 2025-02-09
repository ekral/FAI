using Microsoft.EntityFrameworkCore;
using PujcovnaAutomobilu.Models;

namespace PujcovnaAutomobilu.Tests
{

    public class DatabaseFixture
    {
        public DatabaseFixture()
        {
            using var context = CreateContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Automobils.AddRange(
                new Automobil() { Id = 1, Model = "Škoda 105", Pujceno = false },
                new Automobil() { Id = 2, Model = "Citroen Berlingo", Pujceno = false },
                new Automobil() { Id = 3, Model = "Škoda Octavia", Pujceno = false }
            );

            context.SaveChanges();
        }


        public PujcovnaAutomobiluContext CreateContext()
        {
            var builder = new DbContextOptionsBuilder<PujcovnaAutomobiluContext>().UseSqlite("Data Source = testovaci.db");

            PujcovnaAutomobiluContext context = new PujcovnaAutomobiluContext(builder.Options);

            return context;
        }

    }
}
