#include <stdio.h>
#include<math.h>
class Hypoteka
{
public:
	double p;
	double urokProcenta;
	int pocetLet;

	double Splatka()
	{
		double r = urokProcenta / 100 / 12;
		int n = pocetLet * 12;
		return (p * r * pow(1 + r, n)) / (pow(1 + r, n) - 1);
	}
};

void ZadejHodnoty(Hypoteka* hypoteka)
{
	int ret;
	
	puts("Zadej vysi pujcky");
	ret = scanf_s("%lf", &hypoteka->p);

	puts("Zadej urok");
	ret = scanf_s("%lf", &hypoteka->urokProcenta);

	puts("Zadej pocet let");
	ret = scanf_s("%d", &hypoteka->pocetLet);
}

void Vypis(Hypoteka* hypoteka)
{
	printf("pujcka: %.2lf urok: %.2lf pocet let: %d\n", hypoteka->p, hypoteka->urokProcenta, hypoteka->pocetLet);
}

int main()
{
	Hypoteka h1;
	Hypoteka h2;

	ZadejHodnoty(&h1);
	ZadejHodnoty(&h2);
	
	Vypis(&h1);
	printf("Vyse splatky 1: %.2lf\n", h1.Splatka());
	Vypis(&h2);
	printf("Vyse splatky 2: %.2lf\n", h2.Splatka());
}


