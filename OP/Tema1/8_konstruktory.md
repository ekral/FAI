## Konstruktory 

Pro zvládnutí předmětu potřebujete vědět jak definovat konstruktory s parametry ve strukturách. 

Na následujících příkladech si probereme jednotlivé příkazy. 

* Nejprve si definujeme strukturu pro obelník, který bude mít definované rozměry `a` a `b`:
```cs 
struct Obdelnik
{
    public double a;
    public double b;
}
```
* Proměnnou typu `Obdelnik` vytvoříme následující způsobem, před prvním použitím musíme přiřadit všem fieldům hodnoty.
```cs 
Obdelnik o1;
o1.a = 2.0;
o1.b = 3.0;
Console.WriteLine($"Obdelnik ma rozmery {o1.a} x {o1.b}");
// vypise "Obdelnik ma rozmery 2 x 3"
```
* Pomocí výrazu `new Obdelnik()` můžeme přiřadit fieldům `a` a `b` výchozí hodnoty, tedy v případě typu `double` hodnoty `0`.
```cs 
Obdelnik o1 = new Obdelnik();
Console.WriteLine($"Obdelnik ma rozmery {o1.a} x {o1.b}");
// vypise "Obdelnik ma rozmery 0 x 0"
```
* Do struktury můžeme přidat konstruktor s jedním nebo více parametry a v něm nastavit výchozí hodnoty fieldů. Konstruktor je metoda, která nemá žádný návratový typ ani `void` a jmenuje se stejně jako struktura.
```cs 
struct Obdelnik
{
    public double a;
    public double b;

    public Obdelnik(double a, double b)
    {
        this.a = a;
        this.b = b;
    }
}
```
* Konstrukru potom můžeme předat jako argumenty výchozí hodnoty fieldů, například pomocí výrazu `new Obdelnik(2.0, 3.0)` můžeme přiřadit fieldům `a` a `b` výchozí hodnoty `2.0 ` respektive `3.0 `.
```cs 
Obdelnik o1 = new Obdelnik(2.0, 3.0);
Console.WriteLine($"Obdelnik ma rozmery {o1.a} x {o1.b}");
// vypise "Obdelnik ma rozmery 2 x 3"
```
---
Následují dva kompletní příklady.

- Komplexní číslo

```cs 
using System;

namespace Test
{
    struct Komplexni
    {
        public double re;
        public double im;

        public Komplexni(double re, double im)
        {
            this.re = re;
            this.im = im;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Komplexni c1 = new Komplexni(2.0, 3.0);
            Console.WriteLine($"Komplexni cislo ma hodnotu {c1.re} + {c1.im}i");
        }
    }
}
```

- Obdelník

```cs 
using System;

namespace Test
{
    struct Obdelnik
    {
        public double a;
        public double b;

        public Obdelnik(double a, double b)
        {
            this.a = a;
            this.b = b;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Obdelnik o1 = new Obdelnik(2.0, 3.0);
            Console.WriteLine($"Obdelnik ma rozmery {o1.a} x {o1.b}");
        }
    }
}
```
