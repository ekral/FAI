using ProjektPizzeria;

namespace PizzeriaTestProject
{
    public class UnitTestPolozkaKosiku
    {
        [Fact]
        public void OdstranPridavek_JedenPridavek_PridavekOdstranen()
        {
            Produkt pizza = new Produkt(1, "MARGHERITA", 132.0m, Kategorie.Jidlo);
            Produkt ananas = new Produkt(2, "ananas 120g", 25.0m, Kategorie.Pridavek);
            Produkt ancovicky = new Produkt(2, "ančovičky 40g", 55.0m, Kategorie.Pridavek);

            // Vlozit pridavky a vlozit do kosiku
            PolozkaKosiku polozkaKosiku = new PolozkaKosiku(pizza);
            polozkaKosiku.PridejPridavek(ananas);
            polozkaKosiku.PridejPridavek(ananas);
            polozkaKosiku.PridejPridavek(ancovicky);

            polozkaKosiku.OdeberPridavek(ananas);

            var expected = new List<Produkt> { ananas, ancovicky }.OrderBy(p => p.Kod);
            Assert.Equal(expected, polozkaKosiku.Pridavky.OrderBy(p => p.Kod));
        }

        [Fact]
        public void Pridej_NeexistujiciPridavek_ArgumentException()
        {
            Produkt pizza = new Produkt(1, "MARGHERITA", 132.0m, Kategorie.Jidlo);
            Produkt ananas = new Produkt(2, "ananas 120g", 25.0m, Kategorie.Pridavek);
            PolozkaKosiku polozkaKosiku = new PolozkaKosiku(pizza);

            Assert.Throws<ArgumentException>(() => polozkaKosiku.OdeberPridavek(ananas));
        }
    }
}
