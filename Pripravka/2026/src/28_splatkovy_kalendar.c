#include <stdio.h>
#include <math.h>
int main()
{
	// Nactete rocni urok v procentech
	// Nactete pocet let splaceni
	// a vysi pujcky

	double rocniUrokProcenta = 5.0;
	int pocetLet = 30;
	double D = 6000000;

	double i = rocniUrokProcenta / 100 / 12;
	int n = pocetLet * 12;

	double v = 1 / (1 + i);
	double splatka = (i * D) / (1 - pow(v, n));

	printf("splatka: %lf\n", splatka);

	printf("splatka\t\turok\t\tumor\t\tdluh\n");

	double dluh = D;

	for (int j = 0; j < n; j++)
	{
		double urok = dluh * i;
		double umor = splatka - urok;
		dluh -= umor;

		double splacenoProcent = 100 * (dluh / D);

		printf("%8.2lf\t%8.2lf\t%8.2lf\t%8.2lf\t%8.2lf\t", splatka, urok, umor, dluh, splacenoProcent);
	
		for (int k = 0; k < splacenoProcent; k++)
		{
			putchar('*');
		}
		
		putchar('\n');
	}
}
