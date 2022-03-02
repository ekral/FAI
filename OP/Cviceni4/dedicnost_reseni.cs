using System;

namespace ConsoleApp3
{
    class Osoba
    {
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
    }

    class Student : Osoba
    {
        public string Skupina { get; set; }
    }
    
    // Vymyslete vlastni ukazku na dedicnost pro vztah IS-A
    class Soucastka
    {
        public string Vyrobce { get; set; }
        public string Id { get; set; }
    }

    class Motor : Soucastka
    {
        public string Typ { get; set; }
        public double Objem { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // klientsky kod
            Motor motor = new Motor();
            motor.Id = "100";
            motor.Vyrobce = "Mazda";
            motor.Objem = 1308;
            motor.Typ = "Wangleruv";
        }
    }
}
