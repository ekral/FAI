// Ukol 3 misto metod pouzijte lambda vyrazy

using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp23
{
    class Program
    {

        //static int Soucet(int x, int y)
        //{
        //    return x + y;
        //}

        //static void Vypis(string text)
        //{
        //    Console.WriteLine(text);
        //}

        //static bool JeSude(int x)
        //{
        //    return x % 2 == 0;
        //}

        //static bool JeKladne(int x)
        //{
        //    return x > 0;
        //}

        //delegate int MujDelegat1(int x, int y);
        //delegate void MulDelegat2(string text);
        //delegate bool MujDelegat3(int x);

        static Predicate<int> VratPredikat()
        {
            int min = 0;
            Predicate<int> p = x => x > min; // capture min

            return p;
        }

        static void Main(string[] args)
        {
            int x = 2;
            int y = 3;

            Func<int, int, int> d1 = (x, y) => x + y;  // Lambda vyraz
            int vysledek1 = d1.Invoke(x, y);
            Console.WriteLine(vysledek1);

            Action<string> d2 = x => Console.WriteLine(x); // Lambda vyraz
            d2.Invoke("ahoj");

            Predicate<int> d3 = x => x > 0;  // Lambda vyraz
            bool vysledek2 = d3.Invoke(2);
            Console.WriteLine(vysledek2);

            // Prepiste d3 s pouzitim Func
            Func<int, bool> d4 = x => x % 2 == 0; // Lambda vyraz

            List<int> cisla = new List<int> { -2, -3, 0, 5, 9, 7 };
            IEnumerable<int> kladna = cisla.Where(x => x > 0);
            Console.WriteLine(string.Join(",", kladna));
        }
    }
}
