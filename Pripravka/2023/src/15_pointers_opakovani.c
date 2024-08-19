#include <assert.h>

// 1. Opravte bug, kdyz pocetPrvku == 0, tak stejne vypise prvni prvek
void vypis(int* p, int pocetPrvku)
{
    if(pocetPrvku == 0)
    {
            return;
    }

    int i;
    for(i = 0; i < pocetPrvku - 1; i++)
    {
        printf("%d, ", p[i]);
    }
    
    printf("%d", p[i]);
    putchar('\n');
}

int spocitej_sumu(int* p, int pocetPrvku)
{
    // 2. Prepiste s pouzitim inkrementace ukazatele
    // ++p; // posune ukazatel na dalsi objekt typu int
    int suma = 0;
    
    for(int i = 0; i < pocetPrvku; i++)
    {
        suma += *p;// suma += *(p+i)
        p++;
    }
    
    return suma;
}

int main()
{
    int pole[] = { 1, 2, 3, 4, 5 };
    int* p = &pole[0];
    
    vypis(&pole[0], 5);
    
    int suma = spocitej_sumu(&pole[0], 5); 
    
    assert(suma == 12);
    
    *p = 0b11111111111111111111111111111111;
    ++p;
    *p = 0xFFFFFFFF;
    ++p;
    *p = 7;
    ++p;
    *p = 7;
    
	return 0;
}
