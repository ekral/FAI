using System;

namespace Tema9
{
    delegate int DelegatOperace(int x, int y);

    class Program
    {
        static int Suma(int x, int y)
        {
            return x + y;
        }

        static int Soucin(int x, int y)
        {
            return x * y;
        }

        static void Main(string[] args)
        {
            DelegatOperace operace = Suma;
            int vysledek = operace.Invoke(2, 3);
            Console.WriteLine(vysledek);

            operace = Soucin;
            vysledek = operace.Invoke(2, 3);
            Console.WriteLine(vysledek);
        }
    }
}
