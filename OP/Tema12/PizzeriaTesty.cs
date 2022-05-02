using PizzeriaKonzole;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PizzeriaTesty
{

    public class UnitTestPolozkaKosiku
    {
        [Fact]
        public void OdstranPridavek_JedenPridavek_PridavekOdstranen()
        {
            Produkt pizza = new Produkt(1, "MARGHERITA", 132.0m, Kategorie.Jidlo);
            Produkt ananas = new Produkt(2, "ananas 120g", 25.0m, Kategorie.Pridavek);
            Produkt ancovicky = new Produkt(2, "ančovičky 40g", 55.0m, Kategorie.Pridavek);

            // TODO
        }

       
    }
}
