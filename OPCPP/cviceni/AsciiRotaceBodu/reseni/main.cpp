#include <stdio.h>
#define _USE_MATH_DEFINES
#include <math.h>
#include <vector>
#include <sstream>
#include <iostream>
#include <windows.h>

void gotoxy(int x, int y) 
{
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

    void NakresliBod(Bod2d bod)
    {
        NakresliBod(bod.x, bod.y);
    }

    void NakresliBod(double x, double y)
    {
        int rowIndex = (int)round(y);
        int columnIndex = (int)round(x);
        
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

    void Nakresli(Platno* platno)
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

// Zde definujte globalni funkci 🚀
Bod2d Rotace(Bod2d A, double stupne) 
{
    double uhelradian = (stupne / 180) * M_PI;
    double xt = A.x * cos(uhelradian) - A.y * sin(uhelradian);
    double yt = A.x * sin(uhelradian) + A.y * cos(uhelradian);
    Bod2d bod(xt, yt);
    return bod;
}

int main()
{
    int columnCount = 30;
    int rowCount = 20;

    Platno platno(columnCount, rowCount, '-', 'x');

    double stupne = 0.0;

    do
    {
        platno.Vymaz();

        Bod2d A(10.0, 0.0);
        Bod2d S(0.0, 0.0);
        
        Bod2d At = Rotace(A, stupne);
        platno.NakresliUsecku(At, S);

        gotoxy(0, 0);

        platno.Zobraz();

        stupne += 0.1;

    } while (stupne < 90.0);
}
