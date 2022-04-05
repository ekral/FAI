// Ukol 3: nahradte metody lambda vyrazy

using System;
using System.Collections.Generic;
using System.Linq;

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

            Func<int, int, int> o1 = (x, y) => x * y; // nahradit lambda vyrazem

            int vysledek = o1.Invoke(x, y);
            Console.WriteLine(vysledek);

            Action<string> d1 = text => Console.WriteLine(text);  // nahradit lambda vyrazem

            d1.Invoke("Ahoj");

            Predicate<int> s1 = x => x > 0; // nahradit lambda vyrazem

            bool vysledek2 = s1.Invoke(1);
            Console.WriteLine(vysledek2);

            List<int> cisla = new List<int> { -1, 2, 3, 4, 5, -2, -3 };
            int min = 0; // capture
            List<int> vetsiNezMin = cisla.Where(x => x > min).ToList();
            Console.WriteLine(string.Join(",", vetsiNezMin));
        }
    }
}
