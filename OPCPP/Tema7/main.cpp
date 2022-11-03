#include <stdio.h>
#include <vector>

struct Struktura
{
	int n;
	int* pole;

	Struktura(Struktura&) = delete;

	Struktura(int n) : n(n)
	{
		pole = new int[n];
		printf("alokoval jsem pole\n");
	}

	~Struktura()
	{
		delete[] pole;
		printf("uvolnil jsem pole\n");
	}
};

struct Ctverec
{
	int n;
	Ctverec(int n) : n(n)
	{

	}

	int Obsah()
	{
		return n * n;
	}
};

Ctverec* VytvorCtverec()
{
	Ctverec* pc2 = new Ctverec(5);
	return pc2;
}

int main()
{
	Struktura s1(3);
	//Struktura s2 = s1;


	if (true)
	{
		Struktura s1(3);
		printf("pracuji se strukturou\n");
	}
	int pole[3];  // zasobnik

	// halda

	int n = 3;
	int* p3 = new int[n];
	delete[] p3;

	int* p1 = new int;
	delete p1;

	// alokujte pole trech integeru na halde
	int* p4 = new int[3];
	// uvolnete pole trech integeru na halde
	delete[] p4;

	std::vector<int> v1;
	v1.push_back(1);
	v1.push_back(2);
	v1.push_back(3);

	v1[1] = 7;

	for (int i = 0; i < v1.size(); i++)
	{
		printf("%d ", v1[i]);
	}

	for (int prvek : v1)
	{
		printf("%d ", prvek);

	}
}
