using Xunit;

namespace TestProjectBankovniUcet
{
   
    class LoggerStub : ILogger
    {
        public void Log(string message)
        {
           
        }
    }

    public class UnitTestBankovniUcet
    {
        [Fact]
        public void Vloz_JednuCastku_ZustatekVeVysiVkladu()
        {
            BankovniUcet ucet = new BankovniUcet(new LoggerStub());
            ucet.Vloz(2000.0);
            double expected = 2000.0;

            Assert.Equal(expected, ucet.Zustatek);
        }
    }
}