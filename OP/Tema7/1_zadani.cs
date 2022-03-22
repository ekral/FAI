// Ukol 1: Pridejte do kodu logovani do textoveho souboru o pridani noveho zviratka do zoo
//         Vyuzite pri tom techniku Dependency Injection.
// Ukol 2: Vytvorte unit test, ktery otestuje, ze zviratko bylo spravne pridane do zoo
//         ale pri testu se nebude nic logovat do souboru.
// Domaci ukol 1:  Pridejte a otestujte metodu pro odebrani zviratka ze zoo
//                 kdy kazde zviratko bude mit navic Id pro snadnou identifikaci.
// Domaci ukol 2: Vytvorte logovani s pomoci Singletonu a zamyslete se nad tim, jak by to zhorsilo testovani kodu

using Spectre.Console;
using System;
using System.Collections.Generic;

// pridat nuget Spectre Console

namespace Tema7
{
    abstract class Zviratko
    {
        public string Jmeno { get; set; }

        public Zviratko(string jmeno)
        {
            Jmeno = jmeno;
        }

        abstract public string Zvuk(); 
    }

    class Pejsek : Zviratko
    {
        internal const string Druh = "Pejsek";

        public Pejsek(string jmeno) : base(jmeno)
        {
        }

        override public string Zvuk()
        {
            return "Haf haf";
        }
    }

    class Kacenka : Zviratko
    {
        internal const string Druh = "Kacenka";

        public Kacenka(string jmeno) : base(jmeno)
        {
        }

        override public string Zvuk()
        {
            return "mek mek";
        }
    }

    class Zoo 
    {
        public string Nazev { get; set; }

        private List<Zviratko> zviratka;
        public IReadOnlyCollection<Zviratko> Zviratka => zviratka; 

        public Zoo(string nazev)
        {
            Nazev = nazev;
            zviratka = new List<Zviratko>();
        }

        public void Pridej(Zviratko zviratko)
        {
            zviratka.Add(zviratko);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Zoo zoo = new Zoo("Zoo Lesna");

            bool konec = false;

            do
            {
                string druh = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Zvol [green]zviratko[/]:")
                    .AddChoices(new[] { Pejsek.Druh, Kacenka.Druh })
                );

                string jmeno = AnsiConsole.Ask<string>("Zadej [green]jmeno[/]:");

                Zviratko nove = druh switch
                {
                    Pejsek.Druh => new Pejsek(jmeno),
                    Kacenka.Druh => new Kacenka(jmeno),
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
