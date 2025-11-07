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

    friend Bod2d operator + (const Bod2d& A, const Bod2d& B)
    {
        return Bod2d(A.x + B.x, A.y + B.y);
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

// 🚀 Přepracujte úsečku
// Tak že zadáme souřadnici středu úsečky, délku úsečky
// a výchozí úhel bude 0.0

class Usecka
{
public:
    Bod2d S;
    double delka;
    double uhelStupne;

    Usecka(Bod2d S, double delka) : S(S), delka(delka), uhelStupne(0.0)
    {
    }

    void ZmenRotaci(double novyUhel)
    {
        uhelStupne = novyUhel;
    }

    void Nakresli(Platno* platno)
    {
        Bod2d A (S.x - delka / 2, S.y);
        Bod2d B(S.x + delka / 2, S.y);

        Bod2d At = A - S;
        Bod2d Bt = B - S;

        At = rotace(At, uhelStupne);
        Bt = rotace(Bt, uhelStupne);

        At += S;
        Bt += S;

        platno->NakresliUsecku(At, Bt);
    }
};

// 1. Přepracujte trojuhelník aby rotoval obdobným způsobem jako Usecka.
// Poznámka: úsečka i trojúhleník budou rotovat různou rychlostí i směrem

class RovnostrannyTrojuhelnik
{
private:
    Bod2d S;
    double a;
    double uhel_rotace;
public:
    RovnostrannyTrojuhelnik(const Bod2d S, const double a) : S(S), a(a), uhel_rotace(0.0)
    {
    }

    void ZmenRotaci(double novyUhel)
    {
        uhel_rotace = novyUhel;
    }

    void Nakresli(Platno* platno) const
    {
        Bod2d A(S.x - a / 2, S.y - sqrt(3.0) * a / 6);
        Bod2d B(S.x + a / 2, S.y - sqrt(3.0) * a / 6);
        Bod2d C(S.x, S.y + sqrt(3.0) * a / 3);

        Bod2d At = rotace(A - S, uhel_rotace) + S;
        Bod2d Bt = rotace(B - S, uhel_rotace) + S;
        Bod2d Ct = rotace(C - S, uhel_rotace) + S;

        platno->NakresliUsecku(At, Bt);
        platno->NakresliUsecku(Bt, Ct);
        platno->NakresliUsecku(Ct, At);
    }
};


int main()
{
    Platno platno(40, 20);

    Bod2d S(20.0, 10.0);

    Usecka usecka(S, 5.0);
    // Definujte trojuhelnik a v cyklu while ho zarotujte
    RovnostrannyTrojuhelnik trojuhelnik(Bod2d(10.0,5.0), 8.0);
    double stupne = 0.0;
    double stupne_op = 0.0;
    while (true)
    {
        trojuhelnik.ZmenRotaci(stupne_op);
        usecka.ZmenRotaci(stupne);

        stupne += 0.02;
        stupne_op -= 0.1;

        

        platno.Vymaz();

        trojuhelnik.Nakresli(&platno);
        usecka.Nakresli(&platno);

        gotoxy(0, 0);

        platno.Zobraz();
    }

    return 0;
}
