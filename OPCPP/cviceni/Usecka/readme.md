# Vykreslení úsečky

Zapište do matice pomocí znaků `x` úsečku z bodu `A` do bodu `B`.

- Při vykreslování nezapomeňte otestovat všechny směry úsečky.
- Zvolte jakýkoliv algoritmus, nemusí být optimální, přednost má čitelnost kódu.
- Absolutní hodnotu zjistíme funkcí `fabs` a maximum `fmax`.

Naivní řešení:
1) Máme bod A = (x1,y1) a B = (x2, y2).
2) Spočítáme (dx, dy) = B - A.
3) dmax = fmax(fabs(dx), fabs(dy)).
4) stepx = dx / dmax a stepy = dy / dmax.
5) Provádíme A + (stepx, stepy) dokud je hodnota přibližně rovna B (například můžeme počítat od 0.0 po 1.0 do dmax).
   
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
