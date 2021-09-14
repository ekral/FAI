using System;

namespace MujPrvniProjekt
{
    class Program
    { 
        static void Main(string[] args)
        {
            double x = 2.0;
            double y = 3.0;
            double z = 0.0;

            // Matematicke operatory
            z = x + y; // soucet
            z = x - y; // rozdil
            z = x * y; // soucin
            z = x / y; // podil
            z = -x; // zaporna hodnota
            z = x * x; // druha mocnina

            // Matematicke operace ze tridy Math
            z = Math.Pow(x, 100.0); // mocnina x^100
            z = Math.Sqrt(9.0); // druha odmocnina

            // Konstanta PI
            z = 2 * Math.PI * x; // konstanta PI
            // Priorita operatoru
            z = x * (y + 3.0); // kulate zavorky urcuji prioritu 

            // Zmena hodnoty promenne
            z = z + 2.0; // zvyseni o hodnotu
            z = z - 2.0; // snizeni o hodnotu
            ++z; // zvyseni o 1
            --z; // snizeni o 1

            // Operace pro int
            int a = 2;
            int b = 3;
            b = (int)Math.Pow(a, 100.0); // mocnina x^100
            b = (int)Math.Sqrt(9.0); // druha odmocnina
            a = a + 2; // zvyseni o hodnotu
            a = a - 2; // snizeni o hodnotu
        }
    }
}