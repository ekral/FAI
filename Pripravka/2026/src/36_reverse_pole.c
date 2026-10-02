#include <stdio.h>

int main()
{
    int pole[] = { 1, 8, 9, 7, 2, 3, 5, 7, 8, 9 };
    int n = sizeof(pole) / sizeof(int);
    
    // prohodte hodnoty prvniho a posledniho prvku
    for (int i = 0; i < n / 2; i++)
    {
        int tmp = pole[i];
        pole[i] = pole[n - (1 + i)];
        pole[n - (1 + i)] = tmp;
    }
    
}
