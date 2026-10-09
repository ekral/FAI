# 04: Polymorfismus a virtuální metody
**autor: Erik Král ekral@utb.cz**

---


Obsah
- Polymorfismus.
- Polymorfismus a statická typová kontrola.
- Upcasting.
- Downcasting.

## Polymorfismus

Polymorfismem (mnohotvarost) rozumíme statický polymorfismus jako přetěžování metod, přetěžování operátorů a dynamický polyformismus, kdy chceme za běhu programu nahrazazovat objekt jiným kompatibilním objektem. Runtime čistě objektového jazyka smalltalk dokonce umožnoval pozastavit běžící program u zákazníka, vyměnit aktuální objekt za jiný, zaktualizovat reference a pokračovat v programu ve stejném stavu v jakém jsme ho přerušili.

- Více se o runtime jazyka smalltalk můžete dozvědět na stránkách opensource implementace jazyka Smalltalk Pharo (nejde už již o běžně používaný jazyk):

[The immersive programming experience. Pharo, 2022]( https://pharo.org/)


### Duck typing a statická typová kontrola

Nejprve si ukážeme příklad z javascriptu, funkci vypis můžeme předat cokoliv, integer, string a program na chybu příjde až za běhu. Je to proto, že v javascriptu není statická typová kontrola, tedy kontrola typů v době překladu. Program prostě zkusí za běhu (podívá se do dictionary objektu), jestli má zviratko funkci zvuk a proměnou jméno.

Výhodou je, že program je jednodušší, nevýhodou že neodhalíme případné chyby předem a nevíme jaké typy máme předávat a refraktorizace kódu jsou složitější (při změnám kódu nevíme co se pokazilo).

```javascript
class Kacenka 
{
  constructor(jmeno) 
  {
    this.jmeno = jmeno;
  }

  zvuk() 
  {
    return `${this.jmeno} dela quack`;
  }
}

class Kocicka 
{
  constructor(jmeno) 
  {
    this.jmeno = jmeno;
  }

  zvuk() 
  {
    return `${this.jmeno} dela mnau`;
  }
}

function vypis(zviratko)
{
    console.log(zviratko.zvuk());
}

const kacenka = new Kacenka("Donald");
const kocicka = new Kocicka("Mikes");

vypis(kacenka);
vypis(kocicka);
```

> Output:
>
> ```
> Donald dela quack
> Mikes dela mnau
> Mikes dela mnau
> ```

Níže je stejný příklad v jazyce C#, který statickou kontrolu má. Museli jsme vytvořit společného předka Zviratko aby kompilator vedel ze Pejsek i Kocicka mají property Jmeno a metodu Zvuk. A u metody Zvuk jsme museli označit že je virtualní nebo abstraktní, aby se rozhodovalo dle typu objektu (Pejsek nebo Kocicka) a ne typu reference (Zviratko).

```csharp
abstract class Zviratko(string jmeno)
{
    public string Jmeno {get;} = jmeno;

    abstract public string Zvuk();
}

class Pejsek(string jmeno) : Zviratko(jmeno)
{
    override public string Zvuk()
    {
        return $"{Jmeno} dela haf haf";
    }
}

class Kocicka(string jmeno) : Zviratko(jmeno)
{
    override public string Zvuk()
    {
        return $"{Jmeno} dela mnau";
    }
}

void Vypis(Zviratko zviratko)
{
    Console.WriteLine(zviratko.Zvuk());
}

Pejsek pejsek = new("Rex");
Kocicka kocicka = new("Mikes");

Vypis(pejsek);
Vypis(kocicka);
```

> Output:
>
> ```
> Rex dela haf haf
> Mikes dela mnau
> 
> ```

### Polymorfismus a statická typová kontrola

Příklad s pejskem a kočičkou si teď probereme podrobně. Nejprve si definujeme třídy `Pejsek` a `Kocicka`. V příkladech jsou vynechány konstruktory, aby byl kód kratší.

```csharp
class Pejsek
{
    public string Jmeno { get; set; }

    public void Zvuk()
    {
        Console.WriteLine("Haf haf");
    }
}

class Kocicka
{
    public string Jmeno { get; set; }

    public void Zvuk()
    {
        Console.WriteLine("Mnau");
    }
}
```

Nyní bychom chtěli mít třídu Zoo, do které bychom mohli dávat jak pejsky tak kočičky. Následující kód ale není platný, to znamená, že bychom do našeho Zoo mohli dávat jen kočičky, ale ne zvířátka různého typu do jedné zoo.

```csharp
class Zoo
{
    private List<Kocicka> zviratka = new List<Kocicka>();

    public void Pridej(Kocicka kocicka)
    {
        zviratka.Add(kocicka);
    }

    public void VypisZvuky()
    {
        foreach (var zviratko in zviratka)
        {
            zviratko.Zvuk();
        }
    }
}

Zoo zoo = new Zoo();

Pejsek rex = new Pejsek() { Jmeno = "Rex" };
Kocicka micka = new Kocicka() { Jmeno = "Micka" };

zoo.Pridej(micka);
//zoo.Pridej(rex); // nejde prelozit
```

Je to proto, že v **jazyce C#**, ale i například jazyce C++ nebo Java **není možné změnit objekt typu `Pejsek` na objekt typu `Kocicka`**. Je to proto, že jazyk C# používá statickou typovou kontrolu (Static typing), tedy kompilátor kontroluje typy v době překladu a vyžaduje abychom explicitně v kódu vyjádřili, že jsou vzájemně nahraditelné. Tedy že mají například stejné metody, property nebo fieldy. Naproti tomu v jazyce JavaScript by to bylo možné, protože v JavaScriptu nemají proměnné pevně přiřazený typ a typová kontrola je dynamická (Dynamic typing). To znamená, že teprve až za běhu programu se v jazyce JavaScript ověří, že jak kočička, tak pejsek mají metodu `Zvuk`, někdy se tomuto postupu říká **duck typing** - tedy pokud to kváká a chodí jako kachna, tak je to kachna.

- Více se pojmech statically a dynamicaly typed můžete dočíst například zde:

[What is the difference between a strongly typed language and a statically typed language? StackOverlow, 2022](https://stackoverflow.com/a/2696369)

V jazyce C#, protože má statickou typovou kontrolu, vyjádříme že jsou objekty kompatibilní buď pomocí rodičovské třídy nebo pomocí rozhraní. V následujícím kódu si nadefinujeme rodičovskou třídu `Zviratko` od ktere bude dědit jak `Pejsek`, tak Kocicka:

```csharp
class Zviratko
{
    public string Jmeno { get; set; }

    public void Zvuk()
    {
        Console.WriteLine("Jsem abstraktni zviratko a nedelam zadny konkretni zvuk");
    }
}

class Pejsek : Zviratko
{
    // Pozdeji vyresime lepe
    public void Zvuk()
    {
        Console.WriteLine("Haf haf");
    }
}

class Kocicka : Zviratko
{
    // Pozdeji vyresime lepe
    public void Zvuk()
    {
        Console.WriteLine("Mnau");
    }
}
```

### Upcasting

Nyní můžeme prostřednictví reference typu `Zviratko` nahradit pejska kočičkou a naopak. Této operaci, kdy převádíme potomka na rodiče říkáme **upcasting**.

```csharp
Zviratko z = new Pejsek() { Jmeno = "Rex" };
z = new Kocicka() { Jmeno = "Micka" };
```

A v zoo můžeme mít seznam zvířátek, do kterého můžeme dávat pejsky, kočičky a v budoucnu i všechna nová zvířátka, pokud budou potomkem třídy `Zviratko`:

```csharp
class Zoo
{
    private List<Zviratko> zviratka = new List<Zviratko>();

    public void Pridej(Zviratko zviratko)
    {
        zviratka.Add(zviratko);
    }

    public void VypisZvuky()
    {
        foreach (var zviratko in zviratka)
        {
            zviratko.Zvuk();
        }
    }
}

Zoo zoo = new Zoo();

Pejsek rex = new Pejsek() { Jmeno = "Rex" };
Kocicka micka = new Kocicka() { Jmeno = "Micka" };

zoo.Pridej(micka);
zoo.Pridej(rex);

zoo.VypisZvuky();
```

> Output:
>
> ```
> Mnau
> Haf haf
> 
> ```

### Polymorfismus, early a late binding v OOP

V minulém příkladu jsme si vytvořili seznam zvířátek do kterého jsme přidali pejska a kočičku. Pokud ale prostřednictvím reference typu `Zviratko` zavoláme metodu Zvuk, tak se nám zavolá metoda třídy Zviratko a na terminál se vypíše dvakrát text "Jsem abstraktni zviratko a nedelam zadny konkretni zvuk". Je to opět proto, že v jazyk C# používá **static typing** a o tom, která metoda se zavolá se rozhodne *v době překladu dle typu reference*. V kontextu OOP mluvíme o **early bindingu**. 


```csharp
foreach (Zviratko zviratko in zviratka)
{
    zviratko.Zvuk();
}
```

> Output:
>
> ```
> Jsem abstraktni zviratko a nedelam zadny konkretni zvuk
> Jsem abstraktni zviratko a nedelam zadny konkretni zvuk
> 
> ```

V jazyce JavaScript, protože používá dynamic typing, se o tom, která metoda se zavolá rozhoduje až za běhu programu. Proto by se zavolali správně metody pejska a kočičky. Což je to co chceme. V kontextu OOP tomu říkáme **late binding**. Pokud je late bindig očekáváné chování, proč se v jazyce C# a nebo jazyce C++ nepoužívá jako výchozí? Nepoužívá se jako výchozí z důvodu výkonu, protože rozhodování o tom, která metoda se má zavolat až za běhu programu je pomalejší, než když se o tom rozhodne je jednou hned při překladu programu.

V jazyce C# a dalších z důvodu výkonu explicitně říkáme aby používali pomalejší late bindig jen ty metody u kterých to potřebujeme. V našem příkladu označíme metodu `Zvuk` v třídě `Zviratko` jako `virtual` a třídách `Pejsek` a `Kocicka` ji označíme klíčovým slovem `override`. Říkáme, že překrýváme virtuální metodu. Tímto zápisem potomu určíme, že se má pro metodu `Zvuk` použít late binding, tedy o tom, která metoda se zavolá se rozhodne až **za běhu programu dle typu objektu**.

V následujícím kompletním příkladu máme překrytou vrituální metodu `Zvuk` a zvířátka v seznamu zvířátek už správně vypisují konkrétní zvuky, které dělají:

```csharp
class Zviratko
{
    public string Jmeno { get; set; }

    virtual public void Zvuk()
    {
        Console.WriteLine("Jsem abstraktni zviratko a nedelam zadny konkretni zvuk");
    }
}

class Pejsek : Zviratko
{
    override public void Zvuk()
    {
        Console.WriteLine("Haf haf");
    }
}

class Kocicka : Zviratko
{
    override public void Zvuk()
    {
        Console.WriteLine("Mnau");
    }
}

List<Zviratko> zviratka = new List<Zviratko>();
zviratka.Add(new Pejsek() { Jmeno = "Rex" });
zviratka.Add(new Kocicka() { Jmeno = "Micka" });

foreach (Zviratko zviratko in zviratka)
{
    zviratko.Zvuk();
}
```

> Output:
>
> ```
> Haf haf
> Mnau
> 
> ```

- O pojmech early a late binding se můžete dočíst v této knize na straně 66 a straně 103:

[Booch, G., 2007. Object-oriented analysis and design with applications](https://www.amazon.com/Object-Oriented-Analysis-Design-Applications-3rd/dp/020189551X/ref=sr_1_1?crid=3J6T6XIHYPCP8&keywords=Object-Oriented+Analysis+and+Design+with+Application&qid=1646832764&s=books&sprefix=object-oriented+analysis+and+design+with+application%2Cstripbooks-intl-ship%2C128&sr=1-1)

### Downcasting

Operaci, kdy přetypujeme potomka na rodiče říkáme upcasting. Vyjímečně ale můžeme i v kódu provést downcasting, kdy ale musíme být opatrní, protože ne každé zvířátko může být například kočička. Využíváme především operátor `is`:

```csharp
foreach (Zviratko zviratko in zviratka)
{
    if(zviratko is Kocicka kocicka)
    {
        Console.WriteLine(kocicka.Jmeno);       
    }
}
```

> Output:
>
> ```
> Micka
> 
> ```

Máme i operátor `as`, který se používal dříve. Ale zápis je složitější než s `is`.

```csharp
foreach (Zviratko zviratko in zviratka)
{
    Kocicka kocicka = zviratko as Kocicka;

    if(kocicka != null)
    {
        Console.WriteLine(kocicka.Jmeno);       
    }
}
```

> Output:
>
> ```
> Micka
> 
> ```

---
Důležité je si uvědomit, že výše zmíněné postupy se týkají především statically typed jazyků se zaměřením na výkon. Ve Smalltalku, který je dynamically typed, nebylo potřeba definovat virtuální funkce, protože všechny funkce byly jako výchozí late bind a nebylo nutné definovat rozhraní nebo rodičovskou třídu kvůli kompatibilitě objektů. Dá se říct, že OOP bylo ve smalltalku mnohem jednodušší a většina syntaxe kterou se teď učíme pochází z implementace OOP ve statically typed jazyce C++. 
