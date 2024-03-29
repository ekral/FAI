﻿using Utb.PizzaKiosk.Models;

namespace Utb.PizzaKiosk.Tests
{
    public class DatabaseFixture
    {
        public DatabaseFixture()
        {
            using PizzaContext context = CreateContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public PizzaContext CreateContext()
        {
            return new PizzaContext("pizzatests.db");
        }
    }

    
}