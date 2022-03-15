// Ukol 1: Zmente metodu Zvuk na abstraktni a Zviratko na abstraktni tridu
// Ukol 2: a) Vytvorte tridu Zoo, ktera bude obsahovat seznam zviratek
//         b) a pouzijte ji v klientskem kodu
// Ukol 3: (skolni priklad) pro tridu Zoo implementuje rozhrani IDisposal,
//         tak aby ze Zoo odstranil vsechna zviratka,
//         kdyz se zoo odstrani z pameti
// Ukol 4: Napiste jaky je rozdil mezi:
//         a) Rozhranim a
//         b) Abstraktni tridou
//         c) Neabstraktni tridou s virtualnimi metodami

using Spectre.Console;
using System;
using System.Collections.Generic;

// pridat nuget Spectre Console

namespace ConsoleApp12
{
    // Ukol 1: Zmente metodu Zvuk na abstraktni a Zviratko na abstraktni tridu ✓
    abstract class Zviratko
    {
        public string Jmeno { get; set; }
        // pozdni vazba (late binding)
        abstract public string Zvuk(); // nema zadnou implementaci
    }

    // Ukol 2: a) Vytvorte tridu Zoo, ktera bude obsahovat seznam zviratek ✓

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
        // metoda pro pridani noveho zviratka

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

    class Program
    {


        static void Main(string[] args)
        {
            //Ukol 2: b) Vytvorte tridu Zoo a pouzijte ji v klientskem kodu         
            List<Zviratko> zviratka = new List<Zviratko>(); // nahradit tridou zoo

            bool konec = false;

            do
            {
                string druh = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Zvol [green]zviratko[/]:")
                    .AddChoices(new[] { Pejsek.Druh, Kacenka.Druh })
                );

                string name = AnsiConsole.Ask<string>("Zadej [green]jmeno[/]:");

                Zviratko nove = druh switch
                {
                    Pejsek.Druh => new Pejsek() { Jmeno = name },
                    Kacenka.Druh => new Kacenka() { Jmeno = name },
                    _ => throw new ArgumentException("Neplatny druh zviratka")
                };

                zviratka.Add(nove);

                foreach (Zviratko zviratko in zviratka)
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
