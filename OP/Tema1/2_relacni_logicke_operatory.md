Test 2 relační a logické operátory - příprava
---
Pro zvládnutí druhého testu potřebujete znát typ `bool` a znát relační a logické operátory . Nezapomeňte, že opět záleží na prioritě operátorů a pro jistotu používejte kulaté závorky `()`.

Na následujících příkladech si probereme relační a logické operátory. 

Nejprve si ale definujme tři proměnné, celá čísla `x`, `y` a booleanovskou proměnnou `vysledek`:
```cs 
int x = 2;
int y = 3;
bool vysledek;
```
* Měli byste znát následující relační operátory, které vracejí jako výsledek operace typ `bool`:
```cs 
vysledek = x == y;  // rovnost
vysledek = x != y;  // nerovnost
vysledek = x < y;   // mensi nez
vysledek = x > y;   // vetsi nez
vysledek = x <= y;  // nensi nebo rovno
vysledek = x >= y;  // vetsi nebo rovno
```
* Dále byste měli znát logické operace and `&&` , or `||` a negace `!`
```cs 
vysledek = (x < y) && (y == 3); // pravda pokud je x menší než y a zárověň y je rovno 3
vysledek = (x < y) || (y == 2); // pravda pokud je x menší než y a nebo y je rovno 3
vysledek = !(x < y); // operator ! neguje vysledek predchozi operace, vyraz je pravda, pokud je x vetsi nebo rovno y

```

---
V následujících kódech je uvedený kompletní příklad a řešení příkladů ze cvičení.

- Přehled operátorů

```cs 
using System;

namespace MujDruhyProjekt
{
    class Program
    {
        static void Main(string[] args)
        {
            int x = 2;
            int y = 3;

            bool vysledek;
            
            vysledek = x == y;  // rovnost
            vysledek = x != y;  // nerovnost
            vysledek = x < y;   // mensi nez
            vysledek = x > y;   // vetsi nez
            vysledek = x <= y;  // nensi nebo rovno
            vysledek = x >= y;  // vetsi nebo rovno

            vysledek = (x < y) && (y == 3); // pravda pokud je x menší než y a zárověň y je rovno 3
            vysledek = (x < y) || (y == 2); // pravda pokud je x menší než y a nebo y je rovno 3
            vysledek = !(x < y); // operator !neguje vysledek predchozi operace, vyraz je pravda, pokud je x vetsi nebo rovno y

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
```

- Test zda trojúhelník zadaný délkami stran existuje a je pravoúhlý

```cs 
using System;

namespace MujDruhyProjekt
{
    class Program
    {
        static void Main(string[] args)
        {
            // Mame trojuhelnik dany delkami trech strank
            double a = 3.0;
            double b = 4.0;
            double c = 5.0;

            bool vysledek;

            // Napiste vyraz ktery vrati true, kdyz je trojuhelnik pravouhly, musi platit a^2 + b^2 = c^2 
            vysledek = a * a + b * b == c * c; // poznamka, diky chybam v zaukrouhlovanim nam v nekterych pripadech nemusi rovnost vychazet

            Console.WriteLine($"Je trojuhelnik pravouhly (True ano, False ne): {vysledek}");

            // Napiste vyraz ktery vrati true, kdyz trojuhelnik existuje, soucet delek dvou libovolnych stran je vzdy vetsi nez delka treti
            vysledek = (a + b > c) && (a + c > b) && (b + c > a);

            Console.WriteLine($"Existuje trojuhelnik (True ano, False ne): {vysledek}");

            Console.ReadKey();
        }
    }
}
```

- Logické výrazy jakým způsobem student splnit dva testy

```cs 
using System;

namespace MujDruhyProjekt
{
    class Program
    {
        static void Main(string[] args)
        {
            // Mame vysledky dvou testu t1 a t2
            double t1 = 40.0;
            double t2 = 70.0;

            Console.WriteLine($"test1: {t1} test2: {t2}");
            
            bool vysledek;

            // Napiste vyraz ktery vrati true, pokud student splnil alespon jeden ze dvou testu za vice nez 45 bodu
            vysledek = t1 > 45.0 || t2 > 45.0;

            Console.WriteLine($"Splnil alespon jeden test (True ano, False ne): {vysledek}");

            // Napiste pvyraz ktery vrati true, pokud student nesplnil zadny ze dvou testu za vice nez 45 bodu
            vysledek = t1 <= 45.0 && t2 <= 45.0;

            Console.WriteLine($"Nesplnil zadny test (True ano, False ne): {vysledek}");

            // Napiste vyraz ktery vrati true, pokud student splnil kazdy ze dvou testu za vice nez 45 bodu
            vysledek = t1 > 45.0 && t2 > 45.0;

            Console.WriteLine($"Splnil oba testy (True ano, False ne): {vysledek}");

            // Prepiste vyraz ktery vrati true, pokud je alespon jeden ze dvou testu za vice nez 45 bodu s pouzitim negace a AND
            vysledek = !(t1 <= 45.0 && t2 <= 45.0); // Otestujeme zda nebyl napsany zadny test a vysledek potom znegujeme

            Console.WriteLine($"Neplati ze nesplnil oba testy (True ano, False ne): {vysledek}");

            Console.ReadKey();
        }
    }
}
```

- Logické výrazy jakým způsobem student splnit tři testy


```cs 
using System;

namespace MujDruhyProjekt
{
    class Program
    {
        static void Main(string[] args)
        {
            // Mame vysledky tri testu t1 a t2
            double t1 = 30.0;
            double t2 = 40.0;
            double t3 = 70.0;

            Console.WriteLine($"test1: {t1}, test2: {t2}, test3: {t3}");
            bool vysledek;

            // Napiste vyraz ktery vrati true, pokud student splnil alespon jeden ze tri testu za vice nez 45 bodu
            vysledek = (t1 > 45.0) || (t2 > 45.0) || (t3 > 45.0);

            Console.WriteLine($"Splnil alespon jeden test ze tri (True ano, False ne): {vysledek}");

            // Napiste vyraz ktery vrati true, pokud student splnil vsechny tri testy, kazdy za vice nez 45 bodu
            vysledek = (t1 > 45.0) && (t2 > 45.0) && (t3 > 45.0);

            Console.WriteLine($"Splnil vsechny tri testy (True ano, False ne): {vysledek}");

            // Napiste vyraz ktery vrati true, pokud student splnil alespon dva ze tri testu za vice nez 45 bodu
            vysledek = ((t1 > 45.0) && (t2 > 45.0)) || ((t1 > 45.0) && (t3 > 45.0)) || ((t2 > 45.0) && (t3 > 45.0));

            Console.WriteLine($"Splnil alespon dva testy ze tri (True ano, False ne): {vysledek}");

            Console.ReadKey();
        }
    }
}
```
