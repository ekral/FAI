#include <stdio.h>
#include <string.h>
#include <stdbool.h>

int main()
{
    // Null-terminated byte string, ASCII znaky 
    char* retezec = "ahoj"; 
    
    // size_t delka = strlen(retezec);
    
    char prvniZnak = retezec[0];
    char druhyZnak = retezec[1];
    
    putchar(prvniZnak);
    putchar(druhyZnak);
    
    // delka retezce
    int i = 0;
    while(retezec[i])
    {
        ++i;
    }
        
    printf("Delka retezce: %d", i);
    
	return 0;
}
