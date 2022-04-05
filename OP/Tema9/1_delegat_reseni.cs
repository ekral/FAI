// Ukol Delegat: Definujte typ delegate pro metody Vypis a JeSude a vytvorte ukazku pouziti typu delegate

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

        delegate int MujDelegat1(int x, int y);
        delegate void MulDelegat2(string text);
        delegate bool MujDelegat3(int x);
        static void Main(string[] args)
        {
            int x = 2;
            int y = 3;

            MujDelegat1 d1 = Soucet;
            int vysledek1 = d1.Invoke(x, y);
            Console.WriteLine(vysledek1);
            
            MulDelegat2 d2 = Vypis;
            d2.Invoke("ahoj");

            MujDelegat3 d3 = JeSude;
            bool vysledek2 = d3.Invoke(2);
            Console.WriteLine(vysledek2);
        }
    }
}
