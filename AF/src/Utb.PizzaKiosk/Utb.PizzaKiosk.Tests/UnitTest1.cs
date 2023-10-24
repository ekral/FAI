using Microsoft.EntityFrameworkCore;
using Utb.PizzaKiosk.Models;

namespace Utb.PizzaKiosk.Tests
{

    [Collection("PizzaKiosk Database Collection")]
    public class UnitTest1
    {
        public DatabaseFixture Fixture { get; }

        public UnitTest1(DatabaseFixture fixture)
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