Úkoly k procvičování:
1. Úkol delegát [zadání](1_delegat_zadani.cs) [řešení](1_delegat_reseni.cs)
2. Úkol šablony delegátů [zadání](2_sablony_zadani.cs) [řešení](2_sablony_reseni.cs)
3. Úkol lambdy [zadání](3_lambdy_zadani.cs) [řešení](3_lambdy_reseni.cs)
4. Úkol event [zadání](4_event_zadani.cs) [řešení](4_event_reseni.cs)

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

Pokud metoda nevrací žádnou hodnotu a nemá žádný parametr, tak použijeme typ `Action`, například:

```cs 
Action action = VypisAhoj;
action.Invoke();

void VypisAhoj()
{
    Console.WriteLine("Ahoj");
}
```

### `Predicate` 

Je delegát metody, která vrací vždy boolean a má jeden parametr.

Například typ
```cs 
Predicate<int>
```
odpovídá delegátu 
```cs 
delegate bool Operace(int x)
```

### `Func` 

Je delegát metody, která vrací hodnotu a má více parametrů. Návratový typ je uveden jako poslední. Jde o nejobecnější z šablon delegátů. 

Například typ
```cs 
Func<int,string,bool>
```
odpovídá delegátu 
```cs 
delegate bool Operace(int a, string b)
```

## Lambda výrazy

Lambda výrazy nám umoňují zapsat anonymní funkci, tedy funkci bez jména. Používám se s polečně s delegáty. V .NET 2.0 se pro stejný účel používali anonymní metody, tyto byly ale nahrazeny v .NET 3.0 lambda výrazy a už se nepoužívají. 

V následujícím příkladu přiřazujeme referenci na metodu `VratRetezec` delegátu typu `Func`:
```cs 
string VratRetezec(int x, bool y)
{
    return $"x ma hodnotu {x} a promena y ma hodnotu {y}";
}

Func<int,bool, string> delegat = VratRetezec;

string retezec = delegat.Invoke(2, true);

Console.WriteLine(retezec);
```

Pokud bychom tuto metodu používali jen jednou, tak ji nemusíme definovat jako metodu, ale můžeme použít lambda výraz. Všimněte si, že u proměnných `x` a `y` nemusíme uvádět typ a když metoda obsahuje pouze jeden příkaz, tak nepoužíváme ani klíčové slovo `return` a složené závorky. Zápis je potom velmi úsporný.

```cs 
Func<int,bool, string> delegat = (x,y) => $"x ma hodnotu {x} a promena y ma hodnotu {y}";

string retezec = delegat.Invoke(2, true);

Console.WriteLine(retezec);
```

Lambda výrazy se často používájí s knihovnou LINQ, například pokud chceme z Listu získat pouze kladná čísla. Všimněte si, že pokud má lambda výraz jen jeden parametr tak můžeme vynechat i složené závorky. 

```cs 
List<int> cisla = new List<int> { -20, 2, 5, -2, 7, 8 };

List<int> kladna = cisla.Where(x => x > 0).ToList();
```

V následujícím příkladu si všimněte, že v lambda výrazu používáme lokální proměnnou `min` a kopírujeme jen proměnné větší než tato hodnota. Říkáme že tato proměnná je **captured** a prodlouží se její **lifetime**.

```cs 
int min = 2;
List<int> vetsi = cisla.Where(x => x > min).ToList();
```

Lambda výraz je také možné prevést na strom výrazů, což se využívá v některých knihovnách například pro objektově relační mapování, kdy se ze stromu výrázů generuje výraz v jazyce SQL.

### Statements lambda

Pokud chceme v lambda výrazu použít více příkazu, tak musíme použít statements lambda. Statements lambda nejde převést na strom výrazů.

V následujícím příkazu opět filtrujeme jen kladná čísla z Listu, ale při každém testu, zda je číslo kladné, ještě zvyšíme hodnotu počítadla a vypíšeme ji na terminál, musíme proto využít statements labmda. Všimněte si, že jsme museli použít klíčové slovo `return` a složené závorky.

```cs 
int pocitadlo = 0;

List<int> kladna = cisla.Where(x =>
{
    if(x > 0)
    {
        System.Console.WriteLine(++pocitadlo);
        return true;
    }

    return false;

}).ToList();
```

Více se o lambda výrazech můžete dozvědět například zde:
[Lambda expressions (C# reference). Microsoft Docs. 2022](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions)

TODO: event

---
Kompletní příklady:
1. [Výpis](01_vypis.cs)
2. [Operace](02_operace.cs)
3. [Predikát](03_predicate.cs)
