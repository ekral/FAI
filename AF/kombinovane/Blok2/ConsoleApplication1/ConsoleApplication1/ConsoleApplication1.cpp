#include <stdio.h>
#include <vector>

int main()
{
    int n = 0;
    scanf_s("%d", &n);

    if (!(n > 2)) return 0;

    //int pole[5];

    int* pole = new int[n];
    std::vector<int> vektor(n, 0);
    
    for (int i = 0; i < n; i++)
    {
        vektor[i] = 0;
    }
 
    for (int& prvek : vektor)
    {
        prvek = 0;
    }

    pole[0] = 0;
    pole[1] = 1;
    pole[2] = 2;

    *pole = 0;
    *(pole + 1) = 0;
    *(pole + 2) = 0;
    
    int *p = pole;
    *p = 0;
    ++p;
    *p = 1;
    ++p;
    *p = 2;

    size_t rozdilUkazatelu = p - pole;

    int pCislo = (int)p;
    int poleCislo = (int)pole;

    int rozdilCisel = pCislo - poleCislo;

    printf("rozdil ukazatelu: %zu\n", rozdilUkazatelu);
    printf("rozdil cisel: %d\n", rozdilCisel);

    delete[] pole;

    return 0;
}

