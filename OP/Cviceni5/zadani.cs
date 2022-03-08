using System;
using System.Collections.Generic;

namespace ConsoleApp11
{
    class Zviratko
    {
        public string Jmeno { get; set; }

        public Zviratko(string jmeno)
        {
            Jmeno = jmeno;
        }

        // u normalni metody se rozhoduje v dobe prekladu dle typu reference
        // o virtualni metode se rozhoduje az za behu podle typu objektu
        virtual public void Zvuk()
        {
            Console.WriteLine("jsem abstraktni zviratko, nedelam konkretni zvuk");
        }
    }

    class Pejsek : Zviratko
    {
        public Pejsek(string jmeno) : base(jmeno)
        {
        }

        override public void Zvuk()
        {
            Console.WriteLine("haf haf");
        }
    }

    class Tucnak : Zviratko
    {
        public Tucnak(string jmeno) : base(jmeno)
        {
        }

        public override void Zvuk()
        {
            Console.WriteLine("NOOT NOOT");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // Vytvorte manazer Zoo s rozhranim v Specter Console
            // V manazeru budete moct vytvaret nova zoo a pridavat do nich zviratka a 
            // a vypisovat jmena a zvuky vsech zviratek v danem zoo
            // Vytvorte minimalne jeden unit test na to, kdyz pridate zviratko, tak ze je opravdu v zoo
            // Ukazka: https://gist.github.com/ekral/ed05585728a4c032ec404c27971dc435#file-unittest1-cs

            Zviratko zviratko1 = new Pejsek("Maxipes"); // upcasting - mame referenci zviratka na pejska
            zviratko1.Zvuk(); // rozhoduje typ reference

            List<Zviratko> zviratka = new List<Zviratko>();
            zviratka.Add(new Pejsek("Rex"));
            zviratka.Add(new Tucnak("Kowalski"));

            foreach (Zviratko zviratko in zviratka)
            {
                Console.WriteLine($"{zviratko.Jmeno}:");
                zviratko.Zvuk();
            }

            // Chceme v programu nahrazovat objekty jinymi kompatibilnimi objekty
            // tak abychom nemuseli menit existujici kodu
            // V C# mame statickou typovou kontrolou
        }
    }
}
