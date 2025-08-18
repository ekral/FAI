#include <stdio.h>
#define _USE_MATH_DEFINES
#include <math.h>

int main()
{
	double x = pow(2.0, 8.0); // 2^8 = 256.0

	double D = 6100000;
	double urokProcenta = 4.0;
	double i = urokProcenta / 100 / 12.0;
	int n = 30 * 12;

	double v = 1 / (1 + i);
	double A = i * D / (1 - pow(v, n));

	printf("Mesicni splatka: %lf\n", A);
}
