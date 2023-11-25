#include <assert.h>
#include <stdbool.h>

bool arrayEqual(const char* poleA, const char* poleB, char pocetZnaku)
{
    assert(pocetZnaku > 0);
    
    // testuji, zda zacinaji na stejne adrese
    if(poleA == poleB)
    {
        return true;
    }
    
    // testuji zda maji stejne hodnoty
    for(int i = 0; i < pocetZnaku; i++)
    {
        if(poleA[i] != poleB[i])
        {
            return false;
        }
    }
    
    return true;
}

void copyArray(char* original, char* kopie, char pocetZnaku)
{
    // 2. zkopiruje originalni pole do kopie
    
    for(int i = 0; i < pocetZnaku; i++)
    {
        kopie[i] = *(original + i);
    }
}

void vypis(char* const pole, char pocetZnaku)
{
    for(char* p = pole; p < (pole + pocetZnaku); p++)
    {
        char znak = *p;
        putchar(znak);
    }
    
    putchar('\n');
}

int main()
{
    const char * p = "ahoj";  // v readonly statickem bloku
   
    // pole se ukladaji na zasobnik, radove megabajty
    
    char poleA[3];              // prazdne neinicializovane pole, nevim jake hodnoty budou v poli
    char poleB[3] = { 0 };      // inicializovane na nuly, vsechny polozky budou 0
    char poleC[3] = { 1, 2, 3}; // inicializovane na konretni hodnoty
    char poleD[] = { 1, 2, 3};  // muzu vynechat pocet, kdyz inicializuji vsechny
    char poleE[5] = "ahoj";     // string initializations
    char poleF[] = "ahoj";      // string initializations
    
    vypis(poleE, 5);
    
    assert(arrayEqual(poleE, poleE, 5));
    assert(arrayEqual(poleE, poleF, 5));
    
    char kopie[5];
    
    copyArray(poleE, kopie, 5);
    
    vypis(poleE, 5);
    vypis(kopie, 5);
    
    assert(arrayEqual(kopie, "ahoj",5));
    assert(arrayEqual(poleE, kopie, 5));
    
    
    
    return 0;
}
