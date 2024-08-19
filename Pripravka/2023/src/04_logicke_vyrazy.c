#include <stdio.h>

int main()
{
	double a = 5.0;
	double b = 6.0;
	double c = 7.0; // prepona

	// soucet dvou stran bude vetsi nez treti strana

	// napiste na terminal, zda trojuhelnik existuje
	if (a + b > c && a + c > b && b + c > a)
	{
		puts("Trojuhelnik existuje");
		
		double obsah = 0.0;

		// pozor realne testovat s chybou
		if (a * a + b * b == c * c)
		{
			obsah = (a * b) / 2;
		}
		else
		{
			// https://cs.wikipedia.org/wiki/Heron%C5%AFv_vzorec
		}
	}
	else
	{
		puts("Trojuhelnik neexistuje");
	}

	// pokud existuje, tak overte, zda je pravouhly
	// pokud je pravouhly, tak spocijte obsah jednoduse
	// jinak obsah podle heronova vzorce

	return 0;
}
