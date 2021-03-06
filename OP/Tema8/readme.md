Příklady k procvičování:
1. Generická třída [zadání](1_zadani.cs) [řešení](2_reseni.cs)
2. Dictionary [zadání](2_zadani.cs) [řešení](2_reseni)
---

## Generické datové typy a kolekce

Generika (C#, Java) nebo šablony v C++, zmožňují odložit přesnou definici použitého datového typu v rámci datového typu, například třídy nebo rozhraní. V jazyce C se pro podobné účely používá příkaz textového preprocecoru #define.

Generika poskytují vetší znovu použitelnost kódu, zlepšuje typovou bezpečnost a celkový výkon (není nutný [boxing](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing) u hodnotových typů). Nejčastější aplikace je v rámci kolekcí. Je doporučováno vždy preferovat generické třídy a metody před jejími negenerickými verzemi

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

### Generic Constraints

Pomocí Generic Constraints můžeme omezit jaké typy můžeme použít pro generický parametr a tím také rozšířit operace, které s generickým typem můžeme provádět. V následujícím příkladu jsme omezili generický typ `T` třídy `Sklad<T>` na třídu `Zviratko` a její potomky. Díky tomu můžeme v metodě `NajdiPodleJmena` použít property `Jmeno` a vyhledat zvířátko podle jména. 

```cs 
Sklad<Zviratko> zviratka = new Sklad<Zviratko>(10);

zviratka.Zaloz(new Pejsek("Rex"));
zviratka.Zaloz(new Pejsek("Fik"));
zviratka.Zaloz(new Pejsek("Zeryk"));

Zviratko? zviratko = zviratka.NajdiPodleJmena("Fik");

class Sklad<T> where T : Zviratko
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

    public T? NajdiPodleJmena(string jmeno)
    {
        return data.FirstOrDefault(x => x.Jmeno == jmeno);
    }
}

abstract class Zviratko
{
    public string Jmeno { get; set; }
    public abstract string Zvuk();

    protected Zviratko(string jmeno)
    {
        Jmeno = jmeno;
    }
}

class Pejsek : Zviratko
{
    public Pejsek(string jmeno) : base(jmeno)
    {
    }

    public override string Zvuk()
    {
        return "Haf haf";
    }
}
```

Více se o možnostech o generice a Generic Constraints můžete dozvědět například zde:

[Generic classes and methods. 2022](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/types/generics)

[Constraints on type parameters (C# Programming Guide). 2022](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters)


### Dynamické pole List
 
Generická třída List<T> představuje implementaci dynamického pole. Kdy pole s pevnou délkou (Array) má pevně danou délku, kterou není možné změnit. Dynamické pole umožňuje přidávat a odebírat prvky do již existujícího pole protože v případě potřeby si dynamické pole alokuje víc paměti.

Instanci třídy List<int> definujeme následujícím způsobem. Po vytvoření instance neobsahuje žádné prvky.
    
```cs 
List<int> cisla = new List<int>(); 
```

Prvky inicializujeme stejným způsobem jako pole, tedy seznamem prvků ve složených závorkách oddělených čárkou:

```cs 
List<int> cisla = new List<int>() { 1, 2, 3 }; 
```
    
K prvkům přistupujeme pomocí operátoru indexace `[]` nebo pomocí cyklu foreach protože List<int> implementuje rozhraní `IEnumerable`. V následujících příkladech si ukážeme nejprve použití operátoru indexace a potom cyklu `foreach`.

```cs 
// Prvni prvek
Console.WriteLine(cisla[0]);

// Druhy prvek
Console.WriteLine(cisla[1]);

// Treti prvek
Console.WriteLine(cisla[2]);
```    
    
```cs 
foreach (int cislo in cisla)
{
    Console.WriteLine(cislo);
}
```   

V následujících příkladech projdeme základní operace s polem:

```cs 
List<char> znaky = new List<char>() { 'a', 'b', 'c' };

znaky.Add('x'); // Vložení na konec
znaky.Insert(1, 'x'); // Vložení na libovolnou pozici
znaky.Insert(0, 'x'); // Vložení na začátek
znaky.RemoveAt(1); // Odebrání prvku z indexu
znaky.Remove('b'); // Odebrání prvků dle hodnoty
znaky.Clear(); // Odebrání všech prvků
```   

`List<T>` je třída a tedy referenční typ, přiřazením se zkopíruje reference, která odkazuje na stejné data v paměti.

```cs 
List<char> znaky = new List<char>() { 'a', 'b', 'c’ };

List<char> kopie = znaky;
```   

Hlubokou kopii instance třídy `List<T>` můžeme vytvořit předáním původního listu jako argumentu konstruktoru. V příkladu vytváříme hlubokou kopii instance třídy List<int> ale pokud by jako prvky byly referenční typy, tak kopie jednotlivých prvků by opět byly jen reference na stejný objekt.

```cs 
List<char> znaky = new List<char>() { 'a', 'b', 'c‘ };

List<char> kopie = new List<char>(znaky);
```   
    
###  Asociativní pole Dictionary
  
Obyčejné pole ukládá pouze hodnoty. Asociativní pole ukládá dvojici klíč a hodnota. Díky klíči je potom možné velmi rychle vyhledávat vložené hodnoty. Díky ukládání klíče zabírá tento kontejner více paměti.

Instanci třídy Dictionary<TKey,TValue> definujeme následujícím způsobem. Po vytvoření instance neobsahuje žádné prvky. 

```cs 
Dictionary<string, Student> studenti = new Dictionary<string, Student>();
```

Prvky inicializujeme například následujícím způsobem kdy klíč je uvedený v hranatých závorkách a je mu přiřazená hodnota operátorem přiřazení: 
 
```cs 
Dictionary<string, Student> studenti = new Dictionary<string, Student>()
{
    ["A100"] = new Student("Jiri"),
    ["A200"] = new Student("Jiri"),
    ["A300"] = new Student("Jiri")
};
```
    
Nebo starším zápisem, kdy každý záznam je uvedený ve složených závorkách jako pár klíč hodnota oddělený čárkou:
    
```cs 
Dictionary<string, Student> studenti = new Dictionary<string, Student>()
{
    { "A100", new Student("Jiri") },
    { "A200", new Student("Jiri") },
    { "A300", new Student("Jiri") }
};
```
    
K prvkům přistupujeme pomocí indexeru v hranatých závorkách. V případě, že klíč neexistuje, tak metoda vyvolá výjimku.
    
```cs 
try
{
    Student student = studenti["A200"];
}
catch (KeyNotFoundException)
{
    Console.WriteLine("Klíč neexistuje");
}
```
    
Nebo můžeme použít metodu `TryGet`, která představuje bezpečnější a rychlejší způsob, protože nevyvolává výjimku:

```cs 
bool exituje = studenti.TryGetValue("A200", out Student student);
           
if(!exituje)
{
    Console.WriteLine("Klíč neexistuje");
}
```

`Dictionary` můžeme také procházet pomocí cyklu `foreach` a to jak zvlášť hodnoty, klíče nebo pár klíč a hodnota.
    
```cs 
foreach (Student student in studenti.Values)
{
    Console.WriteLine(student.Jmeno);
}

foreach (string key in studenti.Keys)
{
    Console.WriteLine(key);
}
        
foreach (KeyValuePair<string,Student> zaznam in studenti)
{
    Console.WriteLine($"{zaznam.Key}: {zaznam.Value.Jmeno}");
}
```

Prvek na konec listu vložíme pomocí metody `Add`. Parametry jsou klíč a hodnota prvku. Pokud vložíme již jednou existující klíč, tak metoda vyvolá výjímku.

```cs 
try
{
    studenti.Add("A100", new Student("Katerina"));
}
catch (ArgumentException)
{
    Console.WriteLine("Prvek se zadaným klíčem už existuje");
}
```
    
Před přidáním prvku můžeme otestovat, že klíč existuje pomocí metody `ContainsKey`.

```cs 
if(!studenti.ContainsKey("A100"))
{
    studenti.Add("A100", new Student("Katerina"));
}
else
{
    Console.WriteLine("Prvek se zadaným klíčem už existuje");
}
```
    
Prvek také můžeme vložit pomocí metody `TryAdd`. Metoda vrátí false, pokud se vložení nepovede.

```cs 
if (!studenti.TryAdd("A100", new Student("Katerina")))
{
    Console.WriteLine("Prvek se zadaným klíčem už existuje");
}
```
    
Prvek potom odstraníme například pomocí metody `Remove`:
    
```cs 
if (studenti.ContainsKey("A100"))
{
    studenti.Remove("A100");
}
```
    
---

Více se o různých typech kolekcích v jazyce C# dozvíte například zde:

[Collections (C#). Microsoft Docs. 2022](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/collections)
    
