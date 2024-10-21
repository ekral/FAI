# Ascii Rotace bodu

S využitím kódu z úkolu uvedenéno níže vytvořte program, který:

Zarotuje bod (x,y) kolem počátku souřadnic (0,0) a vykreslete úsečku z počátku souřadnic do zarotovaného bodu (x', y'). Použijte vzorec:

$$\begin{align*}
x' &= x \cdot \cos(\theta) - y \cdot \sin(\theta) \\
y' &= x \cdot \sin(\theta) + y \cdot \cos(\theta)
\end{align*}$$

$$\begin{align*}
\text{ Kde} (x', y') \text{ jsou souřadnice zarotovaného bodu, }
(x, y) \text{ jsou souřadnice původního bodu a }
\theta \text{ je úhel rotace v radiánech.}
\end{align*}$$

Vyjděte z následujících zdrojových kódů:

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
        if(x < 0.0 || x >= sirka || y < 0.0 || y >= vyska)
        {
            return;
        }

        const int ix = static_cast<int>(round(x));
        const int iy = static_cast<int>(round(y));

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

    const Bod2d A(19.0, 0.0);
    //Bod2d At = rotace(A, 10); // napiste funkci, ktera zarotuje bod
    
    platno.NakresliBod(A.x, A.y);
    //platno.NakresliBod(At.x, At.y);

    //platno.NakresliUsecku(A, B);

    const Bod2d stred(9.5, 4.5);
    platno.NakresliBod(stred.x, stred.y);

    RovnostrannyTrojuhelnik trojuhelnik(stred, 10.0);
    trojuhelnik.Nakresli(&platno);

    platno.Zobraz();

    return 0;
}
```
