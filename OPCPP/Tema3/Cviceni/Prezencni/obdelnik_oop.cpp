#include <stdio.h>

struct Obdelnik
{
	// clenske promenne (member variables)
	double a;
	double b;

	// clenske funkce (member functions)
	double Obvod()
	{
		double obvod = 2 * (a + b);
		return obvod;
	}

	double Obsah()
	{
		double obsah = a * b;
		return obsah;
	}
};

// napiste funkci, ktera vrati obsah
void Vypis(Obdelnik* obdelnik)
{
	printf("a: %lf b: %lf\n", obdelnik->a, obdelnik->b);
	printf("obvod: %lf obsah: %lf", obdelnik->Obvod(), obdelnik->Obsah());
}

int main()
{
	Obdelnik o1;
	o1.a = 2.0;
	o1.b = 3.0;
	Vypis(&o1);
	double obvod = o1.Obvod();
	double obsah = o1.Obsah();
}
