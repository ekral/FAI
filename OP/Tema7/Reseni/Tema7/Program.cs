// Ukol 1: Pridejte do kodu logovani do textoveho souboru o pridani noveho zviratka do zoo
//         Vyuzite pri tom techniku Dependency Injection.
// Ukol 2: Vytvorte unit test, ktery otestuje, ze zviratko bylo spravne pridane do zoo
//         ale pri testu se nebude nic logovat do souboru.
// Bonus:  Pridejte a otestujte metodu pro odebrani zviratka ze zoo
//         kdy kazde zviratko bude mit navic Id pro snadnou identifikaci.
// Ukol k zamysleni: Vytvorte logovani s pomoci Singletonu a zamyslete se nad tim, jak by to zhorsilo testovani kodu

using Spectre.Console;
using System;
using System.Collections.Generic;

// pridat nuget Spectre Console

namespace Tema7
{
    public interface ILogger
    {
        void Log(string text);
    }

    // TODO: osetrit exceptions
    class FileLogger : ILogger
    {
        public void Log(string text)
        {
            string log = $"{DateTime.Now}: {text}";

            System.IO.File.AppendAllText("log.txt", log + System.Environment.NewLine);
        }
    }

    class DebugLogger : ILogger
    {
        public void Log(string text)
        {
            string log = $"{DateTime.Now}: {text}";

            System.Diagnostics.Debug.WriteLine(log);
        }
    }

    public abstract class Zviratko
    {
        public int Id { get; set; }
        public string Jmeno { get; set; }

        public Zviratko(int id, string jmeno)
        {
            Id = id;
            Jmeno = jmeno;
        }

        abstract public string Zvuk(); 
    }

    public class Pejsek : Zviratko
    {
        internal const string Druh = "Pejsek";

        public Pejsek(int id, string jmeno) : base(id, jmeno)
        {
        }

        override public string Zvuk()
        {
            return "Haf haf";
        }
    }

    public class Kacenka : Zviratko
    {
        internal const string Druh = "Kacenka";

        public Kacenka(int id, string jmeno) : base(id, jmeno)
        {
        }

        override public string Zvuk()
        {
            return "mek mek";
        }
    }

    public class Zoo 
    {
        private ILogger logger;

        public string Nazev { get; set; }

        private List<Zviratko> zviratka;
        public IReadOnlyCollection<Zviratko> Zviratka => zviratka; 

        public Zoo(string nazev, ILogger logger)
        {
            Nazev = nazev;
            this.logger = logger;
            zviratka = new List<Zviratko>();
        }

        public void Pridej(Zviratko zviratko)
        {
            int id = zviratka.Count > 0 ? zviratka.Max(zviratko => zviratko.Id) : 0;
            zviratko.Id = id + 1;
            zviratka.Add(zviratko);

            // Ukol 1 ✓
            logger.Log($"Pridano zviratko id: {zviratko.Id} jmeno: {zviratko.Jmeno}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Zoo zoo = new Zoo("Zoo Lesna", new ConsoleLogger("log.txt"));

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
                    Pejsek.Druh => new Pejsek(0, jmeno),
                    Kacenka.Druh => new Kacenka(0, jmeno),
                    _ => throw new ArgumentException("Neplatny druh zviratka")
                };

                zoo.Pridej(nove);

                foreach (Zviratko zviratko in zoo.Zviratka)
                {
                    Console.WriteLine($"{zviratko.Id}: {zviratko.Jmeno}");
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
