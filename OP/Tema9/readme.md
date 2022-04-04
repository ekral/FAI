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

### Šablony delegátů

Pro nejčastější typy delegátů jsou připraveny šablony delegátů. Proto není potřeba psát vlastní delegáty. 

### `Action` 

Je delegát metody který může mít více parametrů a nevrací žádnou hodnotu (návratový typ je void). 

Například typ
```cs 
Action<string>
```
odpovídá delegátu 
```cs 
delegate void Operace(string msg)
```

### `Predicate` 

Je delegát metody, která vrací vždy boolean a má jeden parametr. Například typ `Func<int,int,bool>` odpovídá delegátu `delegate bool Operace(int a, int b)`.

Například typ
```cs 
Predicate<int>
```
odpovídá delegátu 
```cs 
delegate bool Operace(int x)
```

### `Func` 

Je delegát metody, která vrací hodnotu a má více parametrů. Jde o nejobecnější z šablon delegátů. 

Například typ
```cs 
Func<int,int,bool>
```
odpovídá delegátu 
```cs 
delegate bool Operace(int a, int b)
```

## Lambda výrazy

Lambda výrazy nám umoňují zapsat anonymní funkci, tedy funkci bez jména. Používám se s polečně s delegáty. V .NET 2.0 se pro stejný účel používali anonymní metody, tyto byly ale nahrazeny v .NET 3.0 lambda výrazy a už se nepoužívají. 

---
Kompletní příklady:
1. [Výpis](01_vypis.cs)
2. [Operace](02_operace.cs)
3. [Predikát](03_predicate.cs)
