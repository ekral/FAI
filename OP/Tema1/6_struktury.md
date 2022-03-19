## 6. Struktury

Pro zvládnutí předmětu potřebujete vědět jak deklarovat strukturu, definovat promenou typu struktura, inicializovat vychozi hodnoty a pracovat s prvky struktury (fields). 

Na následujících příkladech si probereme jednotlivé příkazy. 

* Strukturu deklarujeme pomocí klíčového slova `struct`. Pokud chceme přistupovat k fieldům mimo strukturu, tak je musíme deklarovat jako `public`. Následující příklad definuje strukturu pro dvourozměrný bod s public fieldy `X` a `Y`.
```cs 
struct Bod
{
    public double X;
    public double Y;
}
```
* Proměnnou typu struktura definujeme stejným způsobem jako zabudované typy. V následujícím příkladu definujeme proměnnou `b1` typu `Bod`.
```cs 
Bod b1; 
```
* Výchozí hodnoty struktury získáme pomocí zápisu `new Bod()`. Pozor, u struktur nejde o alokaci paměti na haldě, jen o získání objektu s inicializovanými výchozími hodnotami fieldů. 
```cs 
Bod b1 = new Bod();
Console.WriteLine($"Vychozi hodnoty jsou {b1.X} a {b1.Y}");
```
* K fieldům potom přistupujeme pomocí operátoru přímého přístupu `.`. Každý field musí mít před prvním použitím přiřazenou hodnotu.
```cs 
Bod b1;
b1.X = 2;
b1.Y = 3;

Console.WriteLine($"Bod b1 ma souradnice {b1.X} a {b1.Y}");
```
* Stejně jako u zabudovaných typů se při přiřazení hodnoty kopíruje hodnota v paměti. Relační, aritmetické a další operátory bychom ale museli sami definovat.

```cs 
Bod b1;
b1.X = 2;
b1.Y = 3;

Bod b2 = b1;
```

---
V následujících kódech je uvedený kompletní příklad a řešení vybraných příkladů ze cvičení.

- Příklad na strukturu Bod

```cs 
using System;

namespace Test
{
    struct Bod
    {
        public double X;
        public double Y;
    }
    class Program
    {
        static void Main(string[] args)
        {
            Bod b1;
            b1.X = 2;
            b1.Y = 3;

            Bod b2 = b1;

            b1.X = 0;
            b1.Y = 0;

            Console.WriteLine($"Bod b1 ma souradnice {b1.X} a {b1.Y}");
            Console.WriteLine($"Bod b2 ma souradnice {b2.X} a {b2.Y}");
        }
    }
}
```
