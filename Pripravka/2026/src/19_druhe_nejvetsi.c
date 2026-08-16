#include <stdio.h>

int main()
{
    int a = 3;
    int b = 5;
    int c = 7;

    // Vypiste na konzoli hodnotu druheho nejvetsiho cisla ze tri promennych a,b,c
    // bac                  cab   
    if ((a > b && a < c) || (a > c && a < b))
    {
        printf("%d\n", a);
    }   
    //      abc                 cba  
    else if((b > a && b < c) || (b > c && b < a))
    {
        printf("%d\n", b);
    }
    else
    {
        printf("%d\n", c);
    }
}
