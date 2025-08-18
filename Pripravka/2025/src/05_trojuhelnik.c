#include <stdio.h>
#define _USE_MATH_DEFINES
#include <math.h>

int main()
{
    double a = 5.0;
    double b = 6.0;
    double c = 7.0;

    double obvod = a + b + c;
    double s = obvod / 2.0;
    double obsah = sqrt(s * (s - a) * (s - b) * (s - c));

    printf("obvod: %lf\n", obvod);
    printf("obsah: %lf\n", obsah);
}
