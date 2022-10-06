#include <stdio.h>
#include <vector>

class Obdelnik
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

void Vypis(const Obdelnik* o)
{
	printf("a: %lf b: %lf\n", o->a, o->b);
}

struct Komplexni
{
	double re;
	double im;
};

int main()
{
	Komplexni c1;
	c1.im = 1.0;
	c1.re = 2.0;

	Obdelnik o1;
	o1.a = 2.0;
	o1.b = 3.0;

	double obvod = o1.Obvod();
	double obsah = o1.Obsah();

	Vypis(&o1);
	c1.im = o1.a;

	std::vector<int> cisla;
	cisla.push_back(1);
	cisla.push_back(2);
	cisla.push_back(3);
	cisla.push_back(4);
	int x = cisla[0];
	cisla[0] = 5;

	int pocet = cisla.size();
	cisla.insert(cisla.begin() + 2, 7);

	for (int cislo : cisla)
	{
		printf("%d ", cislo);
	}
	putchar('\n');

	printf("obvod: %lf\n", obvod);
}
