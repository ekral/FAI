#include <stdlib.h>
#include <stdio.h>
#include <math.h>

struct Bod2d
{
    double x;
    double y;
};

class Platno
{
private:
    int pocetRadku;
    int pocetSloupcu;
    int pocetZnaku;
    char* data;

public:
    // Konstruktor a za : je member initializer list
    Platno(int pocetRadku, int pocetSloupcu) : 
        pocetRadku(pocetRadku),
        pocetSloupcu(pocetSloupcu),
        pocetZnaku(pocetRadku * pocetSloupcu),
        data(new char[pocetZnaku])
    {

    }

    // Destruktor
    ~Platno()
    {
        delete[] data;
    }

    void Vymaz()
    {
        for (int i = 0; i < pocetZnaku; i++)
        {
            data[i] = '-';
        }
    }

    void NakresliBod(double x, double y)
    {
        int indexRadku = (int)round(y);
        int indexSloupce = (int)round(x);

        int index = indexRadku * pocetSloupcu + indexSloupce;

        data[index] = 'o';
    }

    void Zobraz()
    {
        int index = 0;

        for (size_t i = 0; i < pocetRadku; i++)
        {
            for (size_t i = 0; i < pocetSloupcu; i++)
            {
                char znak = data[index];
                putchar(znak);

                ++index;
            }

            putchar('\n');
        }
    }
};


int main()
{

    // Klientsky kod
    Platno platno(20,30);
 

    platno.Vymaz();

    platno.NakresliBod(2.0, 3.0);
    
    platno.Zobraz();


    return 0;
} 