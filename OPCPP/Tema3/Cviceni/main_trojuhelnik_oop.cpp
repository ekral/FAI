#include <math.h>
#include <stdio.h>

struct Trojuhelnik
{
	double a;
	double b;
	double c; // prepona

	double Obvod() const
	{
		return a + b + c;
	}
	
	double Obsah() const
	{
		// heronuv vzorec
		double s = (a + b + c) / 2;
		double S = sqrt(s * (s - a) * (s - b) * (s - c));

		return S;
	}

	bool JePravouhly() const
	{
		// Pozor na zaokrouhlovani, musime opatrne presat s odchylkou
		return (a * a) + (b * b) == (c * c);
	}
};

void Vypis(const struct Trojuhelnik* p)
{
	printf("a: %lf b: %lf c: %lf\n", p->a, p->b, p->c);
	printf("obvod: %lf obsah: %lf\n", p->Obvod(), p->Obsah());
	printf("pravouhly: %s\n", p->JePravouhly() ? "ano" : "ne");
}

int main()
{
	Trojuhelnik t1;
	t1.a = 3.0;
	t1.b = 4.0;
	t1.c = 5.0;

	Vypis(&t1);
}
