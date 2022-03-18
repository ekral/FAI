using System;

namespace Test
{
    abstract class Zviratko
    {
        public string Jmeno { get; set; }

        public abstract string VratZvuk();
    }

    class Pejsek : Zviratko
    {
        public override string VratZvuk()
        {
            return "haf haf";
        }
    }

    class Kocicka : Zviratko
    {
        public override string VratZvuk()
        {
            return "mnau";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Zviratko pejsek = new Pejsek() { Jmeno = "Azor" };
            Zviratko kocicka = new Kocicka() { Jmeno = "Micka" };

            Console.WriteLine($"{pejsek.Jmeno} dela {pejsek.VratZvuk()}"); // Azor dela haf haf
            Console.WriteLine($"{kocicka.Jmeno} dela {kocicka.VratZvuk()}"); // Micka dela mnau
        }
    }
}
