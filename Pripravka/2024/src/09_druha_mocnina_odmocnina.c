#include <stdio.h>
#include <math.h>

int main()
{
    double x;

    // nactete hodnotu x z konzole pomoci funkce 
    // scanf_s
    printf("Zadej hodnotu x\n");
    scanf_s("%lf", &x);

    // spocitejte a vypiste druhou mocninu x
    double mocnina = x * x;
    printf("mocnina: %lf\n", mocnina);

    // spocitejte a vypiste druhou odmocninu x
    double odmocnina = sqrt(x);
    printf("odmocnina: %lf\n", odmocnina);
}
