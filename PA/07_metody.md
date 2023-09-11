## 7. Metody

Pro zvládnutí předmětu potřebujete vědět jak definovat metody ve strukturách, jak definovat parametry metody a jak předávat argumenty metodám a nakonec jak vrátit hodnotu z metody. 

Na následujících příkladech si probereme jednotlivé příkazy. 

* Nejprve si definujeme strukturu pro obélník, který bude mít definované rozměry `a` a `b`:
```cs 
struct Obdelnik
{
    public double a;
    public double b;
}
```

* Dále nadefinujeme metodu, pomocí které můžeme změnit rozměry obdélníku. Typ `void` znamená, že metoda nevrací žádný typ a v kulatých závorkách uvádíme seznam parametrů oddělený čárkou. Pomocí klíčového slova `this` můžeme odlišit **field** od lokální proměnné nebo parametru.
```cs 
struct Obdelnik
{
    public double a;
    public double b;

    public void ZmenRozmery(double a, double b)
    {
        this.a = a;
        this.b = b;
    }
}
```
* Metodu potom zavoláme pomocí operátoru `.` a jména metody, kterému říkáme také identifikátor metody. Metodě potom předáváme v kulatých závorkách parametry pro její argumenty. Parametry se předávají jako hodnoty a argumenty jsou na nich nezávislé.
```cs 
Obdelnik o1 = new Obdelnik(); // rozmery budou 0,0
o1.ZmenRozmery(2.0, 3.0);
```
* Metody také mohou vracet jednu hodnotu. V následujícím kódu definujeme metodu `VratObvod`, která má návratový typ `double` a nemá žádné parametry: 
```cs 
struct Obdelnik
{
    public double a;
    public double b;

    public void ZmenRozmery(double a, double b)
    {
        this.a = a;
        this.b = b;
    }

    public double VratObvod()
    {
        return a + b;
    }
}
```
* U metody, která má návratový typ jiný než `void` a tedy vrací hodnotu je výsledkém volání metody hodnota, kterou vrátila metoda. Toto návratoou hodnotu v následujícím příkladu přiřazujeme pomocí operátoru `=` proměnné `obvod`.
```cs 
Obdelnik o1 = new Obdelnik(); // rozmery budou 0,0
o1.ZmenRozmery(2.0, 3.0);
double obvod = o1.VratObvod();
```
---
Následuje kompletní příklad.

- Metody struktury Obdelník

```cs 
using System;

namespace Test
{
    class Program
    {
        struct Obdelnik
        {
            public double a;
            public double b;

            public void ZmenRozmery(double a, double b)
            {
                this.a = a;
                this.b = b;
            }

            public double VratObvod()
            {
                return a + b;
            }

            public double VratObsah()
            {
                return a * b;
            }
        }

        static void Main(string[] args)
        {
            Obdelnik o1 = new Obdelnik(); // rozmery budou 0,0
            o1.ZmenRozmery(2.0, 3.0);
            double obvod = o1.VratObvod();
            double obsah = o1.VratObsah();

            Console.WriteLine($"Obdelnik ma rozmery {o1.a}, {o1.b} a jeho obvod je {obvod} a obsah {obsah}");
        }
    }
}
```
