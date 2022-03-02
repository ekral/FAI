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

    // Jde o kompozici, protoze kdyz prestane existovat objekt Motorka, tak prestanou existovat i objekty Kola
    // protoze jen motorka ma referenci na objekty kola a s nikym je nesdili.

    // Vymyslete vlastni ukazku na dedicnost pro vztah HAS-A
    class Noha
    {
        public int Delka { get; set; }

        public Noha(int delka)
        {
            Delka = delka;
        }
    }

    class Zidle
    {
        private Noha[] nohy;

        public Zidle()
        {
            nohy = new Noha[4] { new Noha(30), new Noha(30), new Noha(30), new Noha(30) };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Motorka motorka = new Motorka();
            Zidle zidle = new Zidle();
        }
    }
}
