#include <stdio.h>
#include <math.h>
int main()
{
	// (x + 2)(x + 2) = x^2 + 4x + 4
	// (x + 2)(x + 3) = x^2 + 5x + 6

	double a = 1.0;
	double b = 5.0;
	double c = 6.0;

	// spocitejte a vypiste diskriminant

	double D = (b * b) - (4 * a * c);

	printf("D: %lf\n", D);

	// Pridejte do solution novy projekt a
	// nastavte ho jako Startup Project.

	if (D > 0)
	{
		puts("Dva realne koreny");

		// spocitejte a hodnoty realnych korenu x1 a x2
		double x1 = (-b + sqrt(D)) / (2 * a);
		double x2 = (-b - sqrt(D)) / (2 * a);

		printf("x1: %lf\n", x1);
		printf("x2: %lf\n", x2);
	} 
	else if (D == 0)
	{
		double x = (-b) / (2 * a);
		printf("x: %lf\n", x);
	}
	else
	{
		puts("Reseni v oboru komplexnich cisel");
	}
}
