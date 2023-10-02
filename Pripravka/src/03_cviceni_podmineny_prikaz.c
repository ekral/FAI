#include <stdio.h>

int main()
{
	// najit implicitni prevod int na double

	int hmotnostKg = 75;
	int vyskaCm = 0;
	
	printf("Zadej svou hmotnost v kg: ");
	scanf_s("%d", &hmotnostKg);
	printf("Zadana hmotnost je %d\n", hmotnostKg);

	printf("Zadej svou vysku v cm: ");
	scanf_s("%d", &vyskaCm);
	printf("Zadana vyska je %d\n", vyskaCm);

	// bmi 
	double vyskaMetry = vyskaCm / 100.0;
	double bmi = hmotnostKg / (vyskaMetry * vyskaMetry);

	printf("Vase bmi je %lf\n", bmi);

	// napiste zda si zda zakaznikovi zobrazit nabidku na produkt na hubnuti
	
	// vypiste vsechy kategorie bez pouziti and a or

	if (bmi > 30.0)
	{
		printf("Specialni nabidka hubnouciho balicku pro vas.\n");
	}
	else if (bmi <= 18.5)
	{
		printf("Specialni nabidka balicku na pribrani pro vas.\n");
	}
	else
	{
		printf("tycinky pro udrzeni zdrave vahy.\n");
	}

	if (bmi <= 16.5)
		printf("tezka podvyziva\n");
	
	char* text =
		bmi <= 16.0 ? "tezka podvyziva" :
		bmi <= 18.5 ? "podvaha" :
		bmi <= 25.0 ? "idealni vaha" :
		bmi <= 30.0 ? "nadvaha" :
		bmi <= 35.0 ? "obezita prvniho stupne" :
		bmi <= 40 ?	  "obezita druheho stupne" :
					  "morbidni obezita";

	puts(text);

	// vypsat jen idealni vahu
	// && a zaroven
	if (bmi > 18.5 && bmi <= 25.0)
	{
		printf("idealni vaha");
	}
	
	// prepsat podminku jen pomoci dvou ifu
	if (bmi > 18.5 )
	{
		if (bmi <= 25.5)
		{
			printf("idealni vaha");
		}
	}

	// || nebo, symbol pravy alt + w
	if (bmi <= 16.5 || bmi > 30.0)
	{
		printf("neni idealni vaha");
	}
	else
	{
		printf("idealni vaha");
	}

	// pomoci ifu a else
	// asi budeme muset pouzit printf dvakrat
	if (bmi > 16.5)
	{
		printf("neni idealni vaha");
	}
	else if (bmi > 30 )
	{
		printf("neni idealni vaha");
	}

	return 0;
}
