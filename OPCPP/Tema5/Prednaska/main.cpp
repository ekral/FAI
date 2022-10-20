#include <stdio.h>

void Vypis(int* p, int n)
{
	for (int i = 0; i < n; i++)
	{
		printf("%d ", p[i]);
	}

	putchar('\n');
}

struct Platno
{
	int* p; 

	Platno(int n)
	{
		p = new int[n];
	}

	~Platno()
	{
		delete[] p;
	}
};

int main()
{
	
	Platno platno(3);

	int x = 2;
	int y = 3;

	int* p = nullptr;
		
	p =	new int[3];

	p[0] = 1;
	p[1] = 2;
	p[2] = 3;

	int* kopie = p;

	p[0] = 7;
	p[1] = 7;
	p[2] = 7;

	Vypis(p, 3);		// co vypise  
	Vypis(kopie, 3);	// co vypise

	delete[] p;
}
