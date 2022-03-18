Test 1 - příprava
---
Pro zvládnutí předmětu potřebujete znát minimálně dva aritmetické datové typy a umět definovat proměnné těchto typů. Nezapomeňte, že záleží i na velkých a malých písmenech a proměnná `Math.Pow` musí být napsaná správně včetně velkých a malých písmen.
* `double y = 0;` desetiné číslo se znaménkem
* `int x = 0;` celé číslo se znaménkem

Dále byste měli znát následující operace, které si postupně probereme na příkladech. 

Nejprve si ale definujme tři proměnné `x`, `y` a `z`:
```cs 
double x = 2.0;
double y = 3.0;
double z = 0.0;
```
* Matematické operátory součet, rozdíl, součin, podíl, záporná hodnota, druhá mocnina. Pro druhou mocninu používáme zápis `x * x` protože protože je to rychlejší a jednoduší než použití metody *Math.Pow*.
```cs 
z = x + y; // soucet
z = x - y; // rozdil
z = x * y; // soucin
z = x / y; // podil
z = -x; // zaporna hodnota
z = x * x; // druha mocnina
```
* Matematické operace ze třídy *Math* pro mocninu a odmocninu
```cs 
z = Math.Pow(x, 100.0); // mocnina x^100
z = Math.Sqrt(9.0); // druha odmocnina
```
* Použití konstanty PI ze třídy *Math*
```cs 
z = 2 * Math.PI * x; // konstanta PI
```
* Určování priorit operátorů pomocí kulatých závorek ():
```cs 
z = x * (y + 3.0); // kulate zavorky urcuji prioritu 
```

* A nakonec změna hodnot proměnné samotné:
```cs 
z = z + 2.0; // zvyseni o hodnotu
z = z - 2.0; // snizeni o hodnotu
++z; // zvyseni o 1
--z; // snizeni o 1
```
---
Pro typ `int` je zápis předchozích operací stejný, jen používáme celočíselné numerické konstanty. Největší rozdíl je ale v tom, že metody *Math.Pow* a *Math.Sqrt* pracují s typem `double` takže výsledek musíme explicitně přetypovat s pomocí zápisu `(int)`. Samotné argumenty těchto metod jsou ale typu `double` a typ `int` lze na typ `double` převést implicitně (nemusíme do kódu nic psát).
```cs 
int a = 2;
int b = 3;
b = (int)Math.Pow(a, 100.0); // mocnina x^100
b = (int)Math.Sqrt(9.0); // druha odmocnina
a = a + 2; // zvyseni o hodnotu
a = a - 2; // snizeni o hodnotu
```
---
V následujících kódech je uvedený kompletní příklad a řešení příkladů ze cvičení.

- Přehled aritmetických operátorů

```cs 
using System;

namespace MujPrvniProjekt
{
    class Program
    { 
        static void Main(string[] args)
        {
            double x = 2.0;
            double y = 3.0;
            double z = 0.0;

            // Matematicke operatory
            z = x + y; // soucet
            z = x - y; // rozdil
            z = x * y; // soucin
            z = x / y; // podil
            z = -x; // zaporna hodnota
            z = x * x; // druha mocnina

            // Matematicke operace ze tridy Math
            z = Math.Pow(x, 100.0); // mocnina x^100
            z = Math.Sqrt(9.0); // druha odmocnina

            // Konstanta PI
            z = 2 * Math.PI * x; // konstanta PI
            // Priorita operatoru
            z = x * (y + 3.0); // kulate zavorky urcuji prioritu 

            // Zmena hodnoty promenne
            z = z + 2.0; // zvyseni o hodnotu
            z = z - 2.0; // snizeni o hodnotu
            ++z; // zvyseni o 1
            --z; // snizeni o 1

            // Operace pro int
            int a = 2;
            int b = 3;
            b = (int)Math.Pow(a, 100.0); // mocnina x^100
            b = (int)Math.Sqrt(9.0); // druha odmocnina
            a = a + 2; // zvyseni o hodnotu
            a = a - 2; // snizeni o hodnotu
        }
    }
}
```

- Obsah a obvod čtverce

```cs
using System;

namespace MujPrvniProjekt
{
    class Program
    {
        static void Main(string[] args)
        {
            double n = 3.0;
            Console.WriteLine($"Delka strany je {n}");
            
            // ctverec definovany delkou strany
            // spocitejte a vypiste obvod a obsah ctverce
            
            double obvod = 4 * n;
            Console.WriteLine($"Obvod je {obvod}");

            double obsah = n * n;
            Console.WriteLine($"Obsah je {obsah}");

            Console.ReadKey();
        }
    }
}
```

- Obsah a obvod kruhu

```cs 
using System;

namespace MujPrvniProjekt
{
    class Program
    {
        static void Main(string[] args)
        {
            double r = 3.0;
            Console.WriteLine($"Polomer kruhu je {r}");
            Console.WriteLine($"Hodnota Pi je {Math.PI}");
            
            // kruh je definovany polomerem
            // spocitejte a vypiste obvod a obsah kruh

            double obvod = 2 * Math.PI * r;
            Console.WriteLine($"Obvod je {obvod}");

            double obsah = Math.PI * r * r;
            Console.WriteLine($"Obsah je {obsah}");

            Console.ReadKey();
        }
    }
}
```

- Obsah a obvod trojúhelníka

```cs 
using System;

namespace MujPrvniProjekt
{
    class Program
    {
        static void Main(string[] args)
        {
            double a = 3.0;
            double b = 4.0;
            double c = 5.0;
            Console.WriteLine($"Delka strany trojuhelnika je {a}, {b} a {c}");
            
            // trojuhelnik je definovany delkami stran
            // spocitejte a vypiste obvod a obsah trojuhelniku dle heronova vzorce https://cs.wikipedia.org/wiki/Heron%C5%AFv_vzorec
            
            double obvod = a + b + c;
            Console.WriteLine($"Obvod je {obvod}");

            double s = (a + b + c) / 2;
            double obsah = Math.Sqrt(s * (s - a) * (s - b) * (s -c));
            Console.WriteLine($"Obsah je {obsah}");

            Console.ReadKey();
        }
    }
}
```

- Výpočet BMI

```cs 
using System;

namespace MujPrvniProjekt
{
    class Program
    { 
        static void Main(string[] args)
        {
            double hmotnost = 85;
            double vyska = 1.78;

            Console.WriteLine($"hmotnost {hmotnost}kg a vyska {vyska}m");
            
            // vypocet bmi dle vzorce https://cs.wikipedia.org/wiki/Index_t%C4%9Blesn%C3%A9_hmotnosti

            double bmi = hmotnost / (vyska * vyska);

            Console.WriteLine($"bmi je {bmi}");
        }
    }
}
```

- Výpočet splátky úroku

```cs 
using System;

namespace ConsoleApp11
{
    class Program
    { 
        static void Main(string[] args)
        {
            int pocetLetSplaceni = 20;
            double rocniUrokProcenta = 2;
            double D = 1000000; // dluh

            Console.WriteLine($"Pocet let {pocetLetSplaceni}, urok {rocniUrokProcenta}% rocne a castka {D} Kc");
            
            // Vypocet splatky uroku dle vzorce http://www.aristoteles.cz/matematika/financni_matematika/hypoteka-vypocet.php
            
            int n = pocetLetSplaceni * 12; // pocet mesicu splaceni
            double i = rocniUrokProcenta / (12 * 100); // desetinne cislo

            double v = 1 / (1 + i);
            double splatka = (i * D) / (1 - Math.Pow(v, n));

            Console.WriteLine($"Mesicni splatka bude {splatka:F2} Kc");
        }
    }
}
```
