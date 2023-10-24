#include <stdio.h>
#define _USE_MATH_DEFINES
#include <math.h>
#include <vector>
#include <algorithm>
#include <Windows.h>

void gotoxy(int x, int y) {
    COORD pos = { x, y };
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
        totalChars((columnCount* rowCount) + rowCount),
        maxColumnIndex(columnCount - 1),
        maxRowIndex(rowCount - 1),
        data(totalChars, 0)
    {

        Vymaz();
    }

    void Vymaz()
    {
        int k = 0;

        for (size_t j = 0; j < rowCount; j++)
        {
            for (size_t i = 0; i < columnCount; i++)
            {
                data[k] = pozadi;
                ++k;
            }

            data[k] = '\n';
            ++k;
        }

        --k;
        data[k] = 0;

    }

    void NakresliBod(double x, double y)
    {
        int pos = ((rowCount - round(y) - 1) * columnCount) + round(x);

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
        char* p = data.data();
        puts(p);
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

    void Nakresli(Platno& platno)
    {
        // spocitejte souradnice vrcholu trojuhelnika
        double vp = (a * sqrt(3.0)) / 4;

        Bod2d A(S.x - a / 2, S.y - vp);
        Bod2d B(S.x + a / 2, S.y - vp);
        Bod2d C(S.x, S.y + vp);

        platno.NakresliUsecku(A, B);
        platno.NakresliUsecku(B, C);
        platno.NakresliUsecku(C, A);
    }
};

int main()
{
    Bod2d bodA(2.0, 3.0);
    Bod2d bodB(5.0, 6.0);

    int columnCount = 3;
    int rowCount = 2;

    Platno platno(columnCount, rowCount, '-', 'x');

    bool konec = true;
    double uhelStupne = 0.0;

    do
    {
        platno.Vymaz();

        platno.popredi = 'O';
        platno.NakresliBod(0, 0);

        platno.popredi = '1';
        platno.NakresliBod(platno.maxColumnIndex, 0);

        platno.popredi = '2';
        platno.NakresliBod(platno.maxColumnIndex, platno.maxRowIndex);

        platno.popredi = '3';
        platno.NakresliBod(0, platno.maxRowIndex);

        platno.popredi = 'A';
        platno.NakresliUsecku(bodA, bodB);

        platno.popredi = 'S';
        Bod2d stred(10.0, 8.0);
        platno.NakresliBod(stred.x, stred.y);

        // Odpoznamkovat
        platno.popredi = 't';
        RovnostrannyTrojuhelnik trojuhelnik(stred, 10.0);
        trojuhelnik.Nakresli(platno);

        double x = min(platno.maxRowIndex, platno.maxColumnIndex);
        double y = 0.0;

        double uhelRadiany = (uhelStupne * M_PI) / 180.0;
        double xt = (x * cos(uhelRadiany)) - (y * sin(uhelRadiany));
        double yt = (x * sin(uhelRadiany)) + (y * cos(uhelRadiany));

        platno.popredi = '*';
        platno.NakresliBod(xt, xt);
        platno.NakresliUsecku(Bod2d(0.0, 0.0), Bod2d(xt, yt));

        gotoxy(0, 0);

        platno.Zobraz();

        uhelStupne += 1;

    } while (uhelStupne < 90);
}