#include <stdio.h> 
#include <stdlib.h>
#include <time.h>
#include <stdbool.h>

int main()
{
	srand(time(NULL));
	int cislo = 1 + (rand() % 100);

	puts("Ja pocitac si myslim cislo, zkus ho uhodnout.");

	char pokracovat = 'n';

	do
	{
		int konec = false;
		do
		{
			int tip = 0;
			printf("Zadej cislo: ");
			scanf_s("%d", &tip);

			if (tip == cislo)
			{
				puts("Gratuluji, uhodnul jsi cislo :)");
				konec = true; // az uhodne cislo
			}
			else if (tip < cislo)
			{
				puts("cislo je vetsi");
			}
			else
			{
				puts("cislo je mensi");
			}

		} while (!konec);

		puts("Pokracovat? a/n");

		// TODO nacteni retezce do pole

	} while (pokracovat == 'a');

	// ! je negace, z pravdy je nepravda a naopak
}
