using System;

namespace MujPrvniProjekt
{
    class Program
    {
        static void Main(string[] args)
        {
            double n = 3.0;
            Console.WriteLine($"Delka strany je {n}");
            
            // ctverec definovany delkou strany
            // spocitejte a vypiste obvod a obsah ctverce
            
            double obvod = 4 * n;
            Console.WriteLine($"Obvod je {obvod}");

            double obsah = n * n;
            Console.WriteLine($"Obsah je {obsah}");

            Console.ReadKey();
        }
    }
}
