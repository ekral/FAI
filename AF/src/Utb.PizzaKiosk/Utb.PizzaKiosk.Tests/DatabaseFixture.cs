using Microsoft.EntityFrameworkCore;
using Utb.PizzaKiosk.Data;

namespace Utb.PizzaKiosk.Tests
{
    public class DatabaseFixture
    {
        public DatabaseFixture()
        {
            using var context = CreateContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Add data for test 
        }


        public PizzaKioskContext CreateContext()
        {
            var builder = new DbContextOptionsBuilder<PizzaKioskContext>().UseSqlite("Data Source = testovaci.db");

            PizzaKioskContext context = new PizzaKioskContext(builder.Options);

            return context;
        }

    }
}
