using System;

namespace Tema9
{
    delegate void MujDelegat(int x);

    class Program
    {
        static void VypisA(int x)
        {
            Console.WriteLine($"A {x}");
        }

        static void VypisB(int x)
        {
            Console.WriteLine($"B {x}");
        }

        static void Main(string[] args)
        {
            MujDelegat d1 = VypisA;

            d1?.Invoke(1); // vypise A 1

            MujDelegat d2 = null; 

            d2 += VypisA;
            d2 += VypisB;

            d2?.Invoke(2); // vypise A 2 a B 2

            d2 -= VypisA;

            d2?.Invoke(3); // vypise jen B 3
        }
    }
}
