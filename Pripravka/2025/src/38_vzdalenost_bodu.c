#include <stdio.h>
#include <math.h>
struct Bod
{
	double x;
	double y;
};

void vypis(struct Bod bod)
{
	printf("x: %lf y: %lf\n", bod.x, bod.y);
}

int main()
{
	//struct Bod a = { .x = 0.0, .y = 1.0 };
	struct Bod a = { 0.0, 1.0 };
	struct Bod b = { 4.0, 4.0 };
	vypis(a);
	vypis(b);

	// spocitejte a vypiste vzdalenost dvou bodu
	double dx = b.x - a.x;
	double dy = b.y - a.y;

	double d = sqrt(dx * dx + dy * dy);

	printf("vzdalenost: %lf", d);
}
