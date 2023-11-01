#include <cstdio>
#define _USE_MATH_DEFINES
#include <cmath>
#include <vector>
#include <sstream>
#include <iostream>
#include <windows.h>

void gotoxy(int x, int y) {
    COORD pos = { (SHORT)x, (SHORT)y };
    HANDLE output = GetStdHandle(STD_OUTPUT_HANDLE);
    SetConsoleCursorPosition(output, pos);
}

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
    // static constexpr je moderni zpusob zadani konstanty zname v dobe prekladu
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

    // 😲
    void NakresliBod(Bod2d bod)
    {
        NakresliBod(bod.x, bod.y);
    }

    void NakresliBod(double x, double y)
    {
        int rowIndex = (int)round(y);
        int columnIndex = (int)round(x);

        // pokud je rowIndex nebo columnIndex mimo rozsah, tak bod nevykresli

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
            }

            ss << '\n';
        }

        std::string retezec = ss.str();
        
        std::cout << retezec;

        //puts(retezec.c_str());
    }

};

class RovnostrannyTrojuhelnik
{
private:
    double a;
    Bod2d S;
public:
    RovnostrannyTrojuhelnik(Bod2d S, int a) : S(S), a(a)
    {

    }

    void Nakresli(Platno* platno) const
    {
        // spocitejte souradnice vrcholu trojuhelnika
        double vp = (a * sqrt(3.0)) / 4;

        Bod2d A(S.x - a / 2, S.y - vp);
        Bod2d B(S.x + a / 2, S.y - vp);
        Bod2d C(S.x, S.y + vp);

        platno->NakresliUsecku(A, B);
        platno->NakresliUsecku(B, C);
        platno->NakresliUsecku(C, A);
    }
};

Bod2d Rotuj(Bod2d bod, double stupne)
{
    double uhelRadiany = (stupne * M_PI) / 180.0;

    double xt = (bod.x * cos(uhelRadiany)) - (bod.y * sin(uhelRadiany));
    double yt = (bod.x * sin(uhelRadiany)) + (bod.y * cos(uhelRadiany));

    return Bod2d{xt, yt};
}

Bod2d Rotuj(Bod2d bod, double stupne, Bod2d stred)
{
    Bod2d bodT(bod.x - stred.x, bod.y - stred.y);

    bodT = Rotuj(bodT, stupne);

    bodT.x += stred.x;
    bodT.y += stred.y;

    return bodT;
}

int main()
{
    Bod2d A(7.0, 8.0);
    Bod2d B(10.0, 12.0);

    Bod2d S((A.x + B.x) / 2.0, (A.y + B.y) / 2.0); // 🚗 ✔

    int columnCount = 30;
    int rowCount = 30;

    Platno platno(columnCount, rowCount, '-', 'x');

    bool konec = true;
    double uhelStupne = 0;

    do
    {
        platno.Vymaz();

        Bod2d At = Rotuj(A, uhelStupne, S);
        Bod2d Bt = Rotuj(B, uhelStupne, S);

        platno.popredi = 'x';
        platno.NakresliUsecku(At, Bt);

        platno.popredi = 'A';
        platno.NakresliBod(At);

        platno.popredi = 'S';
        platno.NakresliBod(S);

        platno.popredi = 'B';
        platno.NakresliBod(Bt);

        gotoxy(0, 0);
        platno.Zobraz();
        uhelStupne += 1.0;

    } while (uhelStupne < 360.0);
}