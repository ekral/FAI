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

Bod2d rotace(Bod2d A, double uhelStupne)
{
    double uhelRadiany = (uhelStupne * M_PI) / 180;

    double xt = A.x * cos(uhelRadiany) - A.y * sin(uhelRadiany);
    double yt = A.x * sin(uhelRadiany) + A.y * cos(uhelRadiany);

    Bod2d At(xt, yt);

    return At;
}

int main()
{
    Platno platno(40, 20);

    platno.Vymaz();

    const Bod2d A(20.0, 0.0);

    // 1. rotujte bod A kolem pocatku
    double uhel = 45.0;
    Bod2d At = rotace(A, uhel);

    platno.NakresliBod(At.x, At.y);

    platno.Zobraz();

    return 0;
}
