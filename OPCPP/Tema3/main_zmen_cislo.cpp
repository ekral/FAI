#include <stdio.h>

void zmen(int* p)
{
	p = 0;
}

int main()
{
	int x = 1;
	zmen(&x);
	printf("x: %d\n", x);

}
