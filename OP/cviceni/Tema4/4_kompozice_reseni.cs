using System;
using System.Collections.Generic;

namespace Ukol4
{
    // Kompozice - motorka kola "nesdili" s jinym objektem, kola zaniknou s motorkou

    class Kolo
    {
        public int Prumer { get; set; }

        public Kolo(int prumer)
        {
            Prumer = prumer;
        }
    }

    class Motorka
    {
        private Kolo predni;
        private Kolo zadni;

        public Motorka()
        {
            predni = new Kolo(20);
            zadni = new Kolo(19);
        }
    }

    // Agregace - autobusove nadrazi sdili autobusy s jinymi objekty, kdyz prestane existovat nadrazi, autobusy nezaniknout

    class Autobus
    {
        public string Linka { get; set; }

        public Autobus(string linka)
        {
            Linka = linka;
        }
    }

    class Nadrazi
    {
        public List<Autobus> Autobusy { get; set; }

        public void PridejAutobus(Autobus autobus)
        {
            Autobusy.Add(autobus);
        }
    }

    // Vymyslete dve ukazky na vztah HAS-A
    // Vytvorte jednu pro agregaci
    // Vytvorte jednu pro kompozici

    // Kompozice
    class Noha
    {
        public int Delka { get; set; }

        public Noha(int delka)
        {
            Delka = delka;
        }
    }

    class Zidle
    {
        private Noha[] nohy;

        public Zidle()
        {
            nohy = new Noha[4] { new Noha(30), new Noha(30), new Noha(30), new Noha(30) };
        }
    }

    // Agregace
    class Ridic
    {
        public string Jmeno { get; set; }

        public Ridic(string jmeno)
        {
            Jmeno = jmeno;
        }
    }

    class Automobil
    {
        public string Typ { get; set; }
        public Ridic Ridic { get; set; }

        public Automobil(string typ)
        {
            Typ = typ;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Motorka motorka = new Motorka();
            Zidle zidle = new Zidle();

            Autobus autobus1 = new Autobus("Zlin-Brno");
            Autobus autobus2 = new Autobus("Zlin-Uh");
            List<Autobus> seznamLinek = new List<Autobus>{ autobus1, autobus2 };

            Nadrazi nadrazi = new Nadrazi();

            nadrazi.Autobusy.Add(autobus1);
            nadrazi.Autobusy.Add(autobus2);

            Ridic ridic = new Ridic("Karel");

            Automobil automobil1 = new Automobil("Oktavia");
            automobil1.Ridic = ridic;

            Automobil automobil2 = new Automobil("Mustang");
            automobil2.Ridic = ridic;
        }
    }
}
