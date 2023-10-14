using Utb.PizzaKiosk.Models;

namespace Utb.PizzaKiosk.Tests
{
    [Collection("Database collection")]
    public class PizzaContextUnitTest
    {
        public PizzaContextUnitTest(TestDatabaseFixture fixture) => Fixture = fixture;

        public TestDatabaseFixture Fixture { get; }

        [Fact]
        public void PrvniPizzaJeHawai()
        {
            using PizzaContext context = Fixture.CreateContext();

            Pizza pizza = context.Pizzas.Single(s => s.Id == 1);

            Assert.Equal("Hawai", pizza.Jmeno);
        }

        // otestujte vsechny tri pizzy ze maji spravny nazev
        [Fact]
        public void MajiPizzySpravnyNazev()
        {
            using PizzaContext context = Fixture.CreateContext();

            Assert.Collection(context.Pizzas,
            s1 =>
            {
            Assert.Equal("Hawai", s1.Jmeno);
            },
            s2 =>
            {
                Assert.Equal("Syrova", s2.Jmeno);
            },
            s3 =>
            {
                Assert.Equal("Sunkova", s3.Jmeno);
            }
            );
        }

    }
}
