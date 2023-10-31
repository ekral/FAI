# Ascii Rotace trojúhelníka

Zarotujte rovnostranný trojúhleník kolem jeho středu. Do trojúhleníka přidejte členskou proměnnou rotace. Samotnou rotaci vyřešte v členské funkci třídy `RovnostrannyTrojuhelnik` `Nakresli`.

Použijte vzorec:

$$\begin{align*}
x' &= x \cdot \cos(\theta) - y \cdot \sin(\theta) \\
y' &= x \cdot \sin(\theta) + y \cdot \cos(\theta)
\end{align*}$$

$$\begin{align*}
\text{ Kde} (x', y') \text{ jsou souřadnice zarotovaného bodu, }
(x, y) \text{ jsou souřadnice původního bodu a }
\theta \text{ je úhel rotace v radiánech.}
\end{align*}$$

.

```cpp
#include <cstdio>
#define _USE_MATH_DEFINES
#include <cmath>
#include <vector>
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
        int pos = 0;

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                char znak = data[pos];
                ++pos;

                putchar(znak);
                putchar(znak);
            }

            putchar('\n');
        }
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

int main()
{
    Bod2d bodA(2.0, 3.0);
    Bod2d bodB(5.0, 6.0);

    Bod2d stred(0.0, 0.0); // 🚗

    int columnCount = 30;
    int rowCount = 30;

    Platno platno(columnCount, rowCount, '-', 'x');

    bool konec = true;
    double uhelStupne = 0;

    do
    {
        platno.Vymaz();

        platno.NakresliBod(2, 3);

        platno.popredi = 'O';
        platno.NakresliBod(0, 0);

        platno.popredi = '1';
        platno.NakresliBod(platno.maxColumnIndex, 0);

        platno.popredi = '2';
        platno.NakresliBod(platno.maxColumnIndex, platno.maxRowIndex);

        platno.popredi = '3';
        platno.NakresliBod(0, platno.maxRowIndex);

      

        platno.popredi = 'S';
        Bod2d stred(10.0, 8.0);
        platno.NakresliBod(stred.x, stred.y);

        // Odpoznamkovat
        platno.popredi = 't';
        RovnostrannyTrojuhelnik trojuhelnik(stred, 10.0);
        trojuhelnik.Nakresli(&platno);


        double uhelRadiany = (uhelStupne * M_PI) / 180.0;

        // 🍌
        
        //Bod2d At = Rotuj(A, uhelRadiany, Bod2d stred);
        //Bod2d Bt = Rotuj(B, uhelRadiany, Bod2d stred);

        // Predelat na funkci 🛴
        //double xt = (x * cos(uhelRadiany)) - (y * sin(uhelRadiany));
        //double yt = (x * sin(uhelRadiany)) + (y * cos(uhelRadiany));
        // Predelat na funkci 🛴

        platno.popredi = 'A';
        platno.NakresliUsecku(At, Bt);

        // 🍌

        gotoxy(0, 0);
        platno.Zobraz();
        uhelStupne += 1;
    } while (uhelStupne < 90);
}
```