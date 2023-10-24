using Microsoft.EntityFrameworkCore;
using Utb.PizzaKiosk.Models;

namespace Utb.PizzaKiosk.Tests
{

    [Collection("PizzaKiosk Database Collection")]
    public class UnitTestDatabase
    {
        public DatabaseFixture Fixture { get; }

        public UnitTestDatabase(DatabaseFixture fixture)
        {
            Fixture = fixture;    
        }

        [Fact]
        public void Test1()
        {
            using var context = Fixture.CreateContext();
        }
    }
}