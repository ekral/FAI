#include <stdio.h>

struct Obdelnik
{
	double a;
	double b;
};

double obdelnik_obsah(Obdelnik o)
{
	double obsah = o.a * o.b;
	return obsah;
}

void vypis(Obdelnik o)
{
	double obsah = obdelnik_obsah(o);
	printf("a: %lf b: %lf obsah:%lf alarm: \n", o.a, o.b, obsah);
}

int main()
{
	Obdelnik o1;
	o1.a = 3;
	o1.b = 4;

	// 1. Nadefinujte a zavolte funkci, 
	// ktera spocita obsah obdelnika 
	double obsah = obdelnik_obsah(o1);

	// 2.Nndefinujte a zavolteje funkci, 
	// ktera vypise rozmery obdelnika  a jeho obsah
	vypis(o1);

}
