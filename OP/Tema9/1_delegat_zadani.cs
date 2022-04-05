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

        delegate int Operace(int x, int y);
        

        static void Main(string[] args)
        {
            int x = 2;
            int y = 3;
            Operace o1 = Soucin;
            int vysledek = o1.Invoke(x, y);
            Console.WriteLine(vysledek);
        }
    }
}
