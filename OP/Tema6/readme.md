Příklady k procvičování:
1. Abstrakní metody a rozhraní: [zadání](1_zadani.cs) [řešení](1_reseni.cs)
---

## Abstraktni metoda a třída

Abstraktní metoda nemá implementaci (tělo) a může být definována pouze v abstraktní třídě. Abstraktní třída slouží pouze jako rodičovská třída a v klientském kódu používáme její potomky, ale nemůžeme vytvářet její instance. Opět jde o kontrukci kterou vytváříme z důvodu statické typové kontroly.

Nyní si projdeme příklad, kdy si nejprve vytvoříme virtuální metodu `VratZvuk`, kterou poté změníme na abstraktní.

Nejprve si definujeme třídu `Zviratko`, ktera ma property `Jmeno` a virtuální metodu `VratZvuk`:

```cs 
class Zviratko
{
    public string Jmeno { get; set; }

    public virtual string VratZvuk()
    {
        return "?";
    }
}
```
a potom definujeme dva potomky této třídy, které překryjí s použitím klíčového slova `override` metodu `VratZvuk`. Prvním potomkem je třída `Pejsek`, která vrací zvuk "haf haf" a druhým třída `Kocicka`, která vrací zvuk "mnau".

```cs 
class Pejsek : Zviratko
{
    public override string VratZvuk()
    {
        return "haf haf";
    }
}

class Kocicka : Zviratko
{
    public override string VratZvuk()
    {
        return "mnau";
    }
}

```
Použití tříd v klientském kódu je potom následující:

```cs 
Zviratko zviratko = new Zviratko() { Jmeno = "Obecne zviratko" };
Zviratko pejsek = new Pejsek() { Jmeno = "Azor" };
Zviratko kocicka = new Kocicka() { Jmeno = "Micka" };

Console.WriteLine($"{zviratko.Jmeno} dela {zviratko.VratZvuk()}"); // Obecne zviratko dela ?
Console.WriteLine($"{pejsek.Jmeno} dela {pejsek.VratZvuk()}"); // Azor dela haf haf
Console.WriteLine($"{kocicka.Jmeno} dela {kocicka.VratZvuk()}"); // Micka dela mnau
```

Pokud se zamyslíme nad metodou `VratZvuk` ve třídě `Zviratko`, tak pro ni **nemáme smysluplnou implementaci** protože jde o obecné (abstraktní) zvířátko které nedělá žádný zvuk. Takové metody pro které nemáme implementaci a které musí implementovat až potomci této třídy můžeme označit jako abstraktní klíčovým slovem `abstract`. Abstraktní metody mohou mít pouze abstraktní třídy, třída `Zviratko` tedy musí být označená klíčovým slovem `abstract`. Takové třídy jsou potom určené pouze pro dědičnost a nemůžeme vytvářet jejich instance. Potomci těchto tříd potom musí povinně implementovat abstraktní metody.

```cs 
abstract class Zviratko
{
    public string Jmeno { get; set; }

    public abstract string VratZvuk();
}
```

Použití tříd `Pejsek` a `Kocicka`a v klientském kódu zůstává stejné, jediný rozdíl je v tom, že nemůžeme vytvářet instance třídy `Zviratko`, můžeme mít pouze referenci tohoto typu:

```cs 
Zviratko pejsek = new Pejsek() { Jmeno = "Azor" };
Zviratko kocicka = new Kocicka() { Jmeno = "Micka" };

Console.WriteLine($"{pejsek.Jmeno} dela {pejsek.VratZvuk()}"); // Azor dela haf haf
Console.WriteLine($"{kocicka.Jmeno} dela {kocicka.VratZvuk()}"); // Micka dela mnau
```

## Rozhraní


Rozhraní je podobné jako abstraktní třída pouze s abstraktními metodami. Říkáme, že třída implementuje rozhraní. Každá třída může v jazyce c# dědit od jedné třídy ale může implementovat libovolný počet rozhraní. Rozhraní na rozdíl od abstraktních tříd neobsahují fieldy.

Z hlediska použití popisuje abstraktní třída vztah **"is a"** tedy pejsek **je** zvířátko nebo monitor **je** produkt. Zatímco rozhraní popisuje spíše vztah **"can do"** nebo možná lépe **"must do"**, tedy například že třída faktura **umí** serializaci do textového souboru nebo třída soubor **umí** metodu Dispose, tedy uvolnit všechny své alokované zdroje a zavřít otevřený soubor. Většinou preferujeme více jednoduchých rozhraní s méně metodami, než jedno velké rozhraní s mnoha metodami.

Rozhraní se používají často frameworcích kde pomocí nich určujeme co daná třída umí, například pomocí implementace rozhraní `IComparable` můžeme třídu naučit aby fungovala v metodě `Sort`. Rozhraní se také často používají v technice Dependency Injection, kterou probereme příště, kdy místo třídy používáme rozhraní a vlastní implementaci pak můžeme dle potřeb měnit, například místo reálné implementace použijeme testovací implementaci.

Nejprve si zopakujeme definici abstraktní třídy `Zviratko`:

```cs 
abstract class Zviratko
{
    public abstract string VratZvuk();
}

class Pejsek : Zviratko
{
    public override string VratZvuk()
    {
        return "haf haf";
    }
}
```

Podobný příklad bychom potom mohli zapsat pomocí rozhraní, které zápis výrazně zjednodušuje:

```cs 
interface IZviratko
{
    string VratZvuk();
}

class Pejsek : IZviratko
{
    public string VratZvuk()
    {
        return "haf haf";
    }
}
```
Vzhledem k tomu, že toto rozhraní by mohli implementovat i jiné třídy než zvířátka, tak bychom mohli toto rozhraní mohli také nazvat například `IZvuk` (anglicky `ISoundable`) a implementovat by ji mohla třeba i třída `Auto`:

```cs 
interface IZvuk
{
    string VratZvuk();
}

class Pejsek : IZvuk
{
    public string VratZvuk()
    {
        return "haf haf";
    }
}

class Auto : IZvuk
{
    public string VratZvuk()
    {
        return "brmmm brmmm";
    }
}
```

Použití v klientském kódu potom může být následující:

```cs 
static void VypisZvuk(IZvuk objektSeZvukem)
{
    Console.WriteLine(objektSeZvukem.VratZvuk());
}

static void Main(string[] args)
{
    Pejsek pejsek = new Pejsek();
    Auto auto = new Auto();

    VypisZvuk(pejsek);
    VypisZvuk(auto);
}
```
