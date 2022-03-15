Eshop eshop = new Eshop();
eshop.VytvorObjednavku();
// eshop.Objednavky.Add(new Objednavka()); // nejde

foreach (IPrintable printable in eshop.Objednavky)
{
    printable.Vytiskni();
}

// Zviratko z = new Zviratko(); // nelze vytvorit instanci abstraktni tridy
Pejsek pejsek = new Pejsek();

Zviratko p = new Pejsek();
p = new Kocicka();

List<IPrintable> dokumenty = new List<IPrintable>()
{
    new Faktura(),
    new Objednavka()
};

foreach (IPrintable tisk in dokumenty)
{
    tisk.Vytiskni();
}

using (MojeTrida mojeTrida = new MojeTrida())
{

}

class MojeTrida : IDisposable
{
    private bool disposed = false;
    public MojeTrida()
    {
        Console.WriteLine("Pripojuji se k hardwaru");
    }

    public void Dispose()
    {
        Dispose(false);
    }

    private void Dispose(bool dispossing)
    {
        if (!disposed)
        {
            if (!dispossing)
            {
                // zavolat dispose u zahrnutych objektu
            }

            Console.WriteLine("odpojuji se od hardwaru");
            disposed = true;
        }
    }
    // finalizer (obdoba destruktoru v c++)
    ~MojeTrida()
    {
        // zavola garbage collector, kdyz se rozhode objekt uvolnit z pameti
        // posledni moznnost uvolnit hardware
        Dispose(true);
    }
}

// CAN DO
interface IPrintable
{
    void Vytiskni();
}

class Faktura : IPrintable, IComparable<Faktura>
{
    public int CompareTo(Faktura? other)
    {
        throw new NotImplementedException();
    }

    public void Vytiskni()
    {
        Console.WriteLine("tisknu fakturu");
    }
}


class Objednavka : IPrintable
{
    public void Vytiskni()
    {
        Console.WriteLine("Tisknu objednavku");
    }
}

class Eshop
{
    private List<Objednavka> objednavky;
    public IReadOnlyCollection<Objednavka> Objednavky => objednavky;

    public Eshop()
    {
        objednavky = new List<Objednavka>();
    }

    public void VytvorObjednavku()
    {
        Objednavka nova = new Objednavka();
        objednavky.Add(nova);
    }
}

// slouzi pouze proto aby se od nich dedilo
// nemuzu vytvaret jejich instance
abstract class Zviratko
{
    public string Jmeno { get; set; }
    abstract public void Zvuk();
}

class Pejsek : Zviratko

{
    override public void Zvuk()
    {
        Console.WriteLine("haf haf");
    }

    public void Aport()
    {

    }
}

class Kocicka : Zviratko
{
    override public void Zvuk()
    {
        Console.WriteLine("mnau");
    }
}
