using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PujcovnaAutomobilu.Models;
using PujcovnaAutomobilu.WebApi;

namespace PujcovnaAutomobilu.Tests
{

    [Collection("Pujcovna Automobilu Database Collection")]
    public class UnitTestWebApi
    {
        DatabaseFixture Fixture { get; }
        public UnitTestWebApi(DatabaseFixture fixture)
        {
            Fixture = fixture;
        }
        [Fact]
        public async Task OtestujJedenAutomobil()
        {
            using PujcovnaAutomobiluContext context = Fixture.CreateContext();

            Results<NotFound, BadRequest, Ok<Automobil>> results = await WebApiVersion1.VratAutomobil(1, new MockEmailSender(), context);
            
            Ok<Automobil> result = Assert.IsType<Ok<Automobil>>(results.Result);

            Assert.NotNull(result.Value);

            Assert.Equal("Škoda 105", result.Value.Model);
        }
    }
}
