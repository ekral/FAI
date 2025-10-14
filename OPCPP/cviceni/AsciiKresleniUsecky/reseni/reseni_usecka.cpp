#include <stdio.h>
#include <string>
#include <algorithm>
#include <math.h>

using namespace std;

struct Bod2d
{
    double x;
    double y;

    Bod2d()
    {
        x = 0;
        y = 0;
    }

    Bod2d(double x, double y)
    {
        this->x = x;
        this->y = y;
    }
};

class Platno
{
private:
    string retezec;
public:
    int vyska;
    int sirka;

    Platno(int vyska, int sirka) : retezec(vyska* (sirka + 1), ' ')
    {
        this->vyska = vyska;
        this->sirka = sirka;

        Vymaz();
    }

    void Vymaz()
    {
        fill(retezec.begin(), retezec.end(), '-');

        int index = sirka;

        for (int i = 0; i < vyska; i++)
        {
            retezec[index] = '\n';

            index += sirka + 1;
        }
    }

    void NakresliBod(Bod2d bod, char znak)
    {
        NakresliBod(bod.x, bod.y, znak);
    }

    void NakresliBod(double x, double y, char znak)
    {
        int ix = (int)round(x);
        int iy = (int)round(y);

        if (ix < 0 || ix >= sirka || iy < 0 || iy >= vyska)
        {
            return;
        }

        int index = ix + (vyska - iy - 1) * (sirka + 1);

        retezec[index] = znak;
    }
    
    void NakresliUsecku(Bod2d a, Bod2d b, char znak)
    {
        double dx = b.x - a.x;
        double dy = b.y - a.y;

        double max = std::max(abs(dx), abs(dy));

        double stepx = dx / max;
        double stepy = dy / max;

        double x = a.x;
        double y = a.y;

        for (int i = 0; i <= max; i++)
        {
            NakresliBod(x, y, znak);

            x += stepx;
            y += stepy;
        }
    }

    void Zobraz()
    {
        puts(retezec.c_str());
    }
};

int main()
{
    Platno platno(20, 30);

    platno.NakresliBod(5, 3, 'x');

    Bod2d b1(10, 12);
    platno.NakresliBod(b1, 'o');

    platno.NakresliUsecku(Bod2d(0, 0), Bod2d(10, 0), '1');

    platno.Zobraz();

}
