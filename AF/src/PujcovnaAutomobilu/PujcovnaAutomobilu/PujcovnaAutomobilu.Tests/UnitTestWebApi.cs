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

            Results<NotFound, BadRequest, Ok<Automobil>> result = await WebApiVersion1.VratAutomobil(1, new MockEmailSender(), context);

            Ok<Automobil> okAutomobil = Assert.IsType<Ok<Automobil>>(result.Result);

            Assert.NotNull(okAutomobil.Value);

            Assert.Equal("Škoda 105", okAutomobil.Value.Model);

        }
    }
}
