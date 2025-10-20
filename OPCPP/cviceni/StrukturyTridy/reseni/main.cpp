#include <stdio.h>
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

		double splatka = p * r * pow(1 + r, n) / (pow(1 + r, n) - 1);

		return splatka;
	}
};

void Zadej(Hypoteka* hypoteka)
{
	printf("Zadej pujcku: ");
	scanf_s("%lf", &hypoteka->p);
	printf("Zadej urok v procentech: ");
	scanf_s("%lf", &hypoteka->urokProcenta);
	printf("Zadej pocet let: ");
	scanf_s("%d", &hypoteka->pocetLet);
}

void Vypis(Hypoteka* hypoteka)
{
	printf("pujcka: %.0f urok: %.2f pocet let: %d splatka: %.2f\n", hypoteka->p, hypoteka->urokProcenta, hypoteka->pocetLet, hypoteka->Splatka());	
}

int main()
{
	Hypoteka h1;
	
	puts("Prvni varianta hypoteky");
	Zadej(&h1);
	Vypis(&h1);

	Hypoteka h2;

	puts("Druha varianta hypoteky");
	Zadej(&h2);
	Vypis(&h2);
}