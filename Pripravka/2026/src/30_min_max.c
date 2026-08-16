#include <stdio.h>
int main()
{
    // 2. Zadejte na konzoli minimum a maximum
    // a vypiste cela cisla z rozsahu <minimum,maximum>
    // Zadejte tedy nejmensi a nejvetsi cislo a
    // vypiste cela cisla v tomto rozsahu
    // overte ze maximum je vetsi nebo rovno minimum
    // 5    1
    // 10   3
    //     
    // 5    1
    // 6    2
    // 7    3
    // 8
    // 9
    // 10

    int min;
    scanf_s("%d", &min);

    int max;
    scanf_s("%d", &max);
    
    if (max < min)
    {
        puts("Maximum musi byt vetsi nebo rovno minimum.");
        return 0;
    }
   
    for (int i = min; i <= max; i++)
    {
        printf("%d\n", i);
    }
}
