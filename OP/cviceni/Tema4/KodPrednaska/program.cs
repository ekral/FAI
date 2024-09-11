Student student = new Student("Peppa", "Prasatko", "A1X2");
//student.Jmeno = "Pepa";

Externista externista = new Externista("Jan", "Novy", 50);

Motorka motorka = new Motorka();
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
        Console.WriteLine($"{Jmeno} {Prijmeni} {Skupina}");
    }
}


class Kolo
{
    public int Polomer { get; set; }

    public Kolo(int polomer)
    {
        Polomer = polomer;
    }
}

// Asociace
// - Agregace  1
// - Kompozice 0 

// ownerwhip - ten kdo vlastni objekt, tak ho i odstrani z pameti

// HAS-A
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
class Zamestnanec : Osoba
{
    public string Kancelar { get; set; }

    public Zamestnanec(string jmeno, string prijmeni, string kancelar) : base(jmeno, prijmeni)
    {
        Kancelar = kancelar;
    }
}

class Externista
{
    public Osoba Osoba { get; set; }
    public double Bonus { get; set; }
    public Externista(string jmeno, string prijmeni, double bonus)
    {
        Osoba = new Osoba(jmeno, prijmeni);
        Bonus = bonus;
    }
}
