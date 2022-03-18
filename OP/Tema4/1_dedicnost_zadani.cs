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

    class Program
    {
        static void Main(string[] args)
        {
            // klientsky kod
            Student student1 = new Student();
            student1.Jmeno = "Anezka";
            student1.Prijmeni = "Lengalova";
            student1.Skupina = "AXP1";
        }
    }
}
