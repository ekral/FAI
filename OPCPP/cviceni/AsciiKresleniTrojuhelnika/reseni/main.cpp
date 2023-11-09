#include <cstdio>
#define _USE_MATH_DEFINES
#include <cmath>
#include <vector>
#include <iostream>
#include <sstream>


struct Bod2d
{
    double x;
    double y;

    Bod2d(double x, double y) : x(x), y(y)
    {

    }
};

class Platno
{
private:
    const int columnCount;
    const int rowCount;
    const int totalChars;
    char pozadi;

    std::vector<char> data;
public:
    const int maxColumnIndex;
    const int maxRowIndex;

    char popredi;

    Platno(int columnCount, int rowCount, char pozadi, char popredi) :
            columnCount(columnCount),
            rowCount(rowCount),
            pozadi(pozadi),
            popredi(popredi),
            totalChars(columnCount* rowCount),
            maxColumnIndex(columnCount - 1),
            maxRowIndex(rowCount - 1),
            data(totalChars, 0)
    {

        Vymaz();
    }

    void Vymaz()
    {
        for (int i = 0; i < totalChars; i++)
        {
            data[i] = pozadi;
        }
    }

    void NakresliBod(Bod2d bod)
    {
        NakresliBod(bod.x, bod.y);
    }

    void NakresliBod(double x, double y)
    {
        int rowIndex = (int)round(y);
        int columnIndex = (int)round(x);

        if ((rowIndex < 0) || (rowIndex > maxRowIndex) || (columnIndex < 0) || (columnIndex > maxColumnIndex))
        {
            return;
        }

        int pos = ((rowCount - rowIndex - 1) * columnCount) + columnIndex;

        data[pos] = popredi;
    }

    void NakresliUsecku(Bod2d bodA, Bod2d bodB)
    {
        double dx = bodB.x - bodA.x;
        double dy = bodB.y - bodA.y;

        double dmax = fmax(fabs(dx), fabs(dy));

        double stepx = dx / dmax;
        double stepy = dy / dmax;

        Bod2d bod = bodA;

        double d = 0;

        while (d <= dmax)
        {
            NakresliBod(bod.x, bod.y);

            bod.x += stepx;
            bod.y += stepy;

            ++d;
        }

    }

    void Zobraz()
    {
        std::stringstream ss;

        int pos = 0;

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                char znak = data[pos];
                ++pos;

                ss << znak;
                ss << znak;
            }

            ss << '\n';
        }

        std::string retezec = ss.str();

        std::cout << retezec;

        //puts(retezec.c_str());
    }

};

Bod2d Rotuj(Bod2d bod, double stupne)
{
    double uhelRadiany = (stupne * M_PI) / 180.0;

    double xt = (bod.x * cos(uhelRadiany)) - (bod.y * sin(uhelRadiany));
    double yt = (bod.x * sin(uhelRadiany)) + (bod.y * cos(uhelRadiany));

    return Bod2d{ xt, yt };
}

Bod2d Rotuj(Bod2d bod, double stupne, Bod2d S)
{
    bod.x -= S.x;
    bod.y -= S.y;

    bod = Rotuj(bod, stupne);

    bod.x += S.x;
    bod.y += S.y;

    return bod;
}

class RovnostrannyTrojuhelnik
{
private:
    double a;
    Bod2d S;
    // 🐱‍👤 Pridejte uhel rotace

public:
    RovnostrannyTrojuhelnik(Bod2d S, int a) : S(S), a(a)
    {

    }

    void Nakresli(Platno& platno) const
    {
        // spocitejte souradnice vrcholu trojuhelnika
        double R = (a * sqrt(3.0)) / 3;
        double r = R / 2.0;

        Bod2d A(S.x - a / 2, S.y - r);
        Bod2d B(S.x + a / 2, S.y - r);
        Bod2d C(S.x, S.y + R);

        // 🚀 Zarotujte body kolem stredu

        platno.NakresliUsecku(A, B);
        platno.NakresliUsecku(B, C);
        platno.NakresliUsecku(C, A);

        platno.NakresliBod(S);
    }
};

int main()
{
    int columnCount = 30;
    int rowCount = 20;

    Platno platno(columnCount, rowCount, '-', 'x');

    RovnostrannyTrojuhelnik trojuhelnik(Bod2d(15.0, 10.0), 8);

    bool konec = true;

    platno.Vymaz();

    trojuhelnik.Nakresli(platno);

    platno.Zobraz();

    getchar();
}
