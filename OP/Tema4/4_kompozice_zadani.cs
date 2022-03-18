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

    class Program
    {
        static void Main(string[] args)
        {
            Motorka motorka = new Motorka();

            Autobus autobus1 = new Autobus("Zlin-Brno");
            Autobus autobus2 = new Autobus("Zlin-Uh");
            List<Autobus> seznamLinek = new List<Autobus>{ autobus1, autobus2 };

            Nadrazi nadrazi = new Nadrazi();

            nadrazi.Autobusy.Add(autobus1);
            nadrazi.Autobusy.Add(autobus2);
        }
    }
}
