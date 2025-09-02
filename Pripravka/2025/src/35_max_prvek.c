#include <stdio.h>

int main()
{
    int pole[] = { 1, 8, 9, 7, 2, 3, 5 };
    int n = 7;
    // Najdete a vypiste hodnotu nejvetsiho prvku v poli

    int max = 0;
    for (int i = 0; i < n; i++)
    {
        int prvek = pole[i];
        if (prvek > max)
        {
            max = prvek;
        }
    }

    printf("max: %d\n", max);
}
