#include <stdio.h>
#include <math.h>

struct Zavodnik
{
	char* jmeno;
	double pokus1;
	double pokus2;
	double pokus3;
};

void vypis(struct Zavodnik z)
{
	printf("%s\n", z.jmeno);
	printf("1. pokus: %lf\n", z.pokus1);
	printf("2. pokus: %lf\n", z.pokus2);
	printf("3. pokus: %lf\n", z.pokus3);
	putchar('\n');
}

double NejdelsiSkok(struct Zavodnik z)
{
	double nejdelsi = fmax(z.pokus1, fmax(z.pokus2, z.pokus3));
	return nejdelsi;
}

int main()
{
	struct Zavodnik z1 = { "Karl", 783.0, 810.0, 750.0 };
	struct Zavodnik z2 = { "John", 805.0, 830.0, 850.0 };
	struct Zavodnik z3 = { "Eric", 783.0, 710.0, 750.0 };
	struct Zavodnik z4 = { "Peter", 0.0, 0.0, 0.0 };

	struct Zavodnik zavodnici[] = { z4 };
	int n = sizeof(zavodnici) / sizeof(struct Zavodnik);

	// 1. vypiste seznam vsech zavodniku v poli
	for (int i = 0; i < n; i++)
	{
		struct Zavodnik zavodnik = zavodnici[i];

		vypis(zavodnik);
	}

	// 2. Vypiste jmeno a pokusy viteze s nejdelsim skokem
	int indexNejlepsiho = 0;
	double nejdelsiCelkem = NejdelsiSkok(zavodnici[0]);

	for (int i = 1; i < n; i++)
	{
		struct Zavodnik z = zavodnici[i];
		
		double nejdelsi = NejdelsiSkok(z);

		if (nejdelsi > nejdelsiCelkem)
		{
			nejdelsiCelkem = nejdelsi;
			indexNejlepsiho = i;
		}
	}

	puts("Nejlepsi zavodnik:");
	vypis(zavodnici[indexNejlepsiho]);
}
