#include <stdio.h>
int main()
{
    int pole[] = { 1, 9, 8, 4, 7, 3, 1, 4, 5, 6 };

    // 1 % 2 = 0 * 2 + 1
    // spocitejte a vypiste pocet sudych cisel v poli
    // cislo % 2 == 0

    int pocet = 0;

    for (int i = 0; i < 10; i++)
    {
        int prvek = pole[i];

        if (prvek % 2 == 0)
        {
            ++pocet;
        }
    }

    printf("pocet sudych: %d\n", pocet);
}
