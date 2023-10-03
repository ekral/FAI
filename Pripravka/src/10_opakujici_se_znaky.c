#include <stdio.h>
#include <stdbool.h>

#define MAX_LEN 256

int main()
{
    
    // I kdyz je gets_s soucasti standardu, tak ne vsechny prekladace jej podporuji
    // Je lepsi pouzit funkci fgets.
    // Nikdy nepouzivat funkci gets, ktera neni bezpecna, protoze nema omezeni poctu znaku.
    
    if(!fgets(pole, MAX_LEN, stdin))
    {
        puts("Selhani");
        return 0;
    }
   
    // vypiste pocet opakujicich dvojic stejnych po sobe nasledujicich znaku. 
    // Hodnota v poli s konrektnim indexem muze byt soucasti jen jedne dvojice, nemuze byt soucasti vice dvojic.
    // napriklad "Zoo bylo uuuplne nejlepsi."
    // vypise hodnotu 2, protoze jsou tam dve dvojice, oo a uu.
    // Text "uuuplne nejlepsi" obsahuje jednu dvojici.
    // Text "uuuuplne nejlepsi" obsahuje dve dvojice.
    
    int i = 0;
    int pocetDvojic = 0;
    char znak;
    
    while(znak = pole[i])
    {
        if(znak == pole[i + 1])
        {
            pocetDvojic++;
            i++;
            // Posuneme se na dalsi znak bez testovani na '\0'
            // Protoze dalsi znak je roven aktualnimu,
            // ktery jsme jiz otestovali ze neni '\0'
        }
        
        i++;
    }
    
    printf("Pocet dvojic: %d", pocetDvojic);
    
	return 0;
}
