#include <stdio.h>
#define _USE_MATH_DEFINES
#include <math.h>

int main()
{
    double r = 5.0;
    puts("Zadej polomer");
    scanf_s("%lf", &r);

    double obvod = 2 * M_PI * r;
    double obsah = M_PI * r * r;
    
    printf("obvod: %lf\n", obvod);
    printf("obsah: %lf\n", obsah);
}

