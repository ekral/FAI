
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

Objekt na haldě alokovaný pomocí operátoru `new` existuje od své alokace po uvolnění paměti. V jazyce C# uvolňuje tuto paměť automaticky Garbagge Collecor potom co zjistí, že na objekt na haldě už není žádná reference. Naproti tomu, například v jazyce C nebo C++ musíme paměť uvolňovat manuálně pomocí příkazu `free` respektive `delete`

TODO: reference na přednášku o referenčních typech
