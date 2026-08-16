#include <stdio.h>
int main()
{
    // zadejte na konzoli liche cislo
    // vykreslete pomoci hvezdicek pyramidu s poctem radku
    // rovnajici se zadanemu cislu

    // 5
    //    *
    //   ***
    //  *****
    // *******
    //*********

    puts("Zadej liche cislo:");
    int pocet = 0;
    scanf_s("%d", &pocet);
    // TODO otestovat zda je pocet lichy
    for (int i = 0; i < pocet; i++)
    {
        for (int j = 0; j < pocet - 1 - i; j++)
        {
            putchar(' ');
        }

        for (int k = 0; k < (i * 2) + 1; k++)
        {
            putchar('*');
        }
        
        putchar('\n');
    }
}
