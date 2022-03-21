using System.Collections.Generic;
using Tema7;
using Xunit;

namespace TestProjectTema7
{
    class LoggerStub : ILogger
    {
        public void Log(string text)
        {
            
        }
    }

    public class UnitTestZoo
    {
        [Fact]
        public void Pridej_Zviratko_ZviratkoJeVZoo()
        {
            LoggerStub logger = new LoggerStub();
            Zoo zoo = new Zoo("Testovaci", logger);
            
            Pejsek pejsek = new Pejsek(0, "Rex");
            zoo.Pridej(pejsek);

            List<Zviratko> expected = new List<Zviratko>()
            {
                pejsek
            };

            Assert.Equal(expected, zoo.Zviratka);
        }
    }
}