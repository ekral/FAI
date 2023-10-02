#include <stdio.h>
#include <assert.h>

// doplnte parametricky konstruktor
// vyuzijte member initializer list
struct Bod2d
{
	double x;
	double y;

	Bod2d(double x, double y) : x(x), y(y)
	{

	}
};

// main nemente
int main()
{
	Bod2d bod1(2.0, 3.0);

	assert(bod1.x == 2.0);
	assert(bod1.y == 3.0);
}
