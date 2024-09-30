# Vykreslení úsečky

Máme matici znaků reprezentvoanou jednorozměrným polem, kde jsou uloženy řádky ukončené znakem pro nový řádek. Díky tomu pak můžeme matici vypsat jedním příkazem.

Doplnňte kód, který do matice vykreslí následující obrazce:

1) Horní a dolní řádek

xxxxxxxxxx
----------
----------
----------
----------
----------
----------
----------
----------
xxxxxxxxxx

2) Levý a pravý sloupec

x--------x
x--------x
x--------x
x--------x
x--------x
x--------x
x--------x
x--------x
x--------x
x--------x

3) Úhlopříčky

x--------x
-x------x-
--x----x--
---x--x---
----xx----
----xx----
---x--x---
--x----x--
-x------x-
x--------x

3) Trojúhleník

----xx-----
----xx----
---x--x---
---x--x---
--x----x--
--x----x--
-x------x-
-x------x-
x--------x
xxxxxxxxxx  

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
    struct Bod A = { 1, 2};
    struct Bod B = { 3, 4};

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
