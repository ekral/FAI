#include <stdio.h>
#include <stdbool.h>
#include <string.h>
#include <math.h>

int main()
{
    printf("2^16 = %lf\n", pow(2.0, 16.0));
    printf("2^15 = %lf\n", exp2(15.0));
    
    // "ab" zacina na adrese 3
    // retezec ma adresu 10
    
    char* retezec = "ab"; 
    // unsinged int ma minimalne 2 bajty
    size_t delka = strlen(retezec);
    
    printf("delka rezezce: %zu\n", delka);
    
    puts(retezec);
	return 0;
}
