# ASCII Kreslení rovnostranného trojúhelníka

Cvičení procvičuje definici třídy, členských proměnných, členských funkcí, konstruktoru, member initializer listu a práci s ukazateli na strukturu.

S kódem pro ASCII kreslení úsečky vykreslete rovnostranný trojúhelník.

1) Definujte třídu reprezetující rovnostranný trojúhleník `RovnostrannyTrojuhelnik`
- Trojúhelník bude definovaný **středem** `S` a **délkou strany** `a` a úsečka AB bude rovnoběžná s osou *x*.
- Proměnné `S` a `a` budou private.
2) Ve třídě `RovnostrannyTrojuhelnik` definujte parametrický konstruktor a member initializer list.
3) Ve třídě `RovnostrannyTrojuhelnik` definujte členskou funkci `void Nakresli(Platno* platno)` ktera nakresli trojuhelnik na plátno.

```cpp
#include <iostream>
#include <string>
#include <algorithm>
#include <cmath>
#include <ranges>

using namespace std;

struct Bod2d
{
    double x;
    double y;

    Bod2d(const double x, const double y) : x(x), y(y)
    {

    }
};

class Platno
{
private:
    string retezec;
public:
    const int sirka;
    const int vyska;

    Platno(const int sirka, const int vyska) : retezec((sirka + 1) * vyska, '-'), sirka(sirka), vyska(vyska)
    {
        Vymaz();
    }

    void Vymaz()
    {
        fill(retezec.begin(), retezec.end(), '-');

        for (int i = sirka; i < retezec.length(); i += sirka + 1)
        {
            retezec[i] = '\n';
        }

    }

    void Zobraz() const
    {
        cout << retezec << endl;
    }

    void NakresliBod(const double x, const double y)
    {
        const int ix = static_cast<int>(round(x));
        const int iy = static_cast<int>(round(y));

        if(ix < 0.0 || ix >= sirka || iy < 0.0 || iy >= vyska)
        {
            return;
        }


        const int pos = (vyska - iy - 1) * (sirka + 1) + ix;

        retezec[pos] = 'x';
    }

    void NakresliUsecku(const Bod2d& A, const Bod2d& B)
    {
        double dx = B.x - A.x;
        double dy = B.y - A.y;

        double dmax = max(abs(dx), abs(dy));

        double stepx = dx / dmax;
        double stepy = dy / dmax;

        double x = A.x;
        double y = A.y;

        for (double t = 0.0; t <= dmax; t += 1.0)
        {
            NakresliBod(x, y);

            x += stepx;
            y += stepy;
        }
    }
};

// 🚀 Zde nadefinujte tridu RovnostrannyTrojuhelnik

int main()
{
    Platno platno(20, 10);

    platno.Vymaz();

    Bod2d stred(9.5, 4.5);

    // 🚀 Implementujte:
    //RovnostrannyTrojuhelnik trojuhelnik(stred, 10.0);
    //trojuhelnik.Nakresli(&platno);
    
    platno.Zobraz();

    return 0;
}
```
