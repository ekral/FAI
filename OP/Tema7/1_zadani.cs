// Ukol 1: Pridejte do kodu logovani do textoveho souboru o pridani noveho zviratka do zoo
//         Vyuzite pri tom techniku Dependency Injection.
// Ukol 2: Vytvorte unit test, ktery otestuje, ze zviratko bylo spravne pridane do zoo
//         ale pri testu se nebude nic logovat do souboru.
// Bonus:  Pridejte a otestujte metodu pro odebrani zviratka ze zoo
//         kdy kazde zviratko bude mit navic Id pro snadnou identifikaci.


using Spectre.Console;
using System;
using System.Collections.Generic;

// pridat nuget Spectre Console

namespace ConsoleApp12
{
    // Dedicnost kodu: IS-A
    // Kompozice: HAS-A
    // Rozhrani: Can Do
    interface IPrintable
    {
        void Tiskni();
    }

    // Ukol 1: Zmente metodu Zvuk na abstraktni a Zviratko na abstraktni tridu ✓
    abstract class Zviratko
    {
        public string Jmeno { get; set; }
        // pozdni vazba (late binding)
        abstract public string Zvuk(); // nema zadnou implementaci
    }

    class Pejsek : Zviratko
    {
        internal const string Druh = "Pejsek";

        override public string Zvuk()
        {
            return "Haf haf";
        }
    }

    class Kacenka : Zviratko
    {
        internal const string Druh = "Kacenka";

        override public string Zvuk()
        {
            return "mek mek";
        }
    }

    // Ukol 2: a) Vytvorte tridu Zoo, ktera bude obsahovat seznam zviratek ✓

    // Ukol 3: (skolni priklad) pro tridu Zoo implementuje rozhrani IDisposal ✓

    class Zoo : IDisposable
    {
        public string Nazev { get; set; }

        private List<Zviratko> zviratka;
        public IReadOnlyCollection<Zviratko> Zviratka => zviratka; // expression body definition pro read only property https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members#read-only-properties

        public Zoo(string nazev)
        {
            Nazev = nazev;
            zviratka = new List<Zviratko>();
        }

        public void Pridej(Zviratko zviratko)
        {
            zviratka.Add(zviratko);
        }

        // TODO: vyresit, kdyz vyvojar zavola Dispose rucne
        //       vyresit, kdyz by doslo na finalizer (constructor)
        public void Dispose()
        {
            // skolni priklad, slouzi pouze jako ukazka, pro zviratka to nemusime delat protoze je spravuje Garbage collector
            // tady odstranuje napriklad unmaged memmory, pristup k hardwaru, pripojeni k serveru
            // v beznem kodu se to nepouziva
            Console.WriteLine("Zoo se rusi, odvazim  zviratka ze zoo");
            zviratka.Clear();
        }
    }


    class Program
    {

        static void Main(string[] args)
        {
            //Ukol 2: b) Vytvorte tridu Zoo a pouzijte ji v klientskem kodu ✓          
            using Zoo zoo = new Zoo("Zoo Lesna");

            bool konec = false;

            do
            {
                string druh = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Zvol [green]zviratko[/]:")
                    .AddChoices(new[] { Pejsek.Druh, Kacenka.Druh })
                );

                string name = AnsiConsole.Ask<string>("Zadej [green]jmeno[/]:");

                // https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching
                Zviratko nove = druh switch
                {
                    Pejsek.Druh => new Pejsek() { Jmeno = name },
                    Kacenka.Druh => new Kacenka() { Jmeno = name },
                    _ => throw new ArgumentException("Neplatny druh zviratka")
                };

                zoo.Pridej(nove);

                foreach (Zviratko zviratko in zoo.Zviratka)
                {
                    Console.WriteLine(zviratko.Jmeno);
                    Console.WriteLine(zviratko.Zvuk());
                }

                if (!AnsiConsole.Confirm("Pridat nove zviratko?"))
                {
                    konec = true;
                }

            } while (!konec);
        }
    }
}
