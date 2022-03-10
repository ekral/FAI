using Spectre.Console;
using System;
using System.Collections.Generic;

namespace ConsoleApp12
{
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

        override public string Zvuk()
        {
            return "Haf haf";
        }
    }

    class Kacenka : Zviratko
    {
        override public string Zvuk()
        {
            return "mek mek";
        }
    }


    class Program
    {
        // TODO pouzit enum pro druhy zviratek

        static void Main(string[] args)
        {
            List<Zviratko> zviratka = new List<Zviratko>();
            bool konec = false;

            do
            {
                string druh = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Zvol [green]zviratko[/]:")
                    .AddChoices(new[] { "Pejsek", "Kacenka" })
                );

                string name = AnsiConsole.Ask<string>("Zadej [green]jmeno[/]:");

                Zviratko nove = druh switch
                {
                    "Pejsek" => new Pejsek() { Jmeno = name },
                    "Kacenka" => new Kacenka() { Jmeno = name },
                    _ => throw new ArgumentException("Neplatny druh zviratka")
                };

                zviratka.Add(nove);

                if (!AnsiConsole.Confirm("Pridat nove zviratko?"))
                {
                    
                }

            } while (!konec);


            foreach (Zviratko zviratko in zviratka)
            {
                Console.WriteLine(zviratko.Jmeno);
                Console.WriteLine(zviratko.Zvuk());
            }
        }
    }
}
