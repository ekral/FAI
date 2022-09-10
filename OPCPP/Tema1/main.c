#include <stdio.h>


int main()
{
	int vek = 27;
	double hmotnost = 50.5;
	double vyska = 1.87;

	// spocitejte a vypiste bmi
	double bmi = hmotnost / (vyska * vyska);
	printf("vek hodnota: %d vek adresa %p\n", vek, &vek);
	printf("bmi: %lf\n", bmi);

	// pokud bude bmi mensi nez 18.5, tak vypiste text
	// Reklama na pripravky na nabrani vahy

	// pokud bude vetsi nez 25, tak vypiste text
	// Reklama na pripravky na hubnuti
	
	if (bmi < 18.5)
	{
		printf("Reklama:\n");
		printf("Pripravky na nabrani vahy\n");
	}
	else if (bmi > 25)
	{
		printf("Reklama:\n");
		printf("Pripravky na hubnuti\n");
	}

	// pomoci cyklu while a pote pomoci for vypiste posloupnosti
	// 1 2 3 4 5 6 7 8 9
	// 9 8 7 6 5 4 3 2 1
	// 10 100 1000 10000 100000
	// 2 4 6 8 10 12 14 16
	// 256 128 64 32 16 8 4 2
	
	// DU zopakovat struktury a ukazatele

	return 1;
}
