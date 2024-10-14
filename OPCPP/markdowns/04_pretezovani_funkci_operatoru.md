# Přetěžování funkcí a operátorů

**autor: Erik Král ekral@utb.cz**

---

# Přetěžování funkcí

Přetěžování funkcí znamená, že můžeme mít více funkcí se stejným názvem, pokud mají jiné parametry. V následujícím příkladu máme tři metody `Vypis`. První metoda má jeden parametr typu `int`, druhá dva parametry typu `int` a třetí má parametr typu `double`. Překladač potom podle typu argumentu zavolá správnou metodu `Vypis`.

```cpp
#include <cstdio>

void Vypis(int x)
{
    printf("int x: %d\n", x);
}

void Vypis(int x, int y)
{
    printf("int x: %d, int y: %d\n", x, y);
}

void Vypis(double x)
{
    printf("double x: %f\n", x);
}

int main()
{
    Vypis(3);
    Vypis(3, 4);
    Vypis(3.0);

    return 0;
}
```

# Typ reference

Typ reference vzniknul kvůli přetěžování operátorů. 

# Přetěžování operátorů

Přetěžování operátorů znamená, že kromě metod můžou třídy a struktury podporovat i operátory.

https://en.cppreference.com/w/cpp/language/operators

```cpp
#include <cstdio>

struct Bod
{
    double x;
    double y;

    Bod& operator += (const Bod& rhs)
    {
        x += rhs.x;
        y += rhs.y;

        return *this;
    }

    friend Bod operator + (Bod lhs, const Bod& rhs)
    {
        lhs += rhs;

        return lhs;;
    }
};

int main()
{
    Bod bod1 = {0.0, 0.0};
    Bod bod2 = {3.0, 4.0};

    bod1 += {1.0, 2.0};

    Bod bod3 = bod1 + bod2;

    printf("x: %lf y: %lf\n", bod3.x, bod3.y);
    return 0;
}
```

# Kopírovací a přesouvací konstruktor

Kopírovací a přesouvací konstruktor slouží k vytvoření hluboké kopie instance třídy respektive struktury.
