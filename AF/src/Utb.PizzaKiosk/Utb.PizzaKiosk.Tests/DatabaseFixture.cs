using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utb.PizzaKiosk.Models;

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
