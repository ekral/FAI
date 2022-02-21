
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
static List<int> VratCisla()
{
    List<int> cisla = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
    return cisla;
}

static void Main(string[] args)
{
    List<int> cisla = VratCisla();
}
```

TODO: reference na přednášku o referenčních typech
