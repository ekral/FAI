#include <stdio.h>

int main()
{
	// EOF zadame na Windows ctrl+z
	// na Unixu ctrl+d

	char znak;
	while ((znak = getchar()) != EOF)
	{
		putchar(znak);
	}
}
