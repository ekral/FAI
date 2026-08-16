#include <stdio.h>
int main()
{
    // pomoci bubble sortu seradte prvky od nejmensiho po nejvetsi
    int pole[] = { 1, 9, 8, 4, 7, 3, 1, 4, 5, 6 };
    int n = 10;

    for (int j = 0; j < n; j++)
    {

        for (int i = 0; i < n; i++)
        {
            printf("%d ", pole[i]);
        }

        for (int i = 0; i < n - 1 - j; i++)
        {
            if (pole[i] > pole[i + 1])
            {
                int tmp = pole[i];
                pole[i] = pole[i + 1];
                pole[i + 1] = tmp;
            }
        }

        putchar('\n');
    }
}
