# Ascii Rotace trojúhelníka

Vytvořte animaci rotace rovnostranného trojúhleníka kolem jeho středu. Využijte připravenou funkci `gotoxy`. Kód je funkční jen na operačním systému Windows. V CLionu nastavte `EditConfigurations -> Run in external console`.

```cpp
#include <iostream>
#include <string>
#include <algorithm>
#include <cmath>
#include <windows.h>

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

Bod2d rotace(Bod2d A, const Bod2d S, const double uhelStupne)
{
    A.x -= S.x;
    A.y -= S.y;

    Bod2d At = rotace(A, uhelStupne);

    At.x += S.x;
    At.y += S.y;

    return At;
}

class RovnostrannyTrojuhelnik
{
private:
    Bod2d S;
    double a;

    Bod2d A;
    Bod2d B;
    Bod2d C;

    double uhel;

public:
    RovnostrannyTrojuhelnik(const Bod2d S, const double a) :
        S(S),
        a(a),
        A(S.x - a / 2, S.y - sqrt(3.0) * a / 6),
        B(S.x + a / 2, S.y - sqrt(3.0) * a / 6),
        C(S.x, S.y + sqrt(3.0) * a / 3),
        uhel(0.0)
    {

    }

    void ZmenRotaci(const double uhel)
    {
        this->uhel = uhel;
    }

    void Nakresli(Platno* platno) const
    {
        const Bod2d At = rotace(A, S, uhel);
        const Bod2d Bt = rotace(B, S, uhel);
        const Bod2d Ct = rotace(C, S, uhel);

        platno->NakresliUsecku(At, Bt);
        platno->NakresliUsecku(Bt, Ct);
        platno->NakresliUsecku(Ct, At);
    }
};

void gotoxy(int x, int y)
{
    const COORD pos = {static_cast<short>(x), static_cast<short>(y)};
    const HANDLE output = GetStdHandle(STD_OUTPUT_HANDLE);
    SetConsoleCursorPosition(output, pos);
}

int main()
{
    // Kod pouze pro Windows
    // ⚡️ V Clionu dejte EditConfigurations -> Run in external console

    Platno platno(40, 20);
    double uhel = 0.0;
    
    platno.Vymaz();

    const Bod2d S(19, 9);

    RovnostrannyTrojuhelnik trojuhelnik(S, 10.0 );

    trojuhelnik.ZmenRotaci(uhel);

    trojuhelnik.Nakresli(&platno);
    
    platno.Zobraz();
    
    return 0;
}
```
