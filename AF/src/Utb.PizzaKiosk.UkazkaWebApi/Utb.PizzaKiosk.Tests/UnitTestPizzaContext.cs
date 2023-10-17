using Microsoft.EntityFrameworkCore;
using Utb.PizzaKiosk.Models;
using Xunit;

namespace Utb.PizzaKiosk.Tests
{
    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {

    }

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

    [Collection("Database collection")]
    public class UnitTestPizzaContext 
    {
        private DatabaseFixture Fixture { get; }

        public UnitTestPizzaContext(DatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        public void FirstPizzaIsMargharita()
        {
            using PizzaContext context = Fixture.CreateContext();

            Pizza pizza = context.Pizzas.Single(p => p.Id == 1);

            Assert.Equal("Margharita", pizza.Name);
        }

        [Fact]
        public void AllPizzas()
        {
            using PizzaContext context = Fixture.CreateContext();

            var pizzas = context.Pizzas;

            Assert.Collection(pizzas,
                p1 => { Assert.Equal("Margharita", p1.Name); },
                p2 => { Assert.Equal("Salami", p2.Name); },
                p3 => { Assert.Equal("Funghi", p3.Name); });
        }

        [Fact]
        public void PizzaWithoutStyleShouldHaveMatchingStyleId()
        {
            using PizzaContext context = Fixture.CreateContext();

            Pizza pizza = context.Pizzas.Single(p => p.Id == 1);

            Assert.Null(pizza.PizzaStyle);
            Assert.Equal(1, pizza.PizzaStyleId);
        }

        [Fact]
        public void PizzaShouldHaveStyleEagerLoading()
        {
            using PizzaContext context = Fixture.CreateContext();

            // Eager
            Pizza pizza = context.Pizzas.Include(p => p.PizzaStyle).Single(p => p.Id == 1);

            Assert.NotNull(pizza.PizzaStyle);
            Assert.Equal(1, pizza.PizzaStyleId);
            Assert.Equal("Italsky styl", pizza.PizzaStyle.Description);
        }

        [Fact]
        public async Task PizzaShouldHaveStyleExplicitLoading()
        {
            using PizzaContext context = Fixture.CreateContext();

            // Explictly
            Pizza pizza = context.Pizzas.Single(p => p.Id == 1);
            await context.Entry(pizza).Reference(p => p.PizzaStyle).LoadAsync();

            Assert.NotNull(pizza.PizzaStyle);
            Assert.Equal(1, pizza.PizzaStyleId);
            Assert.Equal("Italsky styl", pizza.PizzaStyle.Description);
        }

        [Fact]
        public void PizzaShouldHaveTwoIngrediences()
        {
            using PizzaContext context = Fixture.CreateContext();

            // Eager
            Pizza pizza = context.Pizzas.Include(p => p.Incrediences).Single(p => p.Id == 1);

            Assert.NotNull(pizza.Incrediences);
            Assert.Equal(2, pizza.Incrediences.Count);
        }

        [Fact]
        public void PizzaMargheritaHasFries()
        {
            using PizzaContext context = Fixture.CreateContext();

            // Eager
            bool existuje = context.Pizzas.Any(p => p.Name == "Margharita" && p.Incrediences.Any(i => i.Name == "Hranolky"));

            Assert.True(existuje);
        }
    }

    
}