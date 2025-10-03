#include <stdio.h>
#define _USE_MATH_DEFINES
#include <math.h>

class Hypoteka
{
public:
	double p;
	double urokProcenta;
	int pocetLet;

	double Splatka()
	{
		double r = urokProcenta / 12.0 / 100.0;
		int n = pocetLet * 12;
		// doplnit vypocet
		double m = (p * r * pow(1 + r, n)) / (pow(1 + r, n) - 1);
		return m;
	}
};

void Vypis(Hypoteka* hypoteka)
{
	printf("pujcka: %.2lf urok: %.2lf pocet let: %d splatka: %.2lf\n", hypoteka->p, hypoteka->urokProcenta, hypoteka->pocetLet, hypoteka->Splatka());
}

void Zadej(Hypoteka* hypoteka)
{
	puts("Zadej vysi pujcky:");
	scanf_s("%lf", &hypoteka->p);

	puts("Zadej urok:");
	scanf_s("%lf", &hypoteka->urokProcenta);

	puts("Zadej pocet let:");
	scanf_s("%d", &hypoteka->pocetLet);
}

int main()
{
	Hypoteka h1;
	Hypoteka h2;

	puts("Hypoteka A");
	Zadej(&h1);
	Vypis(&h1);

	puts("Hypoteka B");
	Zadej(&h2);
	Vypis(&h2);
}
