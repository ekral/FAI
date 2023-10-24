using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utb.PizzaKiosk.Models;

namespace Utb.PizzaKiosk.Tests
{
    [CollectionDefinition("PizzaKiosk Database Collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {

    }
    public class DatabaseFixture
    {
        public DatabaseFixture()
        {
            var builder = new DbContextOptionsBuilder<PizzaKioskContext>().UseSqlite("Data Source = testovaci.db");

            using PizzaKioskContext context = new PizzaKioskContext(builder.Options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }


    }
}
