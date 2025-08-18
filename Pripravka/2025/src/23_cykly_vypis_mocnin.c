#include <stdio.h>

int main()
{
	// s pomoci do-while, while a for vypiste na terminal cisla
	// 1
	// 2
	// 4
	// 8
	// 16
	// 32
	// 64
	// 128

	int x = 1;
	do
	{
		printf("%d\n", x);
		x *= 2;
	} while (x <= 128);

	int y = 1;
	while (y <= 128)
	{
		printf("%d\n", y);
		y *= 2;
	}

	for (int i = 1; i <= 128; i *= 2)
	{
		printf("%d\n", i);
	}

	int z = 1;
	for (int i = 0; i < 8; i++)
	{
		printf("%d\n", z);
		z *= 2;
	}
}
