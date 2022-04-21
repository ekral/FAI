using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp30
{
    class Program
    {
        static void Metoda1()
        {
            Console.WriteLine("Ahoj");
        }

        static void Metoda2(int x)
        {
            Console.WriteLine(x);
        }

        static int Metoda3()
        {
            return 1;
        }

        static int Metoda4(string x)
        {
            return 1;
        }

        static int Metoda5(string x, double y)
        {
            return 1;
        }

        static bool Metoda6()
        {
            return true;
        }

        static bool Metoda7(int x)
        {
            return x > 0;
        }

        static bool Metoda8(int x, double y)
        {
            return x > y;
        }


        static void Main(string[] args)
        {
            // Misto metod Metoda1 az Metoda8 pouzijte lambda vyrazy

            Action d1 = () => Console.WriteLine("Ahoj");
            Action<int> d2 = x => Console.WriteLine(x);
            Func<int> d3 = () => 1;
            Func<string, int> d4 = x => 1;
            Func<string, double, int> d5 = (x, y) => 1;
            Func<bool> d6 = () => true;
            Predicate<int> d7 = x => x > 0;
            Func<int, bool> d7p = x => x > 0;
            Func<int, double, bool> d8 = (x, y) => x > y;
            
            // Ukazky pouziti:
            bool test = d8.Invoke(2, 3.0);
            List<int> cisla = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            List<int> suda = cisla.Where(x => x % 2 == 0).ToList();
            Console.WriteLine(string.Join(",", suda));
        }
    }
}
