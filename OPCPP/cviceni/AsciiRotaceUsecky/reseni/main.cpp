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

    Bod2d& operator -= (const Bod2d& other)
    {
        x -= other.x;
        y -= other.y;

        return *this;
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
class Usecka {
public:
    Bod2d A;
    Bod2d B;
    double stupne;

    Usecka(Bod2d A, Bod2d B) : A(A), B(B), stupne(0.0)
    {

    }

    void ZmenRotaci(double stupne) 
    {
        this->stupne = stupne;
    }

    void Nakresli(Platno* platno)
    {
        // 1) Spocitat bod stred
        double x = A.x + ((B.x - A.x) / 2.0);
        double y = A.y + ((B.y - A.y) / 2.0);

        Bod2d S(x, y);

        // 2) Odecist stred S od A a B
        A -= S;
        B -= S;

        // 3) Zarotovat A a B podle stredu souradnic
        Bod2d At = rotace(A, stupne);
        Bod2d Bt = rotace(B, stupne);
        
        // 4) Pricist stred S k At a Bt
        At += S;
        Bt += S;

        // 5) Nakreslit na platno usecku z bodu At do bodu Bt 
        platno->NakresliUsecku(At, Bt);
    }         
};
    
int main()
{
    Platno platno(40, 20);

    const Bod2d A(10.0, 10.0);
    const Bod2d B(30.0, 10.0);

    double stupne = 0.0;

    // 🚀 implementujte Usecku

    while (true)
    {
        Usecka usecka(A, B);
        usecka.ZmenRotaci(stupne);

        stupne += 0.1;

        platno.Vymaz();
        usecka.Nakresli(&platno);
        gotoxy(0, 0);
        platno.Zobraz();
    }

    return 0;
}
