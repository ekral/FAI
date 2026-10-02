#include <stdio.h>

int main()
{
	int znak;

	do
	{
		znak = getchar();
		putchar(znak);

	} while (znak != 'x');
	
	puts("konec");
}
