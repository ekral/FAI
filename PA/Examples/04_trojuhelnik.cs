using System;

namespace MujPrvniProjekt
{
    class Program
    {
        static void Main(string[] args)
        {
            double a = 3.0;
            double b = 4.0;
            double c = 5.0;
            Console.WriteLine($"Delka strany trojuhelnika je {a}, {b} a {c}");
            
            // trojuhelnik je definovany delkami stran
            // spocitejte a vypiste obvod a obsah trojuhelniku dle heronova vzorce https://cs.wikipedia.org/wiki/Heron%C5%AFv_vzorec
            
            double obvod = a + b + c;
            Console.WriteLine($"Obvod je {obvod}");

            double s = (a + b + c) / 2;
            double obsah = Math.Sqrt(s * (s - a) * (s - b) * (s -c));
            Console.WriteLine($"Obsah je {obsah}");

            Console.ReadKey();
        }
    }
}
