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

            // Add data for test 
        }


        public PujcovnaAutomobiluContext CreateContext()
        {
            var builder = new DbContextOptionsBuilder<PujcovnaAutomobiluContext>().UseSqlite("Data Source = testovaci.db");

            PujcovnaAutomobiluContext context = new PujcovnaAutomobiluContext(builder.Options);

            return context;
        }

    }
}
