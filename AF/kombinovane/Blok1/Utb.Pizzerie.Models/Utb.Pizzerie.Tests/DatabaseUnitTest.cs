namespace Utb.Pizzerie.Tests
{
    [Collection("Database collection")]
    public class DatabaseUnitTest : IClassFixture<TestDatabaseFixture>
    {
        private TestDatabaseFixture Fixture { get; }

        public DatabaseUnitTest(TestDatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        public void VsechnyPizzy()
        {
            var context = Fixture.CreateContext();
            var pizzas = context.Pizzas;

            Assert.Collection(pizzas, p1 => { Assert.Equal("Prvni", p1.Nazev); }, p2 => { Assert.Equal("Druha", p2.Nazev); });
        }
    }
}