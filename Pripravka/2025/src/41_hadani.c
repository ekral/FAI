#include <stdio.h> 
#include <stdlib.h>
#include <time.h>
#include <stdbool.h>

int main()
{
	srand(time(NULL));
	int cislo = 1 + (rand() % 100);

	puts("Ja pocitac si myslim cislo, zkus ho uhodnout.");

	int konec = false;
	do
	{
		int tip = 0;
		printf("Zadej cislo: ");
		scanf_s("%d", &tip);

		konec = true; // az uhodne cislo

	} while (!konec);

	// ! je negace, z pravdy je nepravda a naopak
}
