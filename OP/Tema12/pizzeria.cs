using System;
using System.Collections.Generic;

// Dotykovy objednavkovy system pro pizzerii
// Objednat pizzu a dalsi produkty

// 1. Trida Produkt (pizza, pizza tycinky atd.) plus pridavky

// trida Kosik
// tridy Objednavka a Polozka objednavky
// pokud se zmeni cena Produktu tak se nezmeni cena v objednavce

// Nevytvarejte UI, ale jen si vytvorte testy
// Zvolte minimalni funkcionalitu

namespace ConsoleApp34
{
    public enum Kategorie
    {
        Jidlo,
        Pridavek
    }

    public class Produkt
    {
        public int Kod { get; set; }
        public string Nazev { get; set; }
        public decimal Cena { get; set; }
        public Kategorie Kategorie { get; set; }

        public Produkt(int kod, string nazev, decimal cena, Kategorie kategorie)
        {
            Kod = kod;
            Nazev = nazev;
            Cena = cena;
            Kategorie = kategorie;
        }
    }

    public class PolozkaKosiku
    {
        public Produkt Produkt { get; set; }

        private List<Produkt> pridavky  = new List<Produkt>();
        public IReadOnlyCollection<Produkt> Pridavky => pridavky;


        public PolozkaKosiku(Produkt produkt)
        {
            Produkt = produkt;
        }

        public void PridejPridavek(Produkt produkt)
        {
            pridavky.Add(produkt);
        }

        public void OdeberPridavek(Produkt produkt)
        {
            Produkt odstranit = pridavky.Find(p => p.Kod == produkt.Kod);

            if (odstranit == null)
            {
                throw new ArgumentException("Produkt neni v pridavcich");
            }

            pridavky.Remove(odstranit);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Produkt pizza = new Produkt(1, "MARGHERITA", 132.0m, Kategorie.Jidlo);
            Produkt ananas = new Produkt(2, "ananas 120g", 25.0m, Kategorie.Pridavek);

            // Vlozit pridavky a vlozit do kosiku
            PolozkaKosiku polozkaKosiku = new PolozkaKosiku(pizza);
            polozkaKosiku.PridejPridavek(ananas);
            polozkaKosiku.PridejPridavek(ananas);
            polozkaKosiku.OdeberPridavek(ananas);
        }
    }
}
