// Ukol 2: misto vlastniho definovaneho typu delegate pouzijte
// sablonu Action, Func nebo Predicate

using System;

namespace ConsoleApp23
{
    class Program
    {
        static int Soucet(int x, int y)
        {
            return x + y;
        }

        static int Soucin(int x, int y)
        {
            return x * y;
        }

        static void Vypis(string text)
        {
            Console.WriteLine(text);
        }

        static bool JeSude(int x)
        {
            return x % 2 == 0;
        }

        static bool JeKladne(int x)
        {
            return x > 0;
        }

        //delegate int Operace(int x, int y);

        //// Definujte typ delegate pro metodu Vypis a vytvorte ukazku pouziti typu delegate
        //delegate void VypisDelegat(string text);

        //// Definujte typ delegate pro metodu JeSude a vytvorte ukazku pouziti typu delegate
        //delegate bool SudeDelegat(int x);

        static void Main(string[] args)
        {
        
            int x = 2;
            int y = 3;

            Func<int, int, int> o1 = Soucin;
            int vysledek = o1.Invoke(x, y);
            Console.WriteLine(vysledek);

            Action<string> d1 = Vypis; // Pouzijte Action, Func nebo Predicate
            d1.Invoke("Ahoj");

            Predicate<int> s1 = JeKladne; // Pouzijte Action, Func nebo Predicate
            bool vysledek2 = s1.Invoke(1);
            Console.WriteLine(vysledek2);
        }
    }
}
