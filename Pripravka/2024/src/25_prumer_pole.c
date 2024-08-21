#include <stdio.h>

int main()
{
	int pole[5] = { 2,8,9,6,3 };
	int n = 5;

	for (int i = 0; i < n; i++)
	{
		int prvek = pole[i];

		printf("%d\n", prvek);
	}

	int suma = 0;
	// spocitejte a vypiste sumu prvku v poli

	for (int i = 0; i < n; i++)
	{
		int prvek = pole[i];

		suma += prvek; // suma = suma + prvek
	}

	puts("--");
	printf("%d\n", suma);

	// spocitejte a vypiste prumer prvku v poli
	double prumer = suma / (double)n;
	printf("prumer: %lf\n", prumer);

	// spocitejte a vypiste kolik prvku v poli
	// je vetsi nez prumer
}
