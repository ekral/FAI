# Ascii Rotace úsečky

S využitím kódu z úkolu uvedenéno níže vytvořte program, který:

Zarotuje úsečku kolem středu úsečky.

Pokud chceme zarotovat úsečku, tak musíme:

 1) Zjistit střed úsečky.
 2) Posunout krajní body úsečky, tak aby střed úsečky byl v počátku souřadnic.
 3) Zarotovat krajní body úsečky kolem počátku souřadnic.
 4) Posunout úsečku zpět aby její střed byl opět na původním místě.

Použijte vzorec:

$$\begin{align*}
x' &= x \cdot \cos(\theta) - y \cdot \sin(\theta) \\
y' &= x \cdot \sin(\theta) + y \cdot \cos(\theta)
\end{align*}$$

$$\begin{align*}
\text{ Kde} (x', y') \text{ jsou souřadnice zarotovaného bodu, }
(x, y) \text{ jsou souřadnice původního bodu a }
\theta \text{ je úhel rotace v radiánech.}
\end{align*}$$

```cpp
#include <iostream>
#include <string>
#include <algorithm>
#include <cmath>

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

Bod2d rotace(const Bod2d A, const double uhelStupne)
{
    const double uhelRadiany = (uhelStupne * M_PI) / 180;

    const double xt = A.x * cos(uhelRadiany) - A.y * sin(uhelRadiany);
    const double yt = A.x * sin(uhelRadiany) + A.y * cos(uhelRadiany);

    const Bod2d At(xt, yt);

    return At;
}

// 🚀 Zde nadefinujte Usecku

int main()
{
    Platno platno(40, 20);

    platno.Vymaz();

    const Bod2d A(10.0, 10.0);
    const Bod2d B(30.0, 10.0);

    // 🚀 implementujte Usecku
    
    //Usecka usecka(A,B);
    //usecka.ZmenRotaci(1.0);

    //usecka.Nakresli(&platno);

    platno.Zobraz();

    return 0;
}
```
