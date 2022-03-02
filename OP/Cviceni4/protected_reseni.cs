using System;

namespace ConsoleApp3
{
    class Osoba
    {
        // zvolte spravny modifikator pristupu tak, aby cisloUctu bylo pristupne v tride potomka, ale nebylo pristupne v klientskem kodu
        protected int cisloUctu;
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

        public void Vypis()
        {
            Console.WriteLine($"{Jmeno} {Prijmeni} {cisloUctu} {Skupina}"); // pujde prelozit
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            // klientsky kod
            Student student1 = new Student("Anezka", "Lengalova", "AXP1");
            Console.WriteLine(student1.cisloUctu); // nepujde prelozit
        }
    }
}
