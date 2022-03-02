using System;

namespace ConsoleApp3
{
    class Osoba
    {
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }

        public Osoba(string jmeno, string prijmeni)
        {
            Jmeno = jmeno;
            Prijmeni = prijmeni;
        }
    }

    // doplnte konstruktor pro Studenta a Zamestnance
    class Student : Osoba
    {
        public string Skupina { get; set; }

        public Student(string jmeno, string prijmeni, string skupina) : base(jmeno, prijmeni)
        {
            Skupina = skupina;
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Student student1 = new Student("Anezka", "Lengalova", "AXP1");
        }
    }
}
