using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utb.PizzaKiosk.Models;

namespace Utb.PizzaKiosk.Tests
{
    [Collection("Database collection")]
    public class UnitTestPizzaWebApi
    {
        private DatabaseFixture Fixture { get; }

        public UnitTestPizzaWebApi(DatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        public async Task ThereShouldBeThreePizzas()
        {
            using PizzaContext context = Fixture.CreateContext();

            Ok<Pizza[]> result = await WebApiV1.GetAllPizzas(context);

            Assert.NotNull(result.Value);

            Pizza[] pizzas = result.Value;

            Assert.Equal(3, pizzas.Length);
        }

      
    }
}
