# 03 Modifikátory přístupu public a private

**autor: Erik Král ekral@utb.cz**

---

Pro zvládnutí příkladu potřebujete znát modifikátor přístupu `public` a `private` a umět jej používat s fieldy a metodami.

Na následujících příkladech si probereme jednotlivé příkazy. 

Nejprve si definujeme třídu `BankovniUcet`, která bude mít `public` field `zustatek`:

```c++
class BankovniUcet
{
public:
    double zustatek = 0.0;
};
```

Pokud je členský prvek `public` tak k němu můžeme přistupovat i v klientském kódu mimo členské funkce třídy (nebo struktury) ve které je prvek definovaný.

```c++
int main()
{
    BankovniUcet ucet;
    ucet.zustatek = 10000.0; // muzeme pristupovat
}
```

Pokud je členský prvek `private` tak k němu nemůžeme přistupovat v klientském kódu mimo členské funkce třídy (nebo struktury) ve které je prvek definovaný. Můžeme ale definovat `public` členské funkce a s jejich pomocí k `private` členu přistupovat.

```c++
#include <cstdio>

class BankovniUcet
{
private:
    double zustatek = 0.0;
public:
    [[nodiscard]] double VratZustatek() const
    {
        return zustatek;
    }

    void Vloz(double castka)
    {
        zustatek += castka;
    }

    void Vyber(double castka)
    {
        zustatek -= castka;
    }
};

int main()
{
    BankovniUcet ucet;
    // ucet.zustatek = 10000.0; // nejde prelozit
    ucet.Vloz(10000.0);
    ucet.Vyber(2000.0);

    double zustatek = ucet.VratZustatek();

    printf("zustatek = %lf\n", zustatek);
}
```

Členské prvky skrýváme před vývojářem, abychom zabránili chybám v použití třídy v klienstkém kódu. V našem příkladu nechceme aby vývojář mohl například omylem nastavit záporný zůstatek. Ve funkcím Vloz a Vyber můžeme vyvolávat vyjímky a tím způsobem odhalit chyby z použití během vývoje aplikace.

```c++
#include <cstdio>
#include <stdexcept>

class BankovniUcet
{
private:
    double zustatek = 0.0;
public:
    [[nodiscard]] double VratZustatek() const
    {
        return zustatek;
    }

    void Vloz(double castka)
    {
        if (castka <= 0) throw std::invalid_argument("castka musi byt kladne cislo");

        zustatek += castka;
    }

    void Vyber(double castka)
    {
        if (castka <= 0) throw std::invalid_argument("castka musi byt kladne cislo");
        if (castka > zustatek) throw std::invalid_argument("castka musí být mensi nez zustatek");

        zustatek -= castka;
    }
};

int main()
{
    BankovniUcet ucet;
    // ucet.zustatek = 10000.0; // nejde prelozit
    ucet.Vloz(10000.0);
    ucet.Vyber(2000.0);

    double zustatek = ucet.VratZustatek();

    printf("zustatek = %lf\n", zustatek);

    //ucet.Vloz(-1000.0); // vyvola vyjimku
    //ucet.Vyber(-1000.0); // vyvola vyjimku
    //ucet.Vyber(20000.0); // vyvola vyjimku
}
```