# Příklad na dědičnost: Tvary rovnostranný trojuhelnik, čtverec a úsečka

Cílem příkladu je procvičení dědičnosti kódu. 

1. Třída Shape

Definujte rodičovskou třídu `Shape`, která bude mít `protected` členské prvky `S` (souřadnice středu) a `angle` (úhel rotace kolem středu).

2. Potomci třídy Shape

    - třída `Triangle` - rovnostranný trojúhelník zadaný středem a délkou strany a úhlem rotace kolem středu.
    - třída `Rectangle` - obdelník zadaný délkami stran, středem a úhlem rotace kolem středu.
    - třída `Line` - úsečka zadaná delkou, středem a úhlem rotace kolem středu.

3. Vyreslení obrazců na Ascii plátno.

Vytvořte menu:
1. Create triangle.
2. Create rectangle.
3. Create line.
4. Exit

Po vytvoření obrazce jej vykreslete na Ascii plátno.

Výchozí kód:

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

Bod2d rotate(const Bod2d A, const double uhelStupne)
{
    const double uhelRadiany = (uhelStupne * M_PI) / 180;

    const double xt = A.x * cos(uhelRadiany) - A.y * sin(uhelRadiany);
    const double yt = A.x * sin(uhelRadiany) + A.y * cos(uhelRadiany);

    const Bod2d At(xt, yt);

    return At;
}

Bod2d rotate(Bod2d A, const Bod2d S, const double uhelStupne)
{
    A.x -= S.x;
    A.y -= S.y;

    Bod2d At = rotace(A, uhelStupne);

    At.x += S.x;
    At.y += S.y;

    return At;
}

class Platno
{
private:
    string retezec;
public:
    const int sirka;
    const int vyska;

    // pomoci member initializer listu volam konstruktor stringu
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

// Zde nadefinujte tridu RovnostrannyTrojuhelnik
class RovnostrannyTrojuhelnik
{
private:
    Bod2d S;
    double a;
public:
    RovnostrannyTrojuhelnik(const Bod2d S, const double a) : S(S), a(a)
    {
    }

    void Nakresli(Platno* platno) const
    {
        Bod2d A(S.x - a / 2, S.y - sqrt(3.0) * a / 6);
        Bod2d B(S.x + a / 2, S.y - sqrt(3.0) * a / 6);
        Bod2d C(S.x, S.y + sqrt(3.0) * a / 3);

        platno->NakresliUsecku(A, B);
        platno->NakresliUsecku(B, C);
        platno->NakresliUsecku(C, A);
    }
};

int main()
{
    Platno platno(20, 10);

    platno.Vymaz();

    const Bod2d A(0.0, 0.0);
    const Bod2d B(19.0, 9.0);

    platno.NakresliBod(A.x, A.y);
    platno.NakresliBod(B.x, B.y);

    //platno.NakresliUsecku(A, B);

    const Bod2d stred(9.5, 4.5);
    platno.NakresliBod(stred.x, stred.y);

    RovnostrannyTrojuhelnik trojuhelnik(stred, 10.0);
    trojuhelnik.Nakresli(&platno);

    platno.Zobraz();

    return 0;
}
```





