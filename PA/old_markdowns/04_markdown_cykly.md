## 4. Cykly

Pro zvládnutí předmětu potřebujete znát příkazy `goto`, `while`,  `do-while` a `for`. A dále ukončení cyklu pomocí `break` a přeskočení zbytku cyklu pomocí `continue`

Na následujících příkladech si probereme jednotlivé příkazy. 

* S pomocí příkazu `goto` můžeme přeskočit na libovolný řádek. Tento příkaz se používá spíše vyjímečně.
```cs 
    int i = 0;
label:
    Console.WriteLine(i);
    ++i;
    if (i < 10) goto label;
```
* S použitím příkazu `while` můžeme cyklus zpřehlednit. Cyklus while se používá většinou pokud neznáme předem počet opakování. Když počet opakování známe, tak použijeme spíše příkaz `for`.
```cs 
int i = 0;
while (i < 10)
{
    Console.WriteLine(i);
    ++i;
}
```
* Příkaz `do-while` používáme, pokud nevíme předem počet opakování a chceme aby se cyklus provedl alespoň jednou. Příkaz `Console.ReadKey(true).Key` čeká na stisk klávesy a nemusíme přitom zadávate enter.
```cs 
int n = 0;
do
{
    if (Console.ReadKey(true).Key == ConsoleKey.Spacebar)
    {
        ++n;
    }
} while (n < 3);
```
* Příkaz `for` používáme většinou, pokud předem známe počet opakování.

```cs 
for (int i = 0; i < 10; i++)
{
    Console.WriteLine(i);
}
```
* Provádění cyklu můžeme ukončit pomocí příkazu `break`. V následujícím příkazu výpis ukončíme po stiknutí klávesy `Escape`.

```cs 
Console.WriteLine("Stiskni klavesu Escape pro ukonceni vypisu");
Console.WriteLine("Stiskni libovolnou klavesu jinou klavesu nez Escape pro nove cislo");
            
for (int i = 0; i < 10; i++)
{
    if(Console.ReadKey(true).Key == ConsoleKey.Escape)
    {
        break;
    }
    Console.WriteLine(i);
}

Console.WriteLine("konec vypisu");
```
* Zbytek příkazů ve složeném příkazu cyklu můžeme přeskočit pomocí příkazu `continue`. V následujícím příkazu výpis přeskočíme stiskem klávesy `Spacebar` (mezerník) .

```cs 
for (int i = 0; i < 10; i++)
{
    if(Console.ReadKey(true).Key == ConsoleKey.Spacebar)
    {
        continue;
    }
    Console.WriteLine(i);
}

Console.WriteLine("konec vypisu");
```
---
V následujících kódech je uvedený kompletní příklad a řešení příkladů ze cvičení.

- Příkaz goto

```cs 
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Stiskni klavesu q");

        znovu:
            char znak = Console.ReadKey().KeyChar;

            if(znak != 'q')
            {
                Console.WriteLine();
                Console.WriteLine("Mas zadat q");
                goto znovu;
            }

            Console.WriteLine("zadal jsi q, vyborne");
            Console.ReadKey();
        }
    }
}
```

- Příkaz while

```cs 
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Stiskni klavesu q");
            char znak;
            
            while ((znak = Console.ReadKey().KeyChar) != 'q') 
            {
                 Console.WriteLine();
                 Console.WriteLine("Mas zadat q");
            }
            
            Console.WriteLine("zadal jsi q, vyborne");
            Console.ReadKey();
        }
    }
}
```

- Příkaz do-while

```cs 
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // s pomoci cyklu while vypiste 
            // na konzoli cisla 1,2,3,4,5,6,7,8,9
            int i = 1;
            while (i < 10) 
            {
                Console.WriteLine(i);
                i++;
            }
            
            // cekani na stisk klavesy q
            Console.WriteLine("Stiskni klavesu q");
            char znak;

            do
            {
                znak = Console.ReadKey().KeyChar;
                if(znak != 'q')
                {
                    Console.WriteLine();
                    Console.WriteLine("Mas zadat q");
                }
            }
            while (znak != 'q');
           
            Console.WriteLine("zadal jsi q, vyborne");
            Console.ReadKey();
        }
    }
}
```

- Příkaz for

```cs 
using System;

namespace ConsoleApp6
{
    class Program
    {
        static void Main(string[] args)
        {
            // s pomoci cyklu for vypiste na konzoli 
            // cisla 1,2,3,4,5,6,7,8,9
            for (int i = 1; i < 10; i++)
            {
                Console.WriteLine(i);
            }

            // cisla 10,9,8,7,6,5,4,3,2,1
            for (int i = 10; i > 0; i--)
            {
                Console.WriteLine(i);
            }

            // cisla 10,20,30,40,50,60,70,80,90,100
            for (int i = 10; i <= 100; i = i + 10)
            {
                Console.WriteLine(i);
            }

            // cisla 10,100,1000,10000,100000
            for (int i = 10; i <= 100000; i = i * 10)
            {
                Console.WriteLine(i);
            }

            // cisla 256,128,64,32,16,8,4,2,1
            for (int i = 256; i >= 1; i = i / 2)
            {
                Console.WriteLine(i);
            }

            Console.ReadKey();
        }
    }
}
```

- Výpočet faktoriálu

```cs 
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // Spocitejte faktorial cisla n
            // n!
            int n = 6;
            

            int f = n--;
            for (; n > 1; n--)
            {
                f = f * n;
            }

            Console.WriteLine(f);

            Console.ReadKey();
        }
    }
}
```

- Operátor modulo - zbytek po celočíselném dělení

```cs 
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            int x = 77;
            int zbytek = x % 5; // modulo, zbytek po celociselnem deleni
            if(zbytek == 0)
            {
                Console.WriteLine("cislo je delitelne 5");
            }

            // vypiste cisla od 0 do 100 delitelna 5 nebo 3
            // 0, 3, 5, 6, 9, 10 .... 100

            for (int i = 0; i < 100; i++)
            {
                if((i % 5 == 0) || (i % 3 == 0))
                {
                    Console.WriteLine(i);
                }
            }

            Console.ReadKey();
        }
    }
}
```

