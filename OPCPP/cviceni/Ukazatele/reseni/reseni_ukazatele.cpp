#include <assert.h>
#include <stdio.h>

// pouzijte ukazatel

// typ reference
void prohod(int *x, int *y) // typ ukazatel
{
    int tmp = *x;   // *...operátor indirekce nebo taky dereference,
    *x = *y;        //  musim prohodit hodnotu na kterou ukazatel ukazuje, ne jen adresu
    *y = tmp;
}

int main()
{
    int x = 2;
    int y = 3;

    prohod(&x, &y); // &...adresní operátor nbo taky operator reference

    printf("x: %d y: %d\n", x, y);

    assert(x == 3);
    assert(y == 2);
}
