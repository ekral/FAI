using System;

namespace ConsoleApp11
{
    class Program
    { 
        static void Main(string[] args)
        {
            int pocetLetSplaceni = 20;
            double rocniUrokProcenta = 2;
            double D = 1000000; // dluh

            Console.WriteLine($"Pocet let {pocetLetSplaceni}, urok {rocniUrokProcenta}% rocne a castka {D} Kc");
            
            // Vypocet splatky uroku dle vzorce http://www.aristoteles.cz/matematika/financni_matematika/hypoteka-vypocet.php
            
            int n = pocetLetSplaceni * 12; // pocet mesicu splaceni
            double i = rocniUrokProcenta / (12 * 100); // desetinne cislo

            double v = 1 / (1 + i);
            double splatka = (i * D) / (1 - Math.Pow(v, n));

            Console.WriteLine($"Mesicni splatka bude {splatka:F2} Kc");
        }
    }
}
