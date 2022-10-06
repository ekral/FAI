#include <stdio.h>
#include <math.h>

struct Trojuhelnik
{
	double a;
	double b;
	double c;

	double Obvod()
	{
		double obvod = a + b + c;
		return obvod;
	}

	double Obsah()
	{
		double s = (a + b + c) / 2;
		double obsah = sqrt(s * (s - a) * (s - b) * (s - c));

		return obsah;
	}

	bool JePravouhly()
	{
		bool pravouhly = c * c == a * a + b * b; // TODO pridat eps

		return pravouhly;
	}

	bool Existuje()
	{
		bool existuje = (a + b > c) && (a + c > b) && (a + c > b);
		return existuje;
	}
};

char* BoolToText(bool value)
{
	return value ? "ano" : "ne";
}

void Vypis(Trojuhelnik* t)
{
	printf("a: %lf b: %lf b: %lf\n", t->a, t->b, t->c);
	printf("obvod: %lf obsah: %lf\n", t->Obvod(), t->Obsah());
	printf("existuje: %s pravouhly: %s\n", BoolToText(t->Existuje()), BoolToText(t->JePravouhly()));
}

int main()
{
	Trojuhelnik t1;
	t1.a = 3.0;
	t1.b = 4.0;
	t1.c = 5.0;

	double obvod = t1.Obvod();
	double obsah = t1.Obsah();
	bool pravouhly = t1.JePravouhly();
	bool existuje = t1.Existuje();

	Vypis(&t1);

	getchar();
}
