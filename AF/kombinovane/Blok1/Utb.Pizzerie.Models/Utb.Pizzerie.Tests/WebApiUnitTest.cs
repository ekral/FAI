using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utb.Pizzerie.Tests
{
    [Collection("Database collection")]
    public class WebApiUnitTest : IClassFixture<TestDatabaseFixture>
    {
        private TestDatabaseFixture Fixture { get; }

        public WebApiUnitTest(TestDatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        public async void VsechnyPizzy()
        {
            var context = Fixture.CreateContext();
            Microsoft.AspNetCore.Http.HttpResults.Ok<Models.Pizza[]> result = await PizzerieEndpointsV1.GetAllPizzas(context);

            Assert.NotNull(result.Value);
            Assert.Collection(result.Value, p1 => { Assert.Equal("Prvni", p1.Nazev); }, p2 => { Assert.Equal("Druha", p2.Nazev); });
        }
    }
}
