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
