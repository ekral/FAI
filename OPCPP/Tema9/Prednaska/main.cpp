#include <stdio.h>
#include "obdelnik.h"


int main()
{
    int* x = 0;
    int* y = (int*)11;

    int r = y - x; // (8 - 0) / sizeof(int)
    printf("r: %d\n", r);

    int* p3 = 0;
    int* p4 = p3 + 2;
    int rozdil = p4 - p3;
    printf("rozdil: %d\n", rozdil);

    int pole[] = { 1,2,3,4 };
    int* p = &pole[0];
    ++p;
    *p = 7;

    int* p2 = &pole[0];
    int* konec = p2 + 4;

    while (p2 < konec)
    {
        printf("%d ", *p2);
        ++p2;
    }

    /*for (int i = 0; i < 4; i++)
    {
        printf("%d ", pole[i]);
    }*/
}

