#include <stdio.h>
#include <math.h>

// Definujte strukturu Bod2d aby byl platny kod ve funkci main.
// Pouzijte konstruktor a member initializer list
// Otestuje co udelaji klicoval slovat private a public
// A co se stane, kdyz zmenim Bod2d na tridu.
// Funkci main nemente.

struct Bod2d
{
    double x;
    double y;

    Bod2d(double x, double y) : x(x), y(y)
    {

    }
};

int main()
{
    
    Bod2d bodA(2.0, 3.0);
    printf("x: %lf y: %lf", bodA.x, bodA.y);
}

