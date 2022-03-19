Test 3 podmíněný příkaz - příprava
---
Pro zvládnutí druhého testu potřebujete znát podmíněné příkazy `if`,  `if-else` a zřetězení podmínek pomocí `else if`. Dále potřebujete vědět jak podmínit provedení jednoho příkazu nebo bloku příkazů pomocí složených závorek `{}`.

Na následujících příkladech si probereme jednotlivé příkazy. 

Nejprve si ale definujme jednu proměnnou, číslo s desetinnou čárkou `x`:
```cs 
double x = 9.0;
```
* S pomocí příkazu `if` můžeme podmínit provedení jiného příkazu tím zda je pravivý výraz v podmínce. Výpis na konzoli se provede jen pokud výraz `x > 0` vrátí `true`:
```cs 
if(x > 0.0)
    Console.WriteLine("x je vetsi nez 0");
```
* S použitím příkazu `if-else` můžeme ve větvi else uvést přikaz který se povede pokud je výraz v pomínce `false`.
```cs 
if(x > 0.0)
    Console.WriteLine("x je vetsi nez 0");
else
    Console.WriteLine("x je mensi nebo rovno 0");
```
* Pokud chceme podmínit provedení celého bloku příkazu, tak místo jednoho příkazu můžeme použít celý blok příkazu ve složených závorkách `{}`. Narozdíl od Pythonu zde nezáleží na odsazení příkazů, které je zde jen pro přehlednost. Většinou používáme blok příkazů i pro jeden příkaz, protože pak může být kód v některých případech přehlednější.

```cs 
if (x >= 0.0)
{
    double odmocnina = Math.Sqrt(x);
    Console.WriteLine($"Druha odmocnina x je {odmocnina}");
}
else
{
    Console.WriteLine("x je mensi nez 0");
}
```
* Podmínky můžeme dále zřetězci pokud za příkaz `else` uvedeme další `if`. Poslední else se potom vztahuje k posledním příkazu `if`.

```cs 
if (x > 0.0)
{
    Console.WriteLine("x je vetsi nez 0");
}
else if(x == 0.0)
{
    Console.WriteLine("x je rovno 0");
}
else
{
    Console.WriteLine("x je mensi nez 0");
}
```
---
V následujících kódech je uvedený kompletní příklad a řešení příkladů ze cvičení.

- Vztahy dvou čísel

```cs 
using System;

namespace Priklady
{
    class Program
    {
        static void Main(string[] args)
        {
            // mame dve promenne
            int x = 2;
            int y = 5;
           
            // vypiste hodnotu vetsi z promennych
            if(x > y)
            {
                Console.WriteLine(x);
            }
            else
            {
                Console.WriteLine(y);
            }

            // seradte hodnoty v promennych tak aby v promenne x bylo vetsi cislo nez v promenne x
            if(x < y)
            {
                int tmp = x;
                x = y;
                y = tmp;
            }
        }
    }
}
```

- using System;

```cs 

```

- using System;

```cs 

```

- using System;

```cs 

```

- using System;

```cs 

```

- using System;

```cs 

```
