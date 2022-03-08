Pejsek pejsek = new Pejsek("Fik");
Kocicka kocicka = new Kocicka("Micka", 10);

Zviratko zviratko = kocicka; // upcasting

// downcasting
if(zviratko is Kocicka k)
{
    Console.WriteLine(k.ZaplneniZachodu);
}

// starsi
Kocicka k2 = zviratko as Kocicka;
if(k2 != null)
{
    Console.WriteLine(k.ZaplneniZachodu);
}

List<Zviratko> zviratka = new List<Zviratko>();
zviratka.Add(pejsek);
zviratka.Add(kocicka);

foreach (Zviratko z in zviratka)
{
    Console.WriteLine(z.Jmeno);
    z.Zvuk();
}

class Zviratko
{
    public string Jmeno { get; set; }

    public Zviratko(string jmeno)
    {
        Jmeno = jmeno;
    }

    virtual public void Zvuk()
    {
        Console.WriteLine("jsem abstraktni zviratko, nedelam zadny zvuk");
    }
}

class Pejsek : Zviratko
{
    public Pejsek(string jmeno) : base(jmeno)
    {
    }

    override public void Zvuk()
    {
        Console.WriteLine("haf haf");
    }
}

class Kocicka : Zviratko
{
    public int ZaplneniZachodu { get; set; }

    public Kocicka(string jmeno, int zaplneniZachodu) : base(jmeno)
    {
        ZaplneniZachodu = zaplneniZachodu;
    }
    
    override public void Zvuk()
    {
        Console.WriteLine("mnau");
    }
}

// pozdni vazba, late binding, muze byt pomalejsi, javascript, smalltalk
// c, c++, c# - casna vazba - hodne rychle
// c++, c# - muzu si zvolit ze pujde o pozdni vazbu
// staticka typova kontrola
