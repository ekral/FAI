#include <stdio.h>

int main()
{
	char znak;

	do
	{
		znak = getchar();
		putchar(znak);

	} while (znak != 'x');
	
	puts("konec");
}
