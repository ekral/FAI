#include <stdio.h>

int main()
{
	char pole[32];

	do
	{
		gets_s(pole, 32);
		puts(pole);

	} while (pole[0] != 'x');
	
	puts("konec");
}
