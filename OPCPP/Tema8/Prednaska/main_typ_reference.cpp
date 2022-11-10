#include <stdio.h>
#include <vector>

struct Bod
{
	int x;
	int y;

	Bod(int x, int y) : x(x), y(y)
	{
	}

	Bod Odecti(Bod* other)
	{
		return Bod(x - other->x, y - other->y);
	}

	Bod operator - (const Bod& other)
	{
		return Bod(x - other.x, y - other.y);
	}
};

// typ reference

void Vypis(const Bod bod)
{
	printf("x: %d y: %d\n", bod.x, bod.y);
}

int main()
{
	int x = 0;

	int* p = nullptr; // typ ukazatel
	p = &x; // operator reference znamy taky adresni operator
	*p = 1; // operator dereference znamy taky jako operator indirekce

	int& ref = x;// typ reference
	ref = 2;

	Bod bod1(2, 3);
	Bod bod2(10, 20);

	Bod p3 = bod2.Odecti(&bod1);
	Bod p3 = bod2 - bod1;

	Vypis(bod1);
}

