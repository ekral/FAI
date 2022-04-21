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
            Action d1 = Metoda1;
            Action<int> d2 = Metoda2;
            Func<int> d3 = Metoda3;
            Func<string, int> d4 = Metoda4;
            Func<string,double,int> d5 = Metoda5;
            Func<bool> d6 = Metoda6;
            Func<int,bool> d7 = Metoda7; // Predicate<int>
            Predicate<int> d7b = Metoda7;
            Func<int, double, bool> d8 = Metoda8;
        }
    }
}
