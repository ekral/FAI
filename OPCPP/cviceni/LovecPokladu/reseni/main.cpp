#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#define SIZE 20

int main() 
{
    int arr[SIZE];
    srand((unsigned)time(NULL));

    // naplnění pole náhodnými čísly 1–100
    for (int i = 0; i < SIZE; i++) 
    {
        arr[i] = rand() % 100 + 1;
    }

    // výpis pole pro kontrolu
    printf("Pole hodnot:\n");
    for (int i = 0; i < SIZE; i++) 
    {
        printf("%d ", arr[i]);
    }
    printf("\n");

    /*
        ÚKOL PRO STUDENTY:

        1. Vytvořte ukazatel, který bude ukazovat na začátek pole.
        2. Pomocí ukazatelové aritmetiky:
            - najděte největší číslo (poklad),
            - najděte nejmenší číslo (past).
        3. Určete vzdálenost mezi ukazateli:
            - v počtu objektů typu int,
            - (BONUS) v počtu bajtů po přetypování na (char*).
        4. Výsledek vypište ve formátu:
            Poklad nalezen: X
            Past nalezena: Y
            Vzdálenost mezi nimi: Z objektů typu int
            Vzdálenost mezi nimi (bonus): W bajtů
        5. (Bonus navíc) Simulujte „cestu lovce“.
    */

    int* p_poklad = arr;
    int* p_past = arr;

    for (int* p = arr + 1; p < arr + SIZE; p++)
    {
        // najděte největší číslo(poklad)
        if (*p > *p_poklad)
        {
            p_poklad = p;
        }
        // najděte nejmenší číslo(past)
        if (*p < *p_past)
        {
            p_past = p;
        }
    }
    long long vzdalenost = abs(p_poklad - p_past);
    printf("Poklad nalezen: %d\n", *p_poklad);
    printf("Past nalezen: %d\n", *p_past);
    printf("Vzdalenost mezi nimi: %lld objektu typu int\n", vzdalenost);
    printf("Vzdalenost mezi nimi (bonus): %lld bajtu\n", abs((char*)p_poklad - (char*)p_past));

    return 0;
}
