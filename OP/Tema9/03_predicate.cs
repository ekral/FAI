using System;

namespace Test10
{
    delegate bool DelegatTest(int x);

    class Program
    {
        static bool JeSude(int x)
        {
            return (x % 2) == 0;
        }

        static bool JeLiche(int x)
        {
            return (x % 2) != 0;
        }

        static void Main(string[] args)
        {
            DelegatTest test = JeSude;
            bool vysledek = test.Invoke(2);
            Console.WriteLine(vysledek);

            test = JeLiche;
            vysledek = test.Invoke(2);
            Console.WriteLine(vysledek);
        }
    }
}
