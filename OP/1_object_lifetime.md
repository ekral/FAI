A2OPN Příprava k testu 8 - statické prvky
---
Pro zvládnutí testu potřebujete vědět co jsou to statické členské prvky - fieldy, metody a property a jak se s takovými prvky pracuje. Ke statickým prvkům se přistupuje pomocí jména třídy ne pomocí instance třídy. Statické metody a property mohou přistupovat jen ke statickým prvkům. V následujících příkladech si probereme příklad na statickou metodu.

* Nejprve si definujeme třídu `Vypocet` a v ní statickou metodu `Soucet`:
```cs 
class Vypocty
{
    public static int Soucet(int x, int y)
    {
        return x + y;
    }
}
```
* Tuto metodu potom zavoláme pomocí jména třídy `Vypocty.Soucet` a nevytváříme instanci třídy:

```cs 
static void Main(string[] args)
{
    int vysledek = Vypocty.Soucet(2, 3);
}
```

* Samotná třída může být také označená klíčovým slovem `static`, taková třída potom může obsahovat pouze statické prvky. Příkladem takové třídy ve frameworku .NET je třída `Math`.

```cs 
static class Vypocty
{
    public static int Soucet(int x, int y)
    {
        return x + y;
    }
}
```

* Kromě metod mohou být statické i property nebo fieldy. V následujícím příkladu máme třídu `Data` která má statickou propertu `Id`, kterou zvyšujeme ve statické metodě `ZvysId`. Statický může být i konstruktor, ve kterém můžeme inicializovat statické členské prvky, statický konstruktor nemá žádné parametry.

```cs 
static class Data
{
    public static int Id { get; private set; }

    static Data()
    {
        Id = 1;
    }
    
    public static void ZvysId()
    {
        ++Id;
    }
}
```

* Použití třídy `Data` je potom následující:

```cs 
static void Main(string[] args)
{
    Console.WriteLine(Data.Id); // Vypise 1
    Data.ZvysId(); 
    Console.WriteLine(Data.Id); // Vypise 2
}
```
---
Následuje kompletní příklad.
