#include <iostream>
#include <string>
#include <sstream>
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
    string data;
public:
    const int sirka;
    const int vyska;


    Platno(const int sirka, const int vyska) : data((sirka + 1) * vyska, '-'), sirka(sirka), vyska(vyska)
    {
        Vymaz();
    }

    void Vymaz()
    {
        ranges::fill(data, '-');

        for (int i = sirka; i < data.length(); i += sirka + 1)
        {
            data[i] = '\n';
        }

    }

    void Zobraz() const
    {
        cout << data << endl;
    }

    void Zapis(double x, double y)
    {
        const int pos = static_cast<int>(round((vyska - y - 1) * (sirka + 1) + x));

        data[pos] = 'x';
    }
    void NakresliUsecku(Bod2d bodA, Bod2d bodB)
    {
        double dx = bodA.x - bodB.x;
        double dy = bodA.y - bodB.y;

        double dmax = fmax(fabs(dx), fabs(dy));

        double stepy = dy/dmax;
        double stepx = dx/dmax;

        Bod2d bod = bodB;
        double d = 0;
        while(d<dmax) {
            Zapis(bod.x, bod.y);
            bod.x += stepx;
            bod.y += stepy;

            d = d+1;
        }
    }
};

int main()
{
    Platno platno(20, 10);

    platno.Vymaz();

    Bod2d A(2.0, 3.0);
    Bod2d B(7.0, 9.0);

    //platno.Zapis(A.x, A.y);
    //platno.Zapis(B.x, B.y);

    platno.NakresliUsecku(A, B); // 🚀 Implementujte

    platno.Zobraz();

    return 0;
}
