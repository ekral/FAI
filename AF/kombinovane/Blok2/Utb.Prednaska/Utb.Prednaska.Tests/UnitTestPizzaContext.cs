using Microsoft.EntityFrameworkCore;

namespace Utb.Prednaska.Tests
{
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

            Assert.Equal("Margherita", pizza.Name);
        }

        [Fact]
        public void AddNewPizza()
        {
            using PizzaContext context = Fixture.CreateContext();
            context.Database.BeginTransaction();

            Pizza novaPizza = new Pizza()
            { 
                Id = 0, 
                Name = "Hranolkova", 
                PizzaStyleId = 1,
                Price = 200
            };

            context.Pizzas.Add(novaPizza);

            context.SaveChanges();

            context.ChangeTracker.Clear();

            Pizza pizza = context.Pizzas.Single(p => p.Name == "Hranolkova");

            Assert.Equal("Hranolkova", pizza.Name);
        }
    }
}