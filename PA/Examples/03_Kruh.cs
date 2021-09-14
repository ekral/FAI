using System;

namespace MujPrvniProjekt
{
    class Program
    {
        static void Main(string[] args)
        {
            double r = 3.0;
            Console.WriteLine($"Polomer kruhu je {r}");
            Console.WriteLine($"Hodnota Pi je {Math.PI}");
            
            // kruh je definovany polomerem
            // spocitejte a vypiste obvod a obsah kruh

            double obvod = 2 * Math.PI * r;
            Console.WriteLine($"Obvod je {obvod}");

            double obsah = Math.PI * r * r;
            Console.WriteLine($"Obsah je {obsah}");

            Console.ReadKey();
        }
    }
}
