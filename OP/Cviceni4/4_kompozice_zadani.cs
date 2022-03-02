using System;

namespace ConsoleApp3
{
    class Kolo
    {
        public int Prumer { get; set; }

        public Kolo(int prumer)
        {
            Prumer = prumer;
        }
    }

    class Motorka
    {
        private Kolo predni;
        private Kolo zadni;

        public Motorka()
        {
            predni = new Kolo(20);
            zadni = new Kolo(19);
        }
    }

    // Jde o agregaci nebo kompozici?
    // Vymyslete vlastni ukazku na dedicnost pro vztah HAS-A
    
    class Program
    {
        static void Main(string[] args)
        {
            Motorka motorka = new Motorka();
        }
    }
}
