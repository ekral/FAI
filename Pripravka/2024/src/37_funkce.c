#include <stdio.h>

void prohod(int* x, int* y)
{
    int tmp = *x;
    *x = *y;
    *y = tmp;
}

void vypis(const int* pole, int delka)
{
    // vypiste prvky pole na konzoli
    for (int i = 0; i < delka; i++)
    {
        printf("%d ", pole[i]);
    }

    putchar('\n');
}

int max(const int* pole, int delka)
{
    int max = 0;

    // najdete maximalni hodnotu prvku v poli

    return max;
}

int main()
{
    int x = 2;
    int y = 3;

    prohod(&x, &y);

    int pole[] = { 1, 8, 9, 7, 2, 3, 5, 7, 8, 9 };
    int n = sizeof(pole) / sizeof(int);
    
    vypis(pole, n);

    for (int i = 0; i < n / 2; i++)
    {
        prohod(&pole[i], &pole[n - (1 + i)]);
    }

    vypis(pole, n);
    
    int m = max(pole, n);
}
