#include <stdio.h> 
#include <math.h>

int main()
{
	int kapacita = 100;
	int kotel = 0;
	int prisada;

	puts("Magicky kotel");
	puts("=============");

	do
	{
		printf("Zadej prisadu do kotle: ");

		scanf_s("%d", &prisada);

		if (prisada == 999)
		{
			puts("nebezpecna prisada");
			break;
		}
		else if (prisada > 0)
		{
			puts("platna prisada");
			kotel += prisada; // pricte prisadu ke kotli
		}
		else if (prisada < 0)
		{
			puts("zkazena prisada");
			continue;
		}

		const int pocet_segmentu = 10;
		int plne = (int)round((pocet_segmentu * (double)kotel) / kapacita);
		int prazdne = pocet_segmentu - plne;

		printf("V kotli je: %d prazdne: %d plne: %d\n", kotel, prazdne, plne);

		putchar('\n');
		
		for (int i = 0; i < prazdne; i++)
		{
			puts("|            |");
		}
		
		for (int i = 0; i < plne; i++)
		{
			puts("|oooooooooooo|");
		}
	
		puts(" ------------ ");
		
		putchar('\n');

	} while (prisada != 0);

}
