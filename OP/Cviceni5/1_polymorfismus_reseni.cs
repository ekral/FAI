using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp11
{
    class Zoo
    {
        public string Nazev { get; set; }
        private List<Zviratko> zviratka;

        public Zoo(string nazev)
        {
            Nazev = nazev;
            zviratka = new List<Zviratko>();
        }

        public void Pridej(Zviratko zviratko)
        {
            zviratka.Add(zviratko);
        }

        public IEnumerable<string> VratZvuky()
        {
            return zviratka.Select(z => $"{z.Jmeno} dela {z.Zvuk()}");
        }
    }

    class Zviratko
    {
        public string Jmeno { get; set; }

        public Zviratko(string jmeno)
        {
            Jmeno = jmeno;
        }
        // u prekrytych metod se rozhoduje az za behu programu, volani je o neco pomalejsi
        virtual public string Zvuk()
        {
            // zjistuji zvuk
            return "nemam konkretni zvuk, jsem abstraktni zviratko";
        }
    }

    class Pejsek : Zviratko
    {
        public Pejsek(string jmeno) : base(jmeno)
        {
        }

        override public string Zvuk()
        {
            // zjistuji zvuk
            return "haf haf";
        }

        public override string ToString()
        {
            return $"Pejsek {Jmeno}";
        }
    }

    class Kocicka : Zviratko
    {
        public Kocicka(string jmeno) : base(jmeno)
        {
        }

        override public string Zvuk()
        {
            return "Mnau mnau";
        }

        public override string ToString()
        {
            return $"Kocicka {Jmeno}";
        }
    }

    class Program
    {
        static void VypisZvuky(Zoo zoo)
        {
            Console.WriteLine(zoo.Nazev);

            IEnumerable<string> zvuky = zoo.VratZvuky();
            foreach (string zvuk in zvuky)
            {
                Console.WriteLine(zvuk);
            }
        }

        static void Main(string[] args)
        {
            Dictionary<string, Zoo> seznamZoo = new Dictionary<string, Zoo>();

            Zoo lesna = new Zoo("Lesna");
            lesna.Pridej(new Pejsek("Fik"));
            lesna.Pridej(new Kocicka("Zavelina"));
            
            seznamZoo["lesna"] = lesna;
    
            foreach (KeyValuePair<string,Zoo> zoo in seznamZoo)
            {
                VypisZvuky(zoo.Value);
            }
        }
    }
}
