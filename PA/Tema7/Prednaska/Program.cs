BankovniUcet ucet = new BankovniUcet(1, 0.0);
ucet.Vloz(20000.0);

if(double.TryParse(Console.ReadLine(), out double castka))
{
    if (castka <= ucet.Zustatek)
    {
        ucet.Vyber(castka);
    }
}

class BankovniUcet
{
    public int Id { get; }

    public double Zustatek { get; private set; }

    public BankovniUcet(int id, double zustatek)
    {
        Id = id;
        Zustatek = zustatek;
    }

    public void Vloz(double castka)
    {
        if (castka <= 0) throw new ArgumentOutOfRangeException(nameof(castka));

        Zustatek += castka;
    }

    public void Vyber(double castka)
    {
        if (castka > Zustatek) throw new ArgumentOutOfRangeException(nameof(castka));

        Zustatek -= castka;
    }
}
