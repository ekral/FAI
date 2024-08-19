// MujDruhyProjekt.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

// STanDard Input Output


#include <stdio.h>

int main()
{
    int x = 5; // int je cele cislo se znamenkem
    printf("hodnota x: %d\n", x);
    printf("adresa x: %p\n", &x);
    printf("pocet bytu: %llu\n", sizeof(x)); // nemusite vedet

    double hmotnost = 110.0; // cislo s plovouci carkou se znamenkem

    printf("hodnota hmotnost: %lf\n", hmotnost);
    printf("adresa hmotnost: %p\n", &hmotnost);
    printf("pocet bytu: %llu\n", sizeof(hmotnost));

    // TODO scanf, prumer tri int, obvod a obsah ctverce, obdelniku, kruhu a trojuhelnika
    // https://github.com/ekral/FAI/tree/master/Pripravka/2024
    double vyskaMetry = 1.86;

    // operator deleno je /, napriklad a / b
    // operator nasobeni je *, napriklad a * a
    double bmi = hmotnost / (vyskaMetry * vyskaMetry);

    printf("bmi: %lf\n", bmi);
}

