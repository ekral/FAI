Příklady k procvičování:
1. Generická třída [zadání](1_zadani.cs) [řešení](2_reseni)
2. Dictionary [zadání](1_zadani.cs) [řešení](2_reseni)
---

## Generické datové typy a kolekce

Generika (C#, Java) nebo šablony v C++, zmožňují odložit přesnou definici použitého datového typu v rámci datového typu, například třídy nebo rozhraní. V jazyce C se pro podobné účely používá příkaz textového preprocecoru #define.

Generika poskytují vetší znovu použitelnost kódu, zlepšuje typovou bezpečnost a celkový výkon (není nutný boxing u hodnotových typů). Nejčastější aplikace je v rámci kolekcí. Je doporučováno vždy preferovat generické třídy a metody před jejími negenerickými verzemi

V následujícím příkladu je ukázka definice generické třídy sklad, který představuje zásobník s pevnou délkou:

```cs 
class Sklad<T>
{
    T[] data;
    private int pocet;

    public Sklad(int kapacita)
    {
        data = new T[kapacita];
    }

    public void Zaloz(T objekt)
    {
        data[pocet++] = objekt;
    }

    public T Vyloz()
    {
        return data[--pocet];
    }
}
```

A při použití této třídy zvolíme konrétní typ, který se použije místo generického parametru `T`:

```cs 
Sklad<int>  skladInt = new Sklad<int>(10);
skladInt.Zaloz(1);
int celeCislo = skladInt.Vyloz();

Sklad<string> skladString = new Sklad<string>(10);
skladString.Zaloz("Ahoj");
string retezec = skladString.Vyloz();
```

### List<T>
 
Generická třída List<T> představuje implementaci dynamického pole. Kdy pole s pevnou délkou (Array) má pevně danou délku, kterou není možné změnit. A dynamické pole umožňuje přidávat a odebírat prvky do již existujícího pole protože v případě potřeby si dynamické pole alokuje víc paměti.

Instanci třídy List<int> definujeme následujícím způsobem. Po vytvoření instance neobsahuje žádné prvky.
    
```cs 
List<int> cisla = new List<int>(); 
```

Instanci třídy List<int> inicializujeme stejným způsobem jako pole, tedy seznamem prvků ve složených závorkách oddělených čárkou:

```cs 
List<int> cisla = new List<int>() { 1, 2, 3 }; 
```
    
K prvkům přistupujeme pomocí operátoru indexace `[]` nebo pomocí cyklu foreach protože List<int> implementuje rozhraní `IEnumerable`. V následujících příkladech si ukážeme nejprve použití operátoru indexace a potom cyklu `foreach`.

```cs 
List<int> cisla = new List<int>() { 1, 2, 3 };

// Prvni prvek
Console.WriteLine(cisla[0]);

// Druhy prvek
Console.WriteLine(cisla[1]);

// Treti prvek
Console.WriteLine(cisla[2]);
```    
    
```cs 
List<int> cisla = new List<int>() { 1, 2, 3 };

foreach (int cislo in cisla)
{
    Console.WriteLine(cislo);
}
```   
    
###  Dictionary<TKey,TValue> 
  
Technika Dependency Injection se používá k tomu aby jedna třída nebyla přímo závislá na jiné třídě a její konkrétní implementaci. Tato technika často používá proto aby byl kód lépe testovatelný, protože můžeme v kódu jednodušeji nakonfigurovat objekt pro potřeby testu. Technika Dependency Injection je založena na tom, že místo reference typu konkrétní třídy používáme referenci typu rozhraní a vlastní instanci potom předáváme nejčastěji v konstruktoru (jsou ale i varianty s Property nebo metodou). 

Lépe se ale tato technika chápe na konrétním příkladu. Ukážeme si příklad, kdy budeme mít třídu `Automobil` a ta bude mít field `motor` a budeme chtít při vytváření instance třídy `Automobil` zvolit, zda bude mít benzínový nebo naftový motor.
