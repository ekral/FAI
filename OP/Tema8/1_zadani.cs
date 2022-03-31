// Ukol 1a: Prepracujte tridu Sklad na generickou, tak aby byl validni klientsky kod v metode Main
// Ukol 1b: Omezte genericky typ tak, aby sel pouzit jen typ Zviratka a jeho potomci.

abstract class Zviratko
{
    public string Jmeno { get; set; }
    public abstract string VratZvuk();
}

class Pejsek : Zviratko
{
    public override string VratZvuk()
    {
        return "haf haf";
    }
}

class Sklad
{
    int[] data;
    private int pocet;

    public Sklad(int kapacita)
    {
        data = new int[kapacita];
    }

    public void Zaloz(int objekt)
    {
        data[pocet++] = objekt;
    }

    public int Vyloz()
    {
        return data[--pocet];
    }
}

class Program
{
    static void Main(string[] args)
    {
        // V Ukolu 1 pujdou vsechny tri

        // Nepujde v Ukolu 2
        Sklad<int> skladInt = new Sklad<int>(10); 
        skladInt.Zaloz(1);
        int celeCislo = skladInt.Vyloz();

        // Nepujde v Ukolu 2
        Sklad<string> skladString = new Sklad<string>(10); 
        skladString.Zaloz("Ahoj");
        string retezec = skladString.Vyloz();

        // Pujde v Ukolu 2
        Sklad<Zviratko> skladZviratek = new Sklad<Zviratko>(10); 
        skladZviratek.Zaloz(new Pejsek() { Jmeno = "Rex" });
        Zviratko pejsek = skladZviratek.Vyloz();
    }
}
