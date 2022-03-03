# Vztahy mezi objekty (asociace, agregace a kompozice), skládání objektů. Dědičnost kódu, výhody a nevýhody ve srovnání se skládáním objektů.

Dědičnost kódu
---
Pro zvládnutí testu potřebujete znát jakým způsobem zapsat dědičnost kódu. Dědičnost kódu popisuje vztah specializace mezi třídami, například počítačová myš je (anglicky *IS A*) produkt, nebo tlačítko v aplikaci je ovládací prvek. 

Na následujím příkladu probereme co je to dědičnost kódu a jak ji zapsat. 

* Nejprve si definujeme rodičovskou třídu `Produkt`:
```cs 
class Produkt
{
    public double Cena { get; set; }
    public int Hodnoceni { get; set; }
}
```
* Jiná třída potom může zdědit kód této třídy. V následujícím příkladu máme třídu `Mys` jejíž součástí se díky dedičnosti stane kód třídy `Produkt`. Říkáme, že třída `Mys` je potomkem třídy `Produkt`. 

```cs 
class Mys : Produkt
{
    public int Dpi { get; set; }

    public Mys(double cena, double hodnoceni, int dpi) : base(cena, hodnoceni)
    {
        Dpi = dpi;
    }
}
```

Dědičnost a konstruktor
---

* Nyní si přidáme do třídy `Produkt` parametrický konstruktor:
```cs 
class Produkt
{
    public double Cena { get; set; }
    public int Hodnoceni { get; set; }

    public Produkt(double cena, int hodnoceni)
    {
        Cena = cena;
        Hodnoceni = hodnoceni;
    }
}
```
* Pomocí klíčového slova `base` potom zavoláme parametrický konstruktor rodiče. Konrétně zápis `base(cena, hodnoceni)` v následujícím kódu zavolá parametrický konstruktor třídy `Produkt`. Pokud má rodičovská tříd konstruktor bez parametrů nebo nemá žádný konstruktor, tak klíčové slovo `base` nemusíme použít.

```cs 
class Mys : Produkt
{
    public int Dpi { get; set; }

    public Mys(double cena, double hodnoceni, int dpi) : base(cena, hodnoceni)
    {
        Dpi = dpi;
    }
}
```

* Nakonec vytvoříme instanci třídy `Mys`. Všimněte si, že proměnná `Mys` má property Cena a Hodnocení, které zdědila od třídy `Produkt`.
```cs 
static void Main(string[] args)
{
    double cena = 600;
    int hodnoceni = 8;
    int dpi = 2000;

    Mys mys = new Mys(cena, hodnoceni, dpi);

    Console.WriteLine($"Pocitacova mys cena: {mys.Cena} hodnoceni: {mys.Hodnoceni} dpi: {mys.Dpi}");
}
```
* Poznámka: dědičnost kódu se samotná nepoužívá tak často jak by se zdálo, většinou se používá v kombinaci s polymorfismem.

---
Příklady k procvičování:


Kompozice a agregace
---

