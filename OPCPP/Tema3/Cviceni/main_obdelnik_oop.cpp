#include <stdio.h>
#include <stdexcept>

struct Obdelnik
{
public:
	// clenske promenne
	double a;
	double b;

	// clenske funkce
	double obsah() const
	{
		double obsah = a * b;
		return obsah;
	}

	double obvod() const
	{
		double obvod = 2 * (a + b);
		return obvod;
	}

	void zmen_rozmery(double a, double b)
	{
		if (a <= 0) throw std::invalid_argument("a musi byt kladne cislo");
		if (b <= 0) throw std::invalid_argument("b musi byt kladne cislo");

		this->a = a;
		this->b = b;
	}
};

// vypis neni clenska funkce, protoze
// krome clenskych promennych pristupuje i funkci printf
// a nechci aby byla struktuka Obdelnik zavisla na stdio.h

void vypis(const Obdelnik* p)
{
	double obsah = p->obsah();
	double obvod = p->obvod();
	printf("a: %lf b: %lf obsah: %lf obvod: %lf\n", p->a, p->b, obsah, obvod);
}

int main()
{
	Obdelnik o1;
	o1.a = 3;
	o1.b = 4;

	o1.zmen_rozmery(0.1, 0.0);

	double obsah = o1.obsah();
	double obvod = o1.obvod();

	vypis(&o1);

}
