#include <stdio.h>
#include <math.h>

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
    static constexpr int columnCount = 30;
    static constexpr int rowCount = 20;
    static constexpr int totalChars = columnCount * rowCount;
    char pozadi;

    char data[totalChars];
public:
    static constexpr int maxColumnIndex = columnCount - 1;
    static constexpr int maxRowIndex = rowCount - 1;

    char popredi;

    Platno(char pozadi, char popredi) : pozadi(pozadi), popredi(popredi), data{ 0 }
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

        while(d <= dmax)
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
            }

            putchar('\n');
        }
    }

};

class RovnostrannyTrojuhelnik{
public:
    RovnostrannyTrojuhelnik(Bod2d s, int a): s(s), a(a)
    {

    }

    void Nakresli(Platno* platno)
    {

    }
private:
    double a;
    Bod2d s;
};

int main()
{
    Bod2d bodA(2.0, 3.0);
    Bod2d bodB(5.0, 6.0);

    Platno platno('-', 'x');

    bool konec = true;

    do
    {
        platno.Vymaz();

        Bod2d stred(7.0, 8.0);
        
        platno.NakresliBod(2, 3);

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

        platno.popredi = 't';
        RovnostrannyTrojuhelnik trojuhelnik(stred, 5.0);
        trojuhelnik.Nakresli(&platno);
        
        platno.Zobraz();

    } while (!konec);
}