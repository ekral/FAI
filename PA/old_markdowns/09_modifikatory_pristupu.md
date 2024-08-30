## 9. Modifikátory přístupu public a private

Pro zvládnutí příkladu potřebujete znát modifikátor přístupu `public` a `private` a umět jej používat s fieldy a metodami. 

Na následujících příkladech si probereme jednotlivé příkazy. 

* Nejprve si definujeme třídu pro kruh, která bude mít `public` field `polomer`:
```cs 
class Kruh
{
    public double polomer;
}
```
* Pokud je field nebo metoda `public` tak k ní můžeme přistupovat i mimo metody třídy nebo struktury ve které je feild definovaný.
```cs 
Kruh kruh = new Kruh();
kruh.polomer = 10.0;
Console.WriteLine(kruh.polomer);
```
* Pokud je field nebo metoda `private` tak k ní nemůžeme přistupovat mimo metody třídy nebo struktury ve které je field definovaný. Můžeme ale definovat `public` metody a s jejich potom k `private` fieldu přistupovat. V následujícím příkladu má kruh `private` field `polomer` a `public` metody `VratPolomer` a `NastavPolomer`.
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
* Mimo třídu `Kruh` potom nemůžeme přistupovat k fieldu `polomer` přímo ale jen pomocí metod `VratPolomer` a `NastavPolomer`.
```cs 
Kruh kruh = new Kruh();
//kruh.polomer = 10.0; // nelze prelozit protoze polomer je private
//Console.WriteLine(kruh.polomer); // nelze prelozit protoze polomer je private
kruh.NastavPolomer(10);
Console.WriteLine(kruh.VratPolomer()); 
```

---
Následuje kompletní příklad.

- Příklad kruh

```cs 
using System;

namespace Test9
{
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
    class Program
    {
        static void Main(string[] args)
        {
            Kruh kruh = new Kruh();
            //kruh.polomer = 10.0; // nelze prelozit protoze polomer je private
            //Console.WriteLine(kruh.polomer); // nelze prelozit protoze polomer je private
            kruh.NastavPolomer(10);
            Console.WriteLine(kruh.VratPolomer());
        }
    }
}
```
