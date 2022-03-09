using System;
using System.Collections.Generic;

namespace ConsoleApp4
{
    class Zviratko
    {
        public string Jmeno { get; set; }
        virtual public void Zvuk()
        {
            Console.WriteLine("Jsem abstraktni zviratko nedelam konkretni zvuk");
        }
    }

    class Pejsek : Zviratko
    {
        override public void Zvuk()
        {
            Console.WriteLine("Haf haf");
        }
    }

    class Krava : Zviratko
    {
        public override void Zvuk()
        {
            Console.WriteLine("Mua Mua");
        }
    }
    class Kocicka : Zviratko
    {
        public int ZaplneniZachodu { get; set; }
        override public void Zvuk()
        {
            Console.WriteLine("Mnau");
        }
    }

    class Zoo
    {
        public List<Zviratko> Zviratka { get; private set; } 
        public Zoo()
        {
            Zviratka = new List<Zviratko>();
        }
        public void Pridej(Zviratko zviratko)
        {
            Zviratka.Add(zviratko);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Zoo lesna = new Zoo();
           
            lesna.Pridej(new Krava() { Jmeno = "Milka" });
            lesna.Pridej(new Pejsek() { Jmeno = "Rex" });
            lesna.Pridej(new Kocicka() { Jmeno = "Cat woman" });

            foreach (Zviratko z in lesna.Zviratka)
            {
                Console.WriteLine(z.Jmeno);
                z.Zvuk();
            }

            List<Zviratko> zviratka = new List<Zviratko>();
            zviratka.Add(new Pejsek() { Jmeno = "komisar Rex" });
            zviratka.Add(new Kocicka() { Jmeno = "Cat Woman" });

            foreach (Zviratko zviratko in zviratka)
            {
                Console.WriteLine($"{zviratko.Jmeno}:");
                zviratko.Zvuk();
            }
            // javascript, smalltalk - pozdni vazba, late biding
            // o tom, ktera metoda se zavola se rozhoduje az za behu programu
            // mensi vykon, volani je pomalejsi

            // V c++, v C# je casna vazba, early binding, rozhoduje se v dobe prekladu
            // vetsi vykon, volani je rychlejsi
            // pozdni vazba jen kdyz ji potrebujeme, tak ji explicitne definujeme

            Zviratko x = new Pejsek();
            x.Zvuk();

            x = new Kocicka();
            x.Zvuk();
        }
    }
}
