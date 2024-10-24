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

class Usecka
{
private:
    Bod2d A;
    Bod2d B;
    Bod2d S;

    double uhel;

public:
    Usecka(const Bod2d A, const Bod2d B) : A(A), B(B), S(A.x + (B.x - A.x) / 2.0, A.y + (B.y - A.y) / 2.0), uhel(0.0)
    {

    }

    void ZmenRotaci(const double uhel)
    {
        this->uhel = uhel;

        A = rotace(A, S, uhel);
        B = rotace(B, S, uhel);
    }

    void Nakresli(Platno* platno) const
    {
        platno->NakresliUsecku(A, B);
    }
};

int main()
{
    Platno platno(40, 20);

    platno.Vymaz();

    const Bod2d A(10.0, 10.0);
    const Bod2d B(30.0, 10.0);

    Usecka usecka(A,B);
    usecka.ZmenRotaci(1.0);

    usecka.Nakresli(&platno);

    platno.Zobraz();

    return 0;
}
