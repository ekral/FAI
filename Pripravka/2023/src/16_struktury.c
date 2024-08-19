#include <stdlib.h>
#include <assert.h>
#include <conio.h>
#include <ctype.h>
#include <stdbool.h>

struct Matice
{
    char* data;
    int pocetRadku;
    int pocetSloupcu;
};

void matice_init(struct Matice* matice, int max_x, int max_y, char znak)
{    
    int pocetPrvku = max_x * max_y;
    // pozadam OS o alokaci pameti pro pocetPrvku * sizeof(char) bajtu
    matice->data = malloc(pocetPrvku * sizeof(char)); 
    // matice-> je kratsi zapis pro (*matice). 
    matice->pocetRadku = max_y; 
    (*matice).pocetSloupcu = max_x;
    
    for(int i = 0; i < pocetPrvku; i++)
    {
        matice->data[i] = znak;
    }
}

void matice_vypis(struct Matice* matice)
{
    int pos = 0;
    for(int i = 0; i < matice->pocetRadku; i++)
    {
        for(int j = 0; j < matice->pocetSloupcu; j++)
        {
            putchar(matice->data[pos]);
            ++pos;
        }
        
        putchar('\n');
    }
}

void matice_nakresli_bod(struct Matice* matice, int x, int y, char znak)
{
    int pos = x + (y * matice->pocetSloupcu);
    matice->data[0] = znak;
}

void matice_free(struct Matice* matice)
{
    free(matice->data);
    matice->data = NULL;
}

int main()
{
    int max_x = 50;
    int max_y = 10;
    
    struct Matice matice;
    matice_init(&matice, max_x, max_y, 'x');
    
    int x = 7;
    int y = 5;
    
    bool konec = false;
    
    int znak = _getch(); // Jen pro windows
    
    if(znak == 'a')
    {
        ++x;
    }
    
    matice_nakresli_bod(&matice, x, y, 'O');

    matice_vypis(&matice);
    
    matice_free(&matice);
    
	return 0;
}
