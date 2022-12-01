// klientsky kod, client code
Ucet ucet = new Ucet(1);
ucet.Vloz(2000.0);
//ucet.Cislo = 2;
Console.WriteLine(ucet.Cislo);

Student student1 = new Student();
student1.Jmeno = "Iva";

// Object initializer
Student student2 = new Student()
{
    Jmeno = "Edmund"
};

class Predmet
{
    public readonly int id; // muzu zmenit jen v konstruktoru
    public int Id2 { get; } // readonly property


    public Predmet(int id)
    {
        this.id = id;
        Id2 = id;
    }

    public void Zmen()
    {
        //id = 0;
        //Id2 = 0;
    }
}
class Student
{
    public string? Jmeno { get; set; }
}


class Ucet
{
    public int Cislo { get; private set; } // autoimplemented property - stejny vykon jako fiedl

    // expression body syntax
    private double zustatek; 
    public double Zustatek
    {
        get => zustatek;
        set => zustatek = value; 
    }

    public Ucet(int cislo)
    {
        Cislo = cislo;
    }

    public void Vloz(double castka)
    {
        zustatek += castka;
    }
}
