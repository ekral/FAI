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
