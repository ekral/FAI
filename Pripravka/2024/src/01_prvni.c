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
}

