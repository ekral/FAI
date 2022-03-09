Příklady k procvičování:
1. Dědičnost: [zadání](1_polymorfismus_zadani.cs) [řešení](1_polymorfismus_reseni.cs)
---

## Polymorfismus

Polymorfismem (mnohotvarost) rozumíme statický polymorfismus jako přetěžování metod, přetěžování operátorů a dynamický polyformismus, kdy chceme za běhu programu nahrazazovat objekt jiným kompatibilním objektem. Runtime čistě objektového jazyka smalltalk dokonce umožnoval pozastavit běžící program u zákazníka, vyměnit aktuální objekt za jiný a pokračovat v programu ve stejném stavu v jakém jsme ho přerušili.

Více se o runtime jazyka smalltalk můžete dozvědět na stránkách opensource implementace jazyka Smalltalk Pharo (nejde už již o běžně používaný jazyk):

[The immersive programming experience. Pharo, 2022]( https://pharo.org/)

Nyní si probereme dynamický polymorfismus na příkladu. Nejprve si definujeme třídy `Pejsek` a `Kocicka`:
```cs 
class Pejsek
{
    public void Zvuk()
    {
        Console.WriteLine("Haf haf");
    }
}

class Kocicka
{
    public void Zvuk()
    {
        Console.WriteLine("Mnau");
    }
}
```

Nyní bychom chtěli mít třídu Zoo, do které bychom mohli dávat jak pejsky tak kočičky. Následující řádek ale není platný, to znamená, že bychom do našeho Zoo mohli dávat jen pejsky nebo kočičky, ale ne obě zvířátka zároveň.
```cs
Pejsek x = new Pejsek();
x = Kocicka(); // nejde prelozit
```

Je to proto, že v **jazyce C#**, ale i například jazyce C++ nebo Java **není možné změnit objekt typu `Pejsek` na objekt typu `Kocicka`**, protože kompilátor vyžaduje abychom explicitně v kódu vyjádřili, že jsou vzájemně nahraditelné. Tedy že mají například stejné metody nebo property. Naproti tomu v jazyce JavaScript by to bylo možné, protože v JavaScriptu nemají proměnné pevně přiřazený typ a typová kontrola je dynamická (Dynamic typing). To znamená, že teprve až za běhu programu se v jazyce JavaScript ověří, že jak kočička, tak pejsek mají metodu `Zvuk`, někdy se tomuto postupu říká **duck typing** - tedy pokud to kváká a chodí jako kachna, tak je to kachna.

TODO: dokončit příklad
TODO: vysvětlit early a late binding

Více se pojmech statically a dynamicaly typed můžete dočíst například zde:

[What is the difference between a strongly typed language and a statically typed language? StackOverlow, 2022](https://stackoverflow.com/a/2696369)

O pojmech early a late binding se můžete dočíst v této knize na straně 66 a straně 103:

[Booch, G., 2007. Object-oriented analysis and design with applications](https://www.amazon.com/Object-Oriented-Analysis-Design-Applications-3rd/dp/020189551X/ref=sr_1_1?crid=3J6T6XIHYPCP8&keywords=Object-Oriented+Analysis+and+Design+with+Application&qid=1646832764&s=books&sprefix=object-oriented+analysis+and+design+with+application%2Cstripbooks-intl-ship%2C128&sr=1-1)

