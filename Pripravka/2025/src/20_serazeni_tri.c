#include <stdio.h>


int main()
{
    // Seradte hodnoty tak, aby 
    // v promenne a byla nejvetsi hodnota
    // v promenne b druha nejvetsi hodnota
    // v promenne c nejmensi hodnota

    int a = 3;
    int b = 5;
    int c = 7;

    if (a < b)
    {
        int tmp = a;
        a = b;
        b = tmp;
    }

    if (b < c)
    {
        int tmp = b;
        b = c;
        c = tmp;
    }

    if (a < b)
    {
        int tmp = a;
        a = b;
        b = tmp;
    }
}
