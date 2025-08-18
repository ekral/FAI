#include <stdio.h>

int main()
{
    int n = 3;
    
    puts("Zadej delku strany");
    scanf_s("%d", &n);

    int obvod = 4 * n;
    int obsah = n * n;

    printf("obvod: %d\n", obvod);
    printf("obsah: %d\n", obsah);

}

