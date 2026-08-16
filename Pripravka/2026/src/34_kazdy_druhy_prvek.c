#include <stdio.h>

int main()
{
    int pole[] = { 1, 8, 9, 7, 2, 3, 5 };
    int n = 7;
    // Pomoci cyklu for vypiste kazdy druhy prvek v poli
    // 8
    // 7
    // 3

    for (int i = 1; i < n; i += 2)
    {
        printf("%d\n", pole[i]);
    }
}
