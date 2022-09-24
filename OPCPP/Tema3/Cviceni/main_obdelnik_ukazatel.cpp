#include <stdio.h>

struct Obdelnik
{
	double a;
	double b;
};

// TODO pri volani metody se vzdy vytvori kopie celeho obdelniku
// pouzije z duvodu optimalizace ukazatel na obdelnik, tak aby se kopirovala jen adresa

double obdelnik_obsah(const Obdelnik* p)
{
	double obsah = p->a * p->b;
	return obsah;
}

// 1. Definujte funkci pro vypocet obvodu obdelnika a vypiste obvod ve vypisu
double obdelnik_obvod(const Obdelnik* p)
{
	double obvod = 2 * (p->a + p->b);
	return obvod;
}

void vypis(const Obdelnik* p)
{
	double obsah = obdelnik_obsah(p);
	double obvod = obdelnik_obvod(p);
	printf("a: %lf b: %lf obsah: %lf obvod: %lf\n", p->a, p->b, obsah, obvod);
}

int main()
{
	Obdelnik o1;
	o1.a = 3;
	o1.b = 4;

	// 1. Nadefinujte a zavolte funkci, 
	// ktera spocita obsah obdelnika 
	double obsah = obdelnik_obsah(&o1);
	double obvod = obdelnik_obvod(&o1);

	// 2.Nndefinujte a zavolteje funkci, 
	// ktera vypise rozmery obdelnika  a jeho obsah
	vypis(&o1);

}
