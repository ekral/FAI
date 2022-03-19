## Property 

Pro zvládnutí předmětu potřebujete umět definovat a používat property a auto-implemented property. 

Na následujících příkladech si probereme jednotlivé příkazy. 

* Nejprve si definujeme třídu pro kruh, která bude mít `private` field `polomer` a `public` metody `VratPolomer` a `NastavPolomer`.
```cs 
class Kruh
{
    private double polomer;

    public double VratPolomer()
    {
        return polomer;
    }

    public void NastavPolomer(double polomer)
    {
        this.polomer = polomer;
    }
}
```
* V jazyku C# můžeme metody `VratPolomer` a `NastavPolomer`nahradit speciálním typem property. Zápis je potom jednodušší. Znak `_` v názvu backing fieldu `_polomer` je jen jmenná konvence a není povinný. Další jmennou konvencí je, že název property začíná velkým znakem.
```cs 
class Kruh
{
    private double _polomer;

    public double Polomer
    {
        get { return _polomer; }
        set { _polomer = value; }
    }
}
```
* Property se potom používá stejně jako field.
```cs 
Kruh kruh = new Kruh();
kruh.Polomer = 10;
Console.WriteLine(kruh.Polomer);
```
* Pokud tělo property neobsahuje žádný kód navíc, tak můžeme zápis property zjednodušit a použít auto-implemented property. A teprve až budeme chtít do části get nebo set něco přidat tak změníme property na plný zápis.
```cs 
class Kruh
{
    public double Polomer { get; set; }
}
```
* Například bychom mohli v propertě vypsat na konzoli text vždy když property čteme nebo měníme její hodnotu.
```cs 
class Kruh
{
    private double _polomer;

    public double Polomer
    {
        get { Console.WriteLine("Cteni property"); return _polomer; }
        set { _polomer = value; Console.WriteLine("Zmena property"); }
    }
}
```
---
Následuje kompletní příklad.

- Full property

```cs 
using System;

namespace Test10
{
    class Kruh
    {
        private double _polomer;

        public double Polomer
        {
            get { return _polomer; }
            set { _polomer = value; }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Kruh kruh = new Kruh();
            kruh.Polomer = 10;
            Console.WriteLine(kruh.Polomer);
        }
    }
}
```

- Autoimplemented property

```cs 
using System;

namespace Test10
{
    class Kruh
    {
        public double Polomer { get; set; }
    }   

    class Program
    {
        static void Main(string[] args)
        {
            Kruh kruh = new Kruh();
            kruh.Polomer = 10;
            Console.WriteLine(kruh.Polomer);
        }
    }
}
```

- Dodatečný kód v property

```cs 
using System;

namespace Test10
{
    class Kruh
    {
        private double _polomer;

        public double Polomer
        {
            get { Console.WriteLine("Cteni property"); return _polomer; }
            set { _polomer = value; Console.WriteLine("Zmena property"); }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Kruh kruh = new Kruh();
            kruh.Polomer = 10;
            Console.WriteLine(kruh.Polomer);
        }
    }
}
```
