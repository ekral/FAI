#include <stdio.h>
int main()
{
    // 1. Zadejte na konzoli cislo
    // a vypiste tolik znaku * kolik je zadane cislo

    // 5
    // ******

    // 3
    // ***

    int pocet;
    scanf_s("%d", &pocet);

    for (int i = 0; i < pocet; i++)
    {
        putchar('*');
    }
}
