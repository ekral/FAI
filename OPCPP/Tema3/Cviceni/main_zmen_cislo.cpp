#include <stdio.h>

// int* typ ukazatel
void zmen(int* p)
{
	*p = 0; // operator indirekce, taky znamy jako operator derefenece
}

int main()
{
	int x = 1;
	zmen(&x); // adresni operator
	printf("x: %d\n", x);

}
