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

    if (bmi > 25) 
        puts("Kup si hubnouci dietu");
    else 
        puts("Kup si neco k jidlu");

    // UKOL: vypiste textove kategorii

    if (bmi <= 16.5)
    {
        puts("Tezka podvyziva");
    }
    else if (bmi <= 18.5)
    {
        puts("podvaha");
    }
    else if (bmi <= 25.0)
    {
        puts("idealni vaha");
    }
    else if (bmi <= 30.0)
    {
        puts("nadvaha");
    }
    else if (bmi <= 35.0)
    {
        puts("obezita prvniho stupne");
    }
    else if (bmi <= 40.0)
    {
        puts("obezita druheho stupne");
    }
    else
    {
        puts("morbidni obezita");
    }
}
