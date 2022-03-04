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

Pokud jeden objekt zahrnuje druhý objekt, tak můžeme mluvit o vztahu HAS-A, tedy že jeden objekt má druhý objekt. V následujícím příkladu bude mít instance třídy `Motorka` reference na dvě instance třídy `Kola`. Protože objekt motorka objekty kola nesdílí s jinými objekty jde o kompozici. V tomto případě také mluvíme o vlastnictví objektu, kdy vlastnictví objektu (ownership) v tomto kontextu znamená, že když zanikne objekt, tak zanikne i objekt který tento objekt vlastní. 

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
Pokud by objekt sdílel zahrnutý objekt s jinými objekty, tak by šlo o **agregaci**. U agregace mluvíme o tom, že objekt používá jiný objekt ale nevlastní ho. V následujícím příkladu sdílí `smsSender` více objednávek.

```cs 
// klientsky kod
SmsSender smsSender = new SmsSender();

Objednavka objednavka1 = new Objednavka(1, smsSender);
Objednavka objednavka2 = new Objednavka(2, smsSender);

objednavka1.Odeslat();
objednavka2.Odeslat();

class SmsSender
{
    public void PosliSms(string text)
    {
        Console.WriteLine($"Posilam sms: {text}");
    }
}

class Objednavka
{
    public int Id { get; set; }
    private SmsSender sender;

    public Objednavka(int id, SmsSender sender)
    {
        Id = id;
        this.sender = sender;
    }

    public void Odeslat()
    {
        sender.PosliSms($"Objednavka {Id} odeslana");
    }
}
```
Pojmy agregace a kompozice se používají také v jazyce UML.

## 5. Dědičnost vs kompozice

Dědičnost kódu můžeme nahradit do určité míry kompozicí. V následujícím příkladu nepoužíváme dědičnost, ale třída `Student` si vytváří vlastní instanci třídy `Osoba`.

```cs 
// klientsky kod
Student student = new Student(123, "Alena", "AXP1");
student.Osoba.Jmeno = "Tereza";
student.Vypis();

class Osoba
{
    public int cisloUctu { get; private set; }
    public string Jmeno { get; set; }

    public Osoba(int cisloUctu, string jmeno)
    {
        this.cisloUctu = cisloUctu;
        Jmeno = jmeno;
    }
}

class Student 
{
    public Osoba Osoba { get; private set; }
    public string Skupina { get; set; }

    public Student(int cisloUctu, string jmeno, string skupina)
    {
        Osoba = new Osoba(cisloUctu, skupina);
        Skupina = skupina;
    }

    public void Vypis()
    {
        Console.WriteLine($"{Osoba.cisloUctu} {Osoba.Jmeno} {Skupina}"); 
    }
}
```
---
Příklady k procvičování:
1. Dědičnost [zadání](1_dedicnost_zadani.cs) [řešení](1_dedicnost_reseni.cs)
2. Konstruktor u dědičnosti zadání řešení
3. Klíčové slovo protected:
4. Kompozice:
5. Dědičnost vs kompozice:

