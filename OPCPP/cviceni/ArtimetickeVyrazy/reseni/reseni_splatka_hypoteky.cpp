#include <stdio.h>
#define _USE_MATH_DEFINES
#include <math.h>

int main()
{
    double p = 6000000.0;
    int rocniUrokProcenta = 6.0;
    int pocetLet = 30;
    double r = (rocniUrokProcenta/100.0)/12;
    int n = pocetLet * 12;

    double m = (p * r * (pow(1+r,n)))/ (pow(1+r,n)-1);

    printf("splatka je %lf\n", m); // 35973.03 


    while(p > 0)
    {
        double nom = r * p;
        double umor = m - nom;
        p = p - umor;

        printf("aktualni dluh %lf\t", p);
        printf("nominalni vyse uroku %lf\t", nom);
        printf("Umor %lf\n", umor);
    }
}
