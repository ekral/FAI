#include <stdio.h>

int main()
{
	// (x + 2)(x + 3) = x^2 + 5x + 6

	double a = 1.0;
	double b = 5.0;
	double c = 6.0;

	// spocitejte a vypiste diskriminant

	double D = (b * b) - (4 * a * c);

	printf("D: %lf\n", D);
}
