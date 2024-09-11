using System;

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

        // Nahradte klicove slovo var sablonou Action, Func nebo Predicate,
        // pokud je mozne pouzit Predicate i Func, tak napiste obe reseni
        // Zmente pouze klientsky kod v metode Main

        static void Main(string[] args)
        {
            var d1 = Metoda1;
            var d2 = Metoda2;
            var d3 = Metoda3;
            var d4 = Metoda4;
            var d5 = Metoda5;
            var d6 = Metoda6;
            var d7 = Metoda7;
            var d8 = Metoda8;
        }
    }
}
