# Přetěžování funkcí a operátorů

**autor: Erik Král ekral@utb.cz**

---

# Přetěžování funkcí

Přetěžování funkcí (function overloading) znamená, že můžeme mít více funkcí se stejným názvem, pokud mají jiné parametry. V následujícím příkladu máme tři metody `Vypis`. První metoda má jeden parametr typu `int`, druhá dva parametry typu `int` a třetí má parametr typu `double`. Překladač potom podle typu argumentu zavolá správnou metodu `Vypis`.

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

Typ reference vzniknul kvůli přetěžování operátorů, kde nebylo možné použít ukazatel, ale bylo potřeba předávat referenci. Typ refeence se zapisuje se znakem `&` za typem, například `int&` nebo `Student&`. Kromě přetížených operátorů se typ reference používá i pro předávání argumentů, typicky s klíčovým slovem `const`.

```cpp
#include <iostream>
#include <string>

using namespace std;

class Student
{
public:
    string jmeno;
    string prijmeni;
    string skupina;
};

void Vypis(const Student& student)
{
    cout << student.jmeno << " " << student.prijmeni << " " << student.skupina << endl;
}

int main()
{
    Student s1;
    s1.jmeno = "Alena";
    s1.prijmeni = "Vesela";
    s1.skupina = "SWI1";

    Vypis(s1);
    
    return 0;
}
```

# Přetěžování operátorů

Přetěžování operátorů (operator overloading) znamená, že kromě metod můžou třídy a struktury podporovat i operátory.

Následující kód definuje pro strukturu Bod unární operátor `+=` a binární operátor `+`. Příklad vychází z kód z odkazu:

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

V následujícím příkladu má třída definovaný kopírovací konstruktor i operátor přiřazení. Ještě by měla mít definovaný `move` konstruktor a `move` operátor přiřazení, ale pro zjednodušení jsou vynechány.

```cpp
#include <iostream>
#include <algorithm>

using namespace std;

class Trida
{
public:
    int n;
    int* data;

    explicit Trida(const int n) :  n(n), data(new int[n])
    {
        fill_n(data, n, 0);
    }

    Trida(const Trida& other) : n(other.n), data(new int[n])
    {
        copy_n(other.data, n, data);
    }

    Trida& operator = (Trida other) noexcept
    {
        swap(*this, other);

        return *this;
    }

    friend void swap(Trida& first, Trida& second) noexcept
    {
        using std::swap; // neni nutne

        swap(first.data, second.data);
        swap(first.n, second.n);
    }

    ~Trida()
    {
        delete[] data;
    }
};

void Vypis(const Trida& trida)
{
    for (int i = 0; i < trida.n; i++)
    {
        cout << trida.data[i];
    }

    cout << endl;
}

int main()
{
    const Trida t1(4);

    t1.data[0] = 1;
    t1.data[1] = 2;
    t1.data[2] = 3;
    t1.data[3] = 4;

    const Trida t2(t1);

    Trida t3(7);
    Vypis(t3);

    t3 = t1;

    Vypis(t1);
    Vypis(t2);
    Vypis(t3);

    return 0;
}
```

Probraná témata jsou pokročilá. Abychom nemuseli definovat kopírovací a přesouvací členské prvky, tak je nejjednoduší používat zabudované typy, jako je string, vector a chytré ukazatele a nepoužívat ["naked pointers"](https://stackoverflow.com/questions/9299489/whats-a-naked-pointer).