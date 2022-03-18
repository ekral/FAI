class BankovniUcet
{
    public int cislo;
    private double zustatek;

    public BankovniUcet(int cislo)
    {
        zustatek = 0.0;
        this.cislo = cislo;
    }

    public double VratZustatek()
    {
        return zustatek;
    }
    public void Vloz(double castka)
    {
        // zapis do databaze
        zustatek += castka;
    }

    public void Vyber(double castka)
    { 
        if (castka <= zustatek)
        {
            // zapis do db
            zustatek -= castka;
        }
    }
}

class Prednaska1
{
    static void Main(string[] args)
    {
        BankovniUcet ucet = new BankovniUcet(1);
        ucet.Vloz(2000);
        ucet.Vyber(3000);
        //ucet.zustatek -= 200; // nejde prelozit

        Console.WriteLine($"zustatek: {ucet.VratZustatek()}");
    }
}
