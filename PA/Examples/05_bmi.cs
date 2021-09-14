using System;

namespace MujPrvniProjekt
{
    class Program
    { 
        static void Main(string[] args)
        {
            double hmotnost = 85;
            double vyska = 1.78;

            Console.WriteLine($"hmotnost {hmotnost}kg a vyska {vyska}m");
            
            // vypocet bmi dle vzorce https://cs.wikipedia.org/wiki/Index_t%C4%9Blesn%C3%A9_hmotnosti

            double bmi = hmotnost / (vyska * vyska);

            Console.WriteLine($"bmi je {bmi}");
        }
    }
}
