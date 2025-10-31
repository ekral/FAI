#include <iostream>
#include <string>
#include <algorithm>
#define _USE_MATH_DEFINES
#include <math.h>
#include <Windows.h>

using namespace std;

void gotoxy(int x, int y)
{
    const COORD pos = { static_cast<short>(x), static_cast<short>(y) };
    const HANDLE output = GetStdHandle(STD_OUTPUT_HANDLE);
    SetConsoleCursorPosition(output, pos);
}

struct Bod2d
{
    double x;
    double y;

    Bod2d(const double x, const double y) : x(x), y(y)
    {

    }

    Bod2d& operator += (const Bod2d& other)
    {
        x += other.x;
        y += other.y;

        return *this;
    }

    // 🚀 vytvorte operator -=
    Bod2d& operator -= (const Bod2d& other)
    {
        x -= other.x;
        y -= other.y;

        return *this;
    }

    friend Bod2d operator - (const Bod2d& A, const Bod2d& B)
    {
        return Bod2d(A.x - B.x, A.y - B.y);
    }
};

class Platno
{
private:
    string retezec;
public:
    const int sirka;
    const int vyska;

    Platno(const int sirka, const int vyska) : retezec((sirka + 1)* vyska, '-'), sirka(sirka), vyska(vyska)
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

        if (ix < 0.0 || ix >= sirka || iy < 0.0 || iy >= vyska)
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

Bod2d rotace(const Bod2d A, const double uhelStupne)
{
    const double uhelRadiany = (uhelStupne * M_PI) / 180;

    const double xt = A.x * cos(uhelRadiany) - A.y * sin(uhelRadiany);
    const double yt = A.x * sin(uhelRadiany) + A.y * cos(uhelRadiany);

    const Bod2d At(xt, yt);

    return At;
}

// 🚀 Zde nadefinujte Usecku
class Usecka
{
public:
    Bod2d A;
    Bod2d B;
    double uhelStupne;

    Usecka(Bod2d A, Bod2d B): A(A), B(B), uhelStupne(0.0)
    {
    }

    void ZmenRotaci(double novyUhel) 
    {
        uhelStupne = novyUhel;
    }

    void Nakresli(Platno* platno) 
    {
        Bod2d stred((A.x + B.x) / 2, (A.y + B.y) / 2);

        Bod2d At = A - stred;
        Bod2d Bt = B - stred;

        At = rotace(At, uhelStupne);
        Bt = rotace(Bt, uhelStupne);

        At += stred;
        Bt += stred;

        platno->NakresliUsecku(At, Bt);
    }
};
int main()
{
    Platno platno(40, 20);

    const Bod2d A(10.0, 10.0);
    const Bod2d B(30.0, 10.0);

    Usecka usecka(A,B);

    double stupne = 0.0;

    while (true)
    {
        usecka.ZmenRotaci(stupne);

        stupne += 0.02;

        platno.Vymaz();

        usecka.Nakresli(&platno);

        gotoxy(0, 0);

        platno.Zobraz();
    }

    return 0;
}
