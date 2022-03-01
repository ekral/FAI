using System;

namespace ConsoleApp9
{
    class Osoba
    {
        protected string Jmeno { get; set; }
        public string Prijmeni { get; set; }

        public Osoba(string jmeno, string prijmeni)
        {
            Jmeno = jmeno;
            Prijmeni = prijmeni;
        }
    }

    // IS-A
    class Student : Osoba
    {
        public string Skupina { get; set; }

        public Student(string jmeno, string prijmeni, string skupina) : base(jmeno, prijmeni)
        {
            Skupina = skupina;
        }

        public void Vypis()
        {
            Console.WriteLine(Jmeno);
        }
    }

    // Ukol: doplnte konstruktor do zamestnance a pripadne do trid ktere jste vymysleli

    class Zamestnanec : Osoba
    {
        public string Kancelar { get; set; }

        public Zamestnanec(string jmeno, string prijmeni, string kancelar) : base(jmeno,prijmeni)
        {
            Kancelar = kancelar;
        }
    }

    // kompozice
    class Externista 
    {
        public Osoba Osoba { get; set; }
        public double Bonus { get; set; }

        public Externista(string jmeno, string prijmeni, string kancelar, double bonus)
        {
            Osoba = new Osoba(jmeno, prijmeni);
            Bonus = bonus;
        }
    }

    // Vymyslete vlastni priklad na dedicnost
    class Zviratko
    {
        public int Vek { get; set; }
    }

    class Pejsek : Zviratko
    {
        public string Jmeno { get; set; }
    }

    class Kocicka : Zviratko
    {
        public bool Zachod { get; set; }
    }

    class Postava
    {
        public int Utok { get; set; }
        public int Obrana { get; set; }
    }

    class Drak : Postava
    {
        public string Jmeno { get; set; }
        public string Lokace { get; set; }
    }
    class Hrdina : Postava
    {
        public string Zbran { get; set; }
    }

    class Kolo
    {
        public int Velikost { get; set; }

        public Kolo(int velikost)
        {
            Velikost = velikost;
        }
    }

    // HAS-A
    class Motorka
    {
        public Kolo Predni { get; set; }
        public Kolo Zadni { get; set; }

        public Motorka(Kolo predni, Kolo zadni)
        {
            Predni = predni;
            Zadni = zadni;
        }
    }

    // Vymyslete priklad na vztah HAS-A
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
        public Noha[] Nohy { get; set; }

        public Zidle()
        {
            Nohy = new Noha[] { new Noha(60), new Noha(60), new Noha(60), new Noha(60) };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // ownership - vlastnik je ten kdo objekt zrusi
            // agregace - kacenka na rybniku
            // kompozice - molekuly kysliku a vodiku ve vode rybniku

            // client code
            Motorka motorka = new Motorka(new Kolo(15), new Kolo(15));

            Student student1 = new Student("Michal", "Vesely", "AXP1");
            //Console.WriteLine(student1.Jmeno);
            Zamestnanec zamestnanec1 = new Zamestnanec("Peppa", "Prasatko", "822");

            Externista externista1 = new Externista("Jiri", "Novy", "122", 100);
        }
    }
}
