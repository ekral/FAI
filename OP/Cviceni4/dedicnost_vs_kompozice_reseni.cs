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

    class StudentKompozice
    {
        public string Skupina { get; set; }
        public Osoba Osoba { get; set; }

        public StudentKompozice()
        {
            Osoba = new Osoba();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Student student = new Student();
            student.Jmeno = "Anezka";
            student.Prijmeni = "Lengalova";
            student.Skupina = "AXP1";

            StudentKompozice studentKompozice = new StudentKompozice();
            studentKompozice.Osoba.Jmeno = "Anezka";
            studentKompozice.Osoba.Prijmeni = "Lengalova";
            studentKompozice.Skupina = "AXP1";
        }
    }
}
