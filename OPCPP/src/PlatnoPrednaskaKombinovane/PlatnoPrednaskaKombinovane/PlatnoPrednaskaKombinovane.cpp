#include <math.h>

struct Bod2d
{
    double x;
    double y;
};

class Platno
{
private:
    int pocetRadku = 20;
    int pocetSloupcu = 30;
    int pocetZnaku = pocetRadku * pocetSloupcu;
    char data[20 * 30];
public:
    void NakresliBod(double x, double y)
    {
        int indexRadku = (int)round(y);
        int indexSloupce = (int)round(x);

        int index = indexRadku * pocetSloupcu + indexSloupce;

        data[index] = 'o';
    }
};


int main()
{
    // Klientsky kod
    Platno platno;
    platno.NakresliBod(2.0, 3.0);
    return 0;
} 