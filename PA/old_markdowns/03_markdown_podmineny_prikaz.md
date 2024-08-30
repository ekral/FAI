## 3. Vývojový diagram a podmíněný příkaz

TODO: doplnit vývojové diagramy

Pro zvládnutí předmětu potřebujete znát podmíněné příkazy `if`,  `if-else` a zřetězení podmínek pomocí `else if`. Dále potřebujete vědět jak podmínit provedení jednoho příkazu nebo bloku příkazů pomocí složených závorek `{}`.

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

- Kvadratická rovnice

```cs 
using System;

namespace Priklady
{
    class Program
    {
        static void Main(string[] args)
        {
            // (x - 3)(x - 3) = 1x^2 -6x + 9
            // (x - 2)(x - 3) = 1x^2 -5x + 6
            // (x - x1)(x - x2) = (ax^2 + bx + c)

            double a = 5.0;
            double b = -6.0;
            double c = 9.0;

            // napiste podminku ze a neni 0
            // pokud neni, tak pokracujte ve vypoctu
            // jinak napiste ze rovnice neni kvadraticka

            if (a != 0.0)
            {
                double D = (b * b) - (4 * a * c);

                // napiste podminku, jestli je 
                // D vetsi nez 0, rovno 0 a nebo mensi nez 0 
                if (D > 0)
                {
                    double v = Math.Sqrt(D);

                    double x1 = (-b + v) / (2 * a);
                    double x2 = (-b - v) / (2 * a);

                    Console.WriteLine($"x1 = {x1} x = {x2}");
                }
                else if (D == 0)
                {
                    double x = (-b) / (2 * a);

                    Console.WriteLine($"x1 = {x}");
                }
                else
                {
                    Console.WriteLine("Nema reseni v oboru realnych cisel");
                }
            }
            else
            {
                Console.WriteLine("rovnice neni kvadraticka");
            }
        }
    }
}
```

- Vztahy třech čísel

```cs 
using System;

namespace Priklady
{
    class Program
    {
        static void Main(string[] args)
        {
            // Mame tri promenne
            int a = 2;
            int b = 5;
            int c = 7;

            // Vypiste hodnotu nejvetsi promenne

            // Reseni s logickym operatorem &&
            if ((a > b) && (a > c))
            {
                Console.WriteLine(a);
            }
            else if (b > c)
            {
                Console.WriteLine(b);
            }
            else
            {
                Console.WriteLine(c);
            }

            // reseni pouze s podminenym prikazem
            if (a > b)
            {
                if (a > c)
                {
                    Console.WriteLine(a);
                }
                else
                {
                    Console.WriteLine(c);
                }
            }
            else
            {
                if (b > c)
                {
                    Console.WriteLine(b);
                }
                else
                {
                    Console.WriteLine(c);
                }
            }

            // Bonus:
            // Seradte hodnoty trech promennych
            // Postupne prohazujeme hodnoty promennych
            if (a < b)
            {
                int tmp = a;
                a = b;
                b = tmp;
            }

            if (b < c)
            {
                int tmp = b;
                b = c;
                c = tmp;
            }

            if (a < b)
            {
                int tmp = a;
                a = b;
                b = tmp;
            }
        }
    }
}
```
