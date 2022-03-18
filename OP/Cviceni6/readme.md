Příklady k procvičování:
1. Abstrakní metody a rozhraní: [zadání](1_zadani.cs) [řešení](1_reseni.cs)
---

## Abstraktni metoda a třída

Abstraktní metoda nemá implementaci (tělo) a může být definována pouze v abstraktní třídy. Abstraktní třída slouží pouze jako rodičovská třída a v klientském kódu používáme její potomky, ale nemůžeme vytvářet její instance. Opět jde o kontrukci kterou vytváříme z důvodu statickké typové kontroly.

Nyní si projdeme příklad, kdy si nejprve vytvoříme virtuální metodu `VratZvuk`, kterou poté změníme na abstraktní.

* Nejprve si definujeme třídu `Zviratko`, ktera ma property `Jmeno` a virtuální metodu `VratZvuk`:

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
* a potom definujeme dva potomky této třídy, které překryjí s použitím klíčového slova `override` metodu `VratZvuk`. Prvním potomkem je třída `Pejsek`, která vrací zvuk "haf haf" a druhým třída `Kocicka`, která vrazí zvuk "mnau".

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
* Použití tříd je potom následující:

```cs 
Zviratko zviratko = new Zviratko() { Jmeno = "Obecne zviratko" };
Zviratko pejsek = new Pejsek() { Jmeno = "Azor" };
Zviratko kocicka = new Kocicka() { Jmeno = "Micka" };

Console.WriteLine($"{zviratko.Jmeno} dela {zviratko.VratZvuk()}"); // Obecne zviratko dela ?
Console.WriteLine($"{pejsek.Jmeno} dela {pejsek.VratZvuk()}"); // Azor dela haf haf
Console.WriteLine($"{kocicka.Jmeno} dela {kocicka.VratZvuk()}"); // Micka dela mnau
```

* Pokud se zamyslíme nad metodou `VratZvuk` ve třídě `Zviratko`, tak pro ni nemáme smysluplnou implementaci protože jde o obecné (abstraktní) zvířátko které nedělá žádný zvuk. Takové metody pro které nemáme implementaci a které musí implementovat až potomci této třídy můžeme označit jako abstraktní klíčovým slovem `abstract`. Abstraktní metody mohou mít pouze abstraktní třídy, třída `Zviratko` tedy musí být označená klíčovým slovem `abstract`. Takové třídy jsou potom určené pouze pro dědičnost a nemůžeme vytvářet jejich instance. Potomci těchto tříd potom musí povinně implementovat abstraktní metody.

```cs 
abstract class Zviratko
{
    public string Jmeno { get; set; }

    public abstract string VratZvuk();
}
```

* Použití tříd `Pejsek` a `Kocicka`a zůstává stejné, jediný rozdíl je v tom, že nemůžeme vytvářet instance třídy `Zviratko`, můžeme mít pouze referenci tohoto typu:

```cs 
Zviratko pejsek = new Pejsek() { Jmeno = "Azor" };
Zviratko kocicka = new Kocicka() { Jmeno = "Micka" };

Console.WriteLine($"{pejsek.Jmeno} dela {pejsek.VratZvuk()}"); // Azor dela haf haf
Console.WriteLine($"{kocicka.Jmeno} dela {kocicka.VratZvuk()}"); // Micka dela mnau
```
## Rozhraní
