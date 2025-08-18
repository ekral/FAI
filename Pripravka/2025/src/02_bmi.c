#include <stdio.h>

int main()
{
    double hmotnost = 0.0;
    double vyskaCentimetry = 0.0;

    printf("Zadej hmotnost: ");
    // funce scanf_s ma jako argument adresu kam ma ulozit vysledek
    scanf_s("%lf", &hmotnost); 

    // Ukol: prepracujte zadavani na vysku v centimetrech
    printf("Zadej vysku v centimetrech: ");
    scanf_s("%lf", &vyskaCentimetry);

    double vyskaMetry = vyskaCentimetry / 100.0;

    double bmi = hmotnost / (vyskaMetry * vyskaMetry);

    printf("bmi: %lf\n", bmi);
}

