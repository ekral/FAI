using System;

namespace Ukol3
{
    class Osoba
    {
        // pouzijte spravny modifikator pristupu, tak aby cislo uctu 
        // nebylo pristupne v klientskem kodu
        // ale byl pristupny v tride potomka

        protected int cisloUctu; // zde zmente modifikator pristupu
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }

        public Osoba(string jmeno, string prijmeni, int cisloUctu)
        {
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            this.cisloUctu = cisloUctu;
        }
    }


    class Student : Osoba
    {
        
        public string Skupina { get; set; }

        public Student(string jmeno, string prijmeni, int cisloUctu, string skupina)
            : base(jmeno, prijmeni, cisloUctu)
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
            Student student = new Student("Bohumil", "Vesely", 123, "AXP1");
            student.Vypis();
            student.cisloUctu = 1; // tento radek nepujde prelozit
        }
    }
}
