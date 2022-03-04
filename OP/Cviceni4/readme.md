## 1. Dědičnost kódu

Dědičnost kódu popisuje vztah specializace mezi třídami, například počítačová myš je (anglicky *IS A*) produkt, nebo tlačítko v aplikaci je ovládací prvek. 

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

## 2. Dědičnost a konstruktor

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

* Nakonec vytvoříme instanci třídy `Mys`. Všimněte si, že proměnná `Mys` má property `Cena` a `Hodnocení`, které zdědila od třídy `Produkt`.
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

## 3. Klíčové slovo protected

Klíčové slovo `protected` představuje modifikátor přístupu používaný pouze v dědičnosti. Tímto modifikátorem označujeme metody a atributy, které očekáváme, že využije jeho potomek v rámci dědění, ale v klientském kodů mají být skryté. V následujícím příkladu je proměnná `cisloUctu` přístupná v rodičovské třídě `Osoba` a v třídě potomka `Student`, ale není přístupná v klientském kódu v metodě `Main`.

```cs 
// klientsky kod
Student student = new Student(123, "Alena", "AXP1");
student.Vypis();
student.cisloUctu = 0; // nejde prelozit

class Osoba
{
    protected int cisloUctu;
    public string Jmeno { get; set; }

    public Osoba(int cisloUctu, string jmeno)
    {
        this.cisloUctu = cisloUctu;
        Jmeno = jmeno;
    }
}

class Student : Osoba
{
    public string Skupina { get; set; }

    public Student(int cisloUctu, string jmeno, string skupina) : base(cisloUctu, jmeno)
    {
        Skupina = skupina;
    }

    public void Vypis()
    {
        Console.WriteLine($"{cisloUctu} {Jmeno} {Skupina}"); // jde prelozit
    }
}
```
## 4. Kompozice

Pokud jeden objekt zahrnuje druhý objekt, tak můžeme mluvit o vztahu HAS-A, tedy že jeden objekt má druhý objekt. V následujícím příkladu bude mít instance třídy `Motorka` reference na dvě instance třídy `Kola`. Protože motorka tyto kola nesdílí s jinými objekty, někdy říkáme že je vlastní, tak mluvíme o kompozici. Vlastnictví objektu (ownership) v tomto kontextu znamená, že když zanikne objekt, tak zanikou i objekty které vlastní.

```cs 
class Kolo
{
    public int Prumer { get; set; }

    public Kolo(int prumer)
    {
        Prumer = prumer;
    }
}

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
```
## 5. Dědičnost vs kompozice


TODO

---
TODO: Příklady k procvičování:
