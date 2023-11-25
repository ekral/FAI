#include <stdio.h>
#define _USE_MATH_DEFINES
#include <math.h>

int main()
{
    double a = 5.0;
    double b = 6.0;
    double c = 7.0;

    double s = (a+b+c)/2;
    double obsah = sqrt(s*(s-a)*(s-b)*(s-c));
 
    printf("obsah je %lf\n", obsah);
}
