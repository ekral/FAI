#include <stdio.h>
#include <math.h>

int main()
{
    double z;

    // nactete hodnotu z a n z konzole pomoci funkce 
    // scanf_s
    printf("Zadej hodnotu zakladu z\n");
    scanf_s("%lf", &z);

    double n;
    printf("Zadej hodnotu exponentu n\n");
    scanf_s("%lf", &n);

    // Spocitejte a vypiste n-tou mocninu cisla z (z na n-tou)
    double mocnina = pow(z, n);

    printf("z na n-tou: %lf\n", mocnina);
}
