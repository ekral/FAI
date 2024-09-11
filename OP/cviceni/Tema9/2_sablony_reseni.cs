// Ukol 2: misto vlastniho definovaneho typu delegate pouzijte
// sablonu Action, Func nebo Predicate

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

        static bool JeKladne(int x)
        {
            return x > 0;
        }

        //delegate int MujDelegat1(int x, int y);
        //delegate void MulDelegat2(string text);
        //delegate bool MujDelegat3(int x);

        static void Main(string[] args)
        {
            int x = 2;
            int y = 3;

            Func<int, int, int> d1 = Soucet;
            int vysledek1 = d1.Invoke(x, y);
            Console.WriteLine(vysledek1);
            
            Action<string> d2 = Vypis; // Nahradte pomoci Action, Func nebo Predicate
            d2.Invoke("ahoj");

            Predicate<int> d3 = JeSude;  // Nahradte pomoci Action, Func nebo Predicate
            bool vysledek2 = d3.Invoke(2);
            Console.WriteLine(vysledek2);

            // Prepiste d3 s pouzitim Func
            Func<int, bool> d4 = JeSude;
        }
    }
}
