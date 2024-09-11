// Ukol Delegat: Definujte typ delegate pro metody Vypis a JeSude a vytvorte ukazku pouziti typu delegate

using System;

namespace Tema9
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

        delegate int MujDelegat1(int x, int y);
        

        static void Main(string[] args)
        {
            int x = 2;
            int y = 3;
            MujDelegat1 d1 = Soucin;
            int vysledek = d1.Invoke(x, y);
            Console.WriteLine(vysledek);
        }
    }
}
