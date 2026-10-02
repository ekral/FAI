#include <stdio.h>

int main()
{
    int a = 2;
    int b = 3;
    int c = 5;
    
    // 1. Vypiste na konzoli hodnotu nejvetsiho cisla ze tri promennych a,b,c
    if (a > b && a > c)
    {
        printf("%d\n", a);
    }
    else if(b > c)
    {
        printf("%d\n", b);
    }
    else
    {
        printf("%d\n", c);
    }


    if (a > b)
    {
        if (a > c)
        {
            printf("%d\n", a);
        }
        else
        {
            printf("%d\n", c);
        }
    }
    else
    {
        if (b > c)
        {
            printf("%d\n", b);
        }
        else
        {
            printf("%d\n", c);
        }
    }
}
