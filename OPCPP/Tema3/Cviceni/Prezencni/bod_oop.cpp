#include <stdio.h>
#include <math.h>
// Nadefinujte strukturu Bod
// s clenskymi promennymi x a y
// a clenskou funkci Vzdalenost
struct Bod
{
	double x;
	double y;

	double Vzdalenost(Bod* other)
	{
		double dx = x - other->x;
		double dy = y - other->y;

		double vzdalenost = sqrt(dx * dx + dy * dy);

		return vzdalenost;
	}
};

void Vypis(Bod* bod)
{
	printf("x: %lf y: %lf\n", bod->x, bod->y);
}

int main()
{
	Bod b1;
	b1.x = 2.0;
	b1.y = 3.0;

	Vypis(&b1);

	Bod b2;
	b2.x = 8.0;
	b2.y = 9.0;

	Vypis(&b2);

	double vzdalenost = b1.Vzdalenost(&b2);
	printf("vzdalenost: %lf\n", vzdalenost);
}
