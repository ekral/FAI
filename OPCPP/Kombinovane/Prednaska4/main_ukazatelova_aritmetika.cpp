#include <stdio.h>

int main()
{
	int* p1 = 0;

	printf("p2 pred: %p\n", p1);	// 0
	int* p2 = p1 + 3;
	printf("p2 po: %p sizeof(int): %llu\n", p2, sizeof(int));		// 4

	long long rozdil = p2 - p1;
	printf("p2 - p1 = %lld\n", rozdil);

	int n = 3;
	int* p = new int[n];

	for (int i = 0; i < n; i++)
	{
		*(p + i) = 0;
		p[i] = 0;
	}

	int* pi = p;
	int* k = p + n;

	while(pi < k)
	{
		*pi = 1;
		++pi;
	}

	delete[] p;
}
