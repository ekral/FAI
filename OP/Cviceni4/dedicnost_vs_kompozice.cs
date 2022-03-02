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

    // Nahradte dedicnost kompozici

    class Program
    {
        static void Main(string[] args)
        {
            Student student = new Student() { Jmeno = "Anezka", Prijmeni = "Lengalova", Skupina = "AXP1" };
        }
    }
}
