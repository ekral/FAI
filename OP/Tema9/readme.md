## Delegát

Delegát je uživatelsky definovaný typ představující jednu nebo více referencí na metody s konkrétním návratovým typem a parametry. Instanci delegáta můžeme tedy přiřadit reference na metody a tyto metody potom prostřednictvím této instance zavolat (říkáme také vyvolat - invoke). Pomocí delegátů můžeme předávat reference na metody jako argumenty jiným metodám.

* Nejprve si deklarujeme typ delegáta, což je prakticky hlavička metody s klíčovým slovem `delegate`:
```cs 
delegate void MujDelegat(int x);
```

* Delegát je referenční typ a proměnnou typu delegát vytvoříme následujícím způsobem. Pokud jí přiřadíme hodnutu `null`, tak nebude mít zatím referenci na žádnou metodu.
```cs 
MujDelegat d = null;
```

* Nyní si definujeme metodu `Vypis`, kterou budeme používat v dalších příkladech:
```cs 
static void Vypis(int x)
{
    Console.WriteLine(x);
}
```

* Proměnné `d` teď přiřadíme referenci na metodu `Vypis`, která stejně jako typ `MujDelegat` má navratový typ `void` a jeden parametr typu `int`. Příkaz `MujDelegat d = Vypis;` by bylo možné také zapsat delším způsobem `MujDelegat d = new MujDelegat(Vypis);`.
```cs 
MujDelegat d = Vypis;
```

* Prostřednictvím proměnné `d` teď můžeme vyvolat metodu `Vypis`. Nejkratší výraz pro vyvolání metody je `d(3)` který odpovídá výrazu `d.Invoke(3)`, častěji ale používáme výraz `d?.Invoke(3)`, který zavolá metodu `Invoke`, pouze pokud proměnná `d` nemá hodnotu `null`. 
```cs 
d(3); 
d.Invoke(3); 
d?.Invoke(3);
```

* V předchozím příkazu jsme proměnné `d` přiřadili referenci jen na jednu metodu. S pomocí operátoru `+=` můžeme přiřadit proměnné `d` reference na více metod. Příkaz `d?.Invoke(3);` potom zavolá jak metodu `VypisA` tak metodu `VypisB`.
```cs 
static void VypisA(int x)
{
    Console.WriteLine($"A {x}");
}

static void VypisB(int x)
{
    Console.WriteLine($"B {x}");
}

static void Main(string[] args)
{
    MujDelegat d = null;

    d += VypisA;
    d += VypisB;

    d?.Invoke(3);
}
```
* Operátor `-=` potom referenci z proměnné `d` odstraní. V kódu je potřeba dávat pozor abychom nezapoměli odstraňovat reference na metody, pokud je již nepotřebujeme, protože reference na metody se neodstraňují automaticky.
```cs 
MujDelegat d = null;

d += VypisA;
d += VypisB;

d?.Invoke(2); // vypise A 2 a B 2

d -= VypisA;

d?.Invoke(3); // vypise jen B 3
```

---
Následuje kompletní příklad.
1. [Výpis](01_vypis.cs)
