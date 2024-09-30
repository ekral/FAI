# Vykreslení úsečky

Zapište do matice pomocí znaků `x` úsečku z bodu `p1` do bodu `p2`.

```c
#include <stdio.h>

#define POCET_RADKU 20
#define POCET_SLOUPCU 20
struct Bod
{
    int x;
    int y;
};

int main(void)
{
    struct Bod p1 = { 1, 2};
    struct Bod p2 = { 3, 4};

    char matice[POCET_RADKU][POCET_SLOUPCU];

    for (int i = 0; i < POCET_SLOUPCU; i++)
    {
        for (int j = 0; j < POCET_RADKU; j++)
        {
            matice[i][j] = '-';
        }
    }

    // zapiste do matice pomoci znaku x usecku z bodu p1 do bodu p2

    for (int i = 0; i < POCET_SLOUPCU; i++)
    {
        for (int j = 0; j < POCET_RADKU; j++)
        {
            putchar(matice[i][j]);
        }
        putchar('\n');
    }
    return 0;
}

```
