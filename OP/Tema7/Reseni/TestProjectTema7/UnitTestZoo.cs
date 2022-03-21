// Ukol 1: Pridejte do kodu logovani do textoveho souboru o pridani noveho zviratka do zoo
//         Vyuzite pri tom techniku Dependency Injection.
// Ukol 2: Vytvorte unit test, ktery otestuje, ze zviratko bylo spravne pridane do zoo
//         ale pri testu se nebude nic logovat do souboru.
// Bonus:  Pridejte a otestujte metodu pro odebrani zviratka ze zoo
//         kdy kazde zviratko bude mit navic Id pro snadnou identifikaci.
// Ukol k zamysleni: Vytvorte logovani s pomoci Singletonu a zamyslete se nad tim, jak by to zhorsilo testovani kodu

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
