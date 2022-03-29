// Ukol 1: Prepracujte tridu Sklad na generickou, tak aby byl validni klientsky kod v metode Main ✓

class Sklad<T>
{
    T[] data;
    private int pocet;

    public Sklad(int kapacita)
    {
        data = new T[kapacita];
    }

    public void Zaloz(T objekt)
    {
        data[pocet++] = objekt;
    }

    public T Vyloz()
    {
        return data[--pocet];
    }
}

abstract class Zviratko
{
    public string Jmeno { get; set; }
    public abstract string Zvuk();
}

class Pejsek : Zviratko
{
    public override string Zvuk()
    {
        return "haf haf";
    }
}

class Kocicka : Zviratko
{
    public override string Zvuk()
    {
        return "mnau";
    }
}
// Ukol 2: Omezte genericky typ tak, aby do skladu sla pridavat jen zviratka ✓

class SkladZviratek<T> where T : Zviratko
{
    T[] data;
    private int pocet;

    public SkladZviratek(int kapacita)
    {
        data = new T[kapacita];
    }

    public void Zaloz(T objekt)
    {
        System.Console.WriteLine($"Zakladam zviratko {objekt.Jmeno} co dela zvuk {objekt.Zvuk()}");
        data[pocet++] = objekt;
    }

    public T Vyloz()
    {
        return data[--pocet];
    }
}

class Program
{
    static void Main(string[] args)
    {
        Sklad<int> skladInt = new Sklad<int>(10);
        skladInt.Zaloz(1);
        int celeCislo = skladInt.Vyloz();

        Sklad<string> skladString = new Sklad<string>(10);
        skladString.Zaloz("Ahoj");
        string retezec = skladString.Vyloz();
        
        SkladZviratek<Zviratko> zviratka = new SkladZviratek<Zviratko>(10);
        zviratka.Zaloz(new Pejsek() { Jmeno = "Rex" });
        zviratka.Zaloz(new Kocicka() { Jmeno = "Micka" });
    }
}
