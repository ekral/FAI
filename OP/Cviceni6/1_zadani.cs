// Ukol 1: zmente Zviratko na abstraktni tridu
// Ukol 2: Vytvorte tridu Zoo, ktera bude obsahovat seznam zviratek
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
// <PackageReference Include="Spectre.Console" Version="*"/>
namespace ConsoleApp12
{
    // Ukol 1: Zmente metodu Zvuk na abstraktni a Zviratko na abstraktni tridu
    class Zviratko
    {
        public string Jmeno { get; set; }
        // pozdni vazba (late binding)
        virtual public string Zvuk()
        {
            return "Jsem abstraktni zviratko nedelam zadny konrektni zvuk";
        }
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
            List<Zviratko> zviratka = new List<Zviratko>();
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
