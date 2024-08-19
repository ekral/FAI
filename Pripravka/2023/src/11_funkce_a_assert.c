#include <stdio.h>
#include <stdbool.h>
#include <assert.h>

int spocitej_dvojice(char* retezec)
{
    int i = 0;
    int pocetDvojic = 0;
    char znak;
    
    while((znak = retezec[i]))
    {
        if(znak == retezec[i + 1])
        {
            pocetDvojic++;
            // Posuneme se na dalsi znak bez testovani na '\0' Protoze dalsi znak je roven aktualnimu,ktery jsme jiz otestovali ze neni '\0'
            i++;
        }
        
        i++;
    }
    
    return pocetDvojic;
}

int main()
{
    int pocet;
    
    // Neco podobneho by bylo v unit testu
    
    pocet = spocitej_dvojice("Zoo");
	assert(pocet == 1);
    
    pocet = spocitej_dvojice("\0");
	assert(pocet == 0);
    
    pocet = spocitej_dvojice("Zoo nee");
	assert(pocet == 2);
    
    pocet = spocitej_dvojice("Zooo");
	assert(pocet == 1);
    
    pocet = spocitej_dvojice("Zoo\0\0");
	assert(pocet == 1);
    
    return 0;
}
