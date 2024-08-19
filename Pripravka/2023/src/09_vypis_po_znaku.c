#include <stdio.h>
#include <string.h>
#include <stdbool.h>

int main()
{
    // Null-terminated byte string, ASCII znaky 
    char* retezec = "ahoj"; 
    
    char znak;
    int i = 0;
    
    while((znak = retezec[i]))
    {
        putchar(znak);
        ++i;
    }
    
	return 0;
}
