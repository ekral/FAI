Příklady k procvičování:
1. Abstrakní metody a rozhraní: [zadání](1_zadani.cs) [řešení](1_reseni.cs)
---

## Dependency injection

Pro zvládnutí testu potřebujete vědět co je to technika Dependency Injection a umět ji používat.

Technika Dependency Injection se používá k tomu aby jedna třída nebyla přímo závislá na jiné třídě a její konkrétní implementaci. Tato technika často používá proto aby byl kód lépe testovatelný, protože můžeme v kódu jednodušeji nakonfigurovat objekt pro potřeby testu. Technika Dependency Injection je založena na tom, že místo reference typu konkrétní třídy používáme referenci typu rozhraní a vlastní instanci potom předáváme nejčastěji v konstruktoru (jsou ale i varianty s Property nebo metdou). 

Lépe se ale tato technika chápe na konrétním příkladu. Ukážeme si příklad, kdy budeme mít třídu `Automobil` a ta bude mít field `motor` a budeme chtít při vytváření instance třídy `Automobil` zvolit, zda bude mít benzínový nebo naftový motor.

### Příklad bez použití Dependency Injection

* Nejprve si definujeme třídu BenzinovyMotor:
```cs 
class BenzinovyMotor
{
    public void Nastartuj()
    {
        Console.WriteLine("Zapalil jsem smes benzinu a vzduchu zapalovaci svickou");
    }
}
```
* A potom třídu `Automobil`, která bude mít referenci a objekt typu `BenzinovyMotor` a metodě Jed zavolá metodu motoru `Nastartuj`:

```cs 
class Automobil
{
    private BenzinovyMotor motor;

    public Automobil()
    {
        motor = new BenzinovyMotor();
    }

    public void Jed()
    {
        Console.WriteLine("Startuji ...");
        motor.Nastartuj();
    }
}
```
* Použití třídy `Automobil` potom bude následující:

```cs 
static void Main(string[] args)
{
    Automobil automobil = new Automobil();
    automobil.Jed();
}
```

### Příklad s použitím Dependency Injection

* Představme si ale situaci, že bychom chtěli při vytváření nového automobilu zvolit typ motoru, například místo benzínového motoru bychom chtěli zvoli následující naftový motor, a nechtěli bychom přitom měnit kód třídy `Automobil`:

```cs 
class NaftovyMotor
{
    public void Nastartuj()
    {
        Console.WriteLine("Zapalil jsem jsem smes díky vznícení způsobené vysokou teplotou stlačeného vzduchu");
    }
}
```
* Nejprve vytvoříme rozhraní, které budou implementovat oba motory (a případně i další nové typy motorů):

```cs 
interface IMotor
{
    void Nastartuj();
}

class BenzinovyMotor : IMotor
{
    public void Nastartuj()
    {
        Console.WriteLine("Zapalil jsem smes benzinu a vzduchu zapalovaci svickou");
    }
}

class NaftovyMotor : IMotor
{
    public void Nastartuj()
    {
        Console.WriteLine("Zapalil jsem jsem smes díky vznícení způsobené vysokou teplotou stlačeného vzduchu");
    }
}
```
* Ve třídě `Automobil` potom budeme mít referenci `motor` typu `IMotor` místo konkrétního typu motoru. A konkrétní motor si předáme jako parametr konstruktoru.

```cs 
class Automobil
{
    private IMotor motor;

    public Automobil(IMotor motor)
    {
        this.motor = motor;
    }

    public void Jed()
    {
        Console.WriteLine("Startuji ...");
        motor.Nastartuj();
    }
}
```

* Při vytváření instance třídy `Automobil` v konstruktoru předáme referenci na instanci konkrétního typu motoru a nemusíme přitom měnit kód třídy `Automobil`:

```cs 
static void Main(string[] args)
{
    Automobil automobilBenzin = new Automobil(new BenzinovyMotor());
    automobilBenzin.Jed();

    Automobil automobilNafta = new Automobil(new NaftovyMotor());
    automobilNafta.Jed();
}
```

## Singleton




