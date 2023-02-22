Příklad k procvičování:
1. Dispose: [zadání](1_zadani.cs) [řešení](1_reseni.cs)

---

# Životní cyklus objektu

Životní cyklus objektu (lifetime) představuje čas mezi vytvoření a zničením objektu. 

## Lokální proměnná

Lokální proměnnná je alokovaná na zásobníku a existuje od své definice do konce bloku definovaného složenými závorkami. 

V následujícím příkladu existuje proměnná `x` od své definice do konce Metody `Main`.

```cs 
static void Main(string[] args)
{
    int x = 1;

    if (x > 0)
    {
        Console.WriteLine(x);
    }
}
```

V dalším příkladu je proměnná `x` definovaná uvnitř metody `if` a proto přestane existovat a bude zničena na konci bloku podmíněného příkazu `if`.

```cs 
static void Main(string[] args)
{
    if (true)
    {
        int x = 1;
        Console.WriteLine(x);
    }
}
```

## Statická proměnná

Statická proměnná (field) existuje po celou dobu běhu programu.

V následujícím příkladu je proměnná `x` definovaná jako statická, znamená to, že bude existovat po celou dobu běhu programu.

```cs 
class Program
{
    private static int x;

    static void Main(string[] args)
    {
        Console.WriteLine(x);
    }
}
```

## Objekt alokovaný na haldě

Objekt na haldě alokovaný pomocí operátoru `new` existuje od své alokace po uvolnění paměti. V jazyce C# uvolňuje tuto paměť automaticky Garbage Collector potom co zjistí, že na objekt na haldě už není žádná reference. Naproti tomu, například v jazyce C nebo C++ musíme paměť uvolňovat manuálně pomocí příkazu `free` respektive `delete`.

V následujícím příkladu alokujeme dynamické pole čísel na haldě pomocí operátoru `new` a referenci si uložíme do lokální proměnné `list`. Lokální proměnná `list` sice přestane existovat na konci metody `VratCisla`, ale vrátí svoji hodnotu, tedy referenci na pole čísel a přiřadí ji proměnné `cisla` v metodě `Main`. Teprve až přestane existovat proměnná `cisla` v metodě `Main`, tak už nebude existovat žádná reference na pole na haldě a Garbage Collector automaticky uvolní alokovanou paměť.

```cs 
static int[] VratCisla()
{
    int[] cisla = new int[] { 1, 2, 3, 4, 5, 6, 7 };
    return cisla;
}

static void Main(string[] args)
{
    int[] cisla = VratCisla();
}
```
Naproti tomu, v **jazyce C** bychom museli paměť uvolnit manuálně pomocí funkce `free`. V následujícím příkladu alokujeme pole čísel na haldě pomocí funkce `malloc` a potom jej uvolňíme pomocí funkce `free`.

```c
static int* vrat_cisla()
{
    int* cisla = malloc(3 * sizeof(int));

    cisla[0] = 1;
    cisla[1] = 1;
    cisla[2] = 1;

    return cisla;
}

int main()
{
    int* cisla = vrat_cisla();

    free(cisla);
}
```
---
Více o problematice programovacích jazyků se můžete dozvědět napřílad v této knize:

[Michael L. Scott. 2009. Programming Language Pragmatics](https://www.cs.rochester.edu/~scott/pragmatics/)

Více se o Garbage Collectoru v jazyce C# dozvíte zde:

[Fundamentals of garbage collection, 2022](https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals)


# Statické prvky

Ke statickým prvkům se přistupuje pomocí jména třídy a ne pomocí instance třídy. Statické metody a property mohou přistupovat jen ke statickým prvkům. V následujících příkladech si probereme příklad na statickou metodu.

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
Více se o statických prvcích v jazyce C# dozvíte zde:

[Static Classes and Static Class Members - C# Programming Guide, 2022](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-classes-and-static-class-members)


# Použití metody Dispose

Dispose Pattern slouží k bezpečnému uvolnění zdrojů (paměti, připojení k serveru, k databázi atd.) i v případě výjimky.
V jazyce C++ ke stejnému účelu používáme destruktor, který se zavolá automaticky předtím, než se uvolní objekt z paměti.
V jazyce C# sice máme destruktor také, ale nevíme kdy přesně se zavolá, protože nevíme kdy Garbage Collector uvolní paměť objektu. 

Uvolnění zdrojů se provádí voláním metody `Dispose` z rozhraní `IDisposable`. A ke správnému volání této metody slouží příkaz `using`.

Nejprve si ukážeme příklad volání metody Dispose bez příkazu `using`. V následujícím příkazu pomocí třídy `HttpClient` zavoláme webovou službu a zobrazíme získaný řetězec. Blok finally se zavolá vždy, i když dojde nebo nedojde k výjimce při volání metody `GetStringAsync`. 

```cs 
static async Task Main(string[] args)
{
    string url = "https://geek-jokes.sameerkumar.website/api?format=json";

    HttpClient client = new HttpClient();

    try
    {
        string jsonString = await client.GetStringAsync(url);
        Console.WriteLine(jsonString);
    }
    finally
    {
        client.Dispose();
    }
}
```

A nyní použijeme klíčové slovo `using`, jehož syntaxe je jednoduší a zajistí, že i v případě výjimky bude zavolaná metoda Dispose a bude řádně ukončeno připojení k serveru. 

```cs 
static async Task Main(string[] args)
{
    string url = "https://geek-jokes.sameerkumar.website/api?format=json";

    using (HttpClient client = new HttpClient())
    {
        string jsonString = await client.GetStringAsync(url);
        Console.WriteLine(jsonString);
    }
}
```

Příklad s třídou `StreamWriter` si můžete vyzkoušet na stránkách SharpLab, které zobrazují vygenerovaný kód kompilátorem:
[Sharplab.io](https://sharplab.io/#v2:CYLg1APgAgTAjAWAFBQMwAJboMLoN7LpGYZQAs6AKgKYDOALgBQCUhxBSxXmADOo1DgA6AJIB5IQGV6AJ2oBDALYB1GQEt61GegDu6zdoC86AHbUdmYeKmyFK/VsYAiTQA96Q+u6cAadPIAHAOoTUHRZAFdqZlZObiIONniuPQ0tIVU0gBk1M2cAQQALAHsAKydmAG4k7gBfGvqkWqA=)

---
Více o metodě dispose najdete například zde:

[IDisposable.Dispose Method, 2022](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable.dispose?view=net-6.0)
