Příklady k procvičování:
1. Abstrakní metody a rozhraní: [zadání](1_zadani.cs) [řešení](1_reseni.cs)
---

## Dependency injection


Technika Dependency Injection se používá k tomu aby jedna třída nebyla přímo závislá na jiné třídě a její konkrétní implementaci. Tato technika často používá proto aby byl kód lépe testovatelný, protože můžeme v kódu jednodušeji nakonfigurovat objekt pro potřeby testu. Technika Dependency Injection je založena na tom, že místo reference typu konkrétní třídy používáme referenci typu rozhraní a vlastní instanci potom předáváme nejčastěji v konstruktoru (jsou ale i varianty s Property nebo metodou). 

Lépe se ale tato technika chápe na konrétním příkladu. Ukážeme si příklad, kdy budeme mít třídu `Automobil` a ta bude mít field `motor` a budeme chtít při vytváření instance třídy `Automobil` zvolit, zda bude mít benzínový nebo naftový motor.

### Příklad bez použití Dependency Injection

* Nejprve si definujeme třídu BenzinovyMotor:
```cs 
class BenzinovyMotor
{
    public void Nastartuj()
    {
        Console.WriteLine("Zapalil jsem smes benzinu a vzduchu zapalovaci svickou");
    }
}
```
* A potom třídu `Automobil`, která bude mít referenci a objekt typu `BenzinovyMotor` a metodě Jed zavolá metodu motoru `Nastartuj`:

```cs 
class Automobil
{
    private BenzinovyMotor motor;

    public Automobil()
    {
        motor = new BenzinovyMotor();
    }

    public void Jed()
    {
        Console.WriteLine("Startuji ...");
        motor.Nastartuj();
    }
}
```
* Použití třídy `Automobil` potom bude následující:

```cs 
static void Main(string[] args)
{
    Automobil automobil = new Automobil();
    automobil.Jed();
}
```

### Příklad s použitím Dependency Injection

* Představme si ale situaci, že bychom chtěli při vytváření nového automobilu zvolit typ motoru, například místo benzínového motoru bychom chtěli zvoli následující naftový motor, a nechtěli bychom přitom měnit kód třídy `Automobil`:

```cs 
class NaftovyMotor
{
    public void Nastartuj()
    {
        Console.WriteLine("Zapalil jsem jsem smes díky vznícení způsobené vysokou teplotou stlačeného vzduchu");
    }
}
```
* Nejprve vytvoříme rozhraní, které budou implementovat oba motory (a případně i další nové typy motorů):

```cs 
interface IMotor
{
    void Nastartuj();
}

class BenzinovyMotor : IMotor
{
    public void Nastartuj()
    {
        Console.WriteLine("Zapalil jsem smes benzinu a vzduchu zapalovaci svickou");
    }
}

class NaftovyMotor : IMotor
{
    public void Nastartuj()
    {
        Console.WriteLine("Zapalil jsem jsem smes díky vznícení způsobené vysokou teplotou stlačeného vzduchu");
    }
}
```
* Ve třídě `Automobil` potom budeme mít referenci `motor` typu `IMotor` místo konkrétního typu motoru. A konkrétní motor si předáme jako parametr konstruktoru.

```cs 
class Automobil
{
    private IMotor motor;

    public Automobil(IMotor motor)
    {
        this.motor = motor;
    }

    public void Jed()
    {
        Console.WriteLine("Startuji ...");
        motor.Nastartuj();
    }
}
```

* Při vytváření instance třídy `Automobil` v konstruktoru předáme referenci na instanci konkrétního typu motoru a nemusíme přitom měnit kód třídy `Automobil`:

```cs 
static void Main(string[] args)
{
    Automobil automobilBenzin = new Automobil(new BenzinovyMotor());
    automobilBenzin.Jed();

    Automobil automobilNafta = new Automobil(new NaftovyMotor());
    automobilNafta.Jed();
}
```

## Singleton

Singleton představuje klasický návrhový vzor (postup) používaný pokud chceme mít pouze a jenom jednu instanci třídy v programu, například instanci třídy pro logování do souboru. Singleton je většinou implementovaný jako třída s private konstruktorem, která má statickou metodu Instance, která vrací referenci na statický field – vlastní instanci třídy, kterou chceme použít.
I když má Singleton své výhody a zaručuje že v programuje je vždy jen jedna jeho instance, bývá někdy označován jako **anti-pattern**, protože využití statických členských metod může snižovat testovatelnost kódu.

V následujícím příkladu nejprve použijeme pro logování (zápisu průběhu programu například do textového souboru) Singleton a poté ten samý příklad vyřešíme pomocí techniky Dependency Injection.

Následující třída `SingletonLogger` má `private` konstruktor, což znamená, že můžeme vytvářet její instance jen v metodách této třídy a ne v klientském kódu. Potom máme static property `Instance`, která ověří zda má static field `instance` hodnotu `null`, tedy zda je již instance vytvořena a pokud ne, tak ji vytvoří a vrátí referenci na tuto instanci. Pokud už instance existuje, tak novou nevytváří, tím je, zaručeno, že v celém programu bude existovat jen jedna instance třídy `SingletonLogger`. Field `counter` počítá počet logů a v příkladu je uvedený jen pro ukázku použití fieldu v tříde `SingletonLogger`.

```cs 
class SingletonLogger
{
    private static SingletonLogger instance;

    private int counter;
    
    private SingletonLogger()
    {
        counter = 0;
    }
    
    public void Log(string text)
    {
        Console.WriteLine($"{counter}: text");
        ++counter;
    }
    
    public static SingletonLogger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SingletonLogger();
            }
            return instance;
        }
    }
}
```

Nyní použijeme `SingletonLogger` v třídě `BankovniUcet`, kdy budeme logovat na konzoli každý vklad na účet.

```cs 
Ucet banka = new Ucet();
banka.Vyber(1000000m);

class Ucet
{
    public decimal Zustatek { get; set; }
    
    public Ucet()
    {
        Zustatek = 1000;
    }
    
    public void Vyber(decimal castka)
    {
        if (castka > Zustatek)
        {
            SingletonLogger.Instance.Log("error");
            return;
        }

        Zustatek -= castka;
    }
}
```

### Singleton vs Dependency Injection

Nyní stejný příklad s logováním vkladu na bankovní účet vyřešíme s pomocí techniky Dependency Injection. Nejprve si nadefinujeme rozhraní `ILogger`:

```cs 
interface ILogger
{
    void Log(string text);
}
```

A potom třídu `ConsoleLogger` implementující toto rozhraní:
```cs 
class ConsoleLogger : ILogger
{
    private int counter;
    
    public ConsoleLogger()
    {
        counter = 0;
    }

    public void Log(string text)
    {
        Console.WriteLine($"{counter}: text");
        ++counter;
    }
}
```

Třída bankovní účet potom bude používat pouze typ rozhraní `ILogger` a nemá žádnou referenci na konkrétní implementaci:
```cs 
class Ucet
{
    private ILogger logger;
    
    public decimal Zustatek { get; set; }
    
    public Ucet(ILogger logger)
    {
        this.logger = logger;
        
        Zustatek = 1000;
    }

    public void Vyber(decimal castka)
    {
        if (castka > Zustatek)
        {
            logger.Log("error");
            return;
        }

        Zustatek -= castka;
    }
}
```
V klientském kódu potom předáme objekt typu `ConsoleLogger` jako argument konstruktoru `BankovniUcet`:
```cs 
ConsoleLogger logger = new ConsoleLogger();
Ucet banka = new Ucet(logger);
banka.Vyber(1000000m);
```

Výhodou použití techniky Dependency Injection v tomto příkladě například je, že můžeme pro potřeby unit testu vytvořit třídu, která neloguje nikam a použít ji v testu tak, aby test neměl žádné vedlejší efekty, tedy neměnil obsah souboru pro logování. A nemusíme přitom měnit kód třídy `BankovniUcet`.

```cs 
class LoggerStub : ILogger
{
    public void Log(string text)
    {
        // neloguje nikam
    }
}

public class TestBankovniUcet
{
    [Fact]
    public void Vyber_Vse_NulovyZustatek()
    {
        LoggerStub logger = new LoggerStub();
        Ucet banka = new Ucet(logger);

        banka.Vyber(1000.0m);
        Assert.Equal(0.0m, banka.Zustatek);
    }
}
```

---
Singleton a další klasické patterny byly představeny v následující klasické knize:

[GAMMA, Erich. Design patterns: elements of reusable object-oriented software. Boston: Addison-Wesley, 1995. ISBN 0-201-63361-2](https://www.oreilly.com/library/view/design-patterns-elements/0201633612/)






