// MojeDalsiAplikace.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <stdio.h>

int main()
{
    // Uzivatel zada na konzoli vysi hrube mzdy
    // a pocet deti
    // Program vypise cistou mzdu
    // 7.1% socialni pojisteni
    // 4.5% zdravotni pojisteni
    // 15% zaloha na DP - 2570,-

    double hrubaMzda = 0.0;
    puts("Zadej vysi hrube mzdy");
    scanf_s("%lf", &hrubaMzda);

    int pocetDeti = 0;
    puts("Zadej pocet deti");
    scanf_s("%d", &pocetDeti);

    double socialniZamestnavatel = 0.248 * hrubaMzda;
    double zdravotniZamestnavatel = 0.09 * hrubaMzda;

    double socialni = 0.071 * hrubaMzda;
    double zdravotni = 0.045 * hrubaMzda;
    double dp = 0.15 * hrubaMzda;

    double slevaNaDite = 0.0;

    if (pocetDeti == 1)
    {
        slevaNaDite = 1267;
    }
    else if (pocetDeti == 2)
    {
        slevaNaDite = 1267 + 1860;
    }
    else if (pocetDeti == 3)
    {
        slevaNaDite = 1267 + 1860 + 2320;
    }
    else if (pocetDeti > 3)
    {
        int pocet = pocetDeti - 3;
        slevaNaDite = 1267 + 1860 + 2320 + (pocet * 2320);
    }

    dp -= 2570;
    dp -= slevaNaDite;

    if (dp > 0 && dp < 100)
    {
        dp = 0;
    }

    double cistaMzda = hrubaMzda - socialni - zdravotni - dp;
    double celkoveMzdoveNaklady = hrubaMzda + socialniZamestnavatel + zdravotniZamestnavatel;
    double celkoveOdvody = socialniZamestnavatel + zdravotniZamestnavatel + socialni + zdravotni + dp;
    
    printf("Hruba mzda\t\t%20.2lf\n", hrubaMzda);
    printf("Socialni pojisteni\t%20.2lf\n", -socialni);
    printf("Zdravotni pojisteni\t%20.2lf\n", -zdravotni);
    printf("Dan z prijmu\t\t%20.2lf\n", -dp);
    printf("Cista mzda\t\t%20.2lf\n", cistaMzda);
    printf("Celkove naklady na mzdu\t%20.2lf\n", celkoveMzdoveNaklady);
    printf("Celkove odvody\t\t%20.2lf\n", celkoveOdvody);
}
