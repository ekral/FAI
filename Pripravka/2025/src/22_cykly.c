#include <stdio.h>

int main()
{
	// Kdyz zname pocet opakovani tak pouzijeme cyklus for
	// Kdyz nezname pocet opakovani a telo se nemusi provest ani jednou, tak while
	// Kdyz nezname pocet opakovani a telo se musi provest aspon jednou, tak do-while

	// s pomoci do-while, while a for vypiste na terminal cisla
	// 0
	// 1
	// 2
	// 3
	// 4
	// 5

	int i = 0;
	
	do
	{
		printf("%d\n", i);
		++i;

	} while (i < 6);

	int j = 0;
	while (j < 6)
	{
		printf("%d\n", j);
		++j;
	}

	for (int i = 0; i < 6; i++)
	{
		printf("%d\n", i);
	}
	
}
