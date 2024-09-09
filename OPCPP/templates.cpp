#include <string>
#include <stdio.h>
#include <vector>

using namespace std;

// Parent class
// Base class
// Superclass

class Zviratko
{
public:
	string jmeno;

	int vek;

	Zviratko(string jmeno, int vek) : jmeno(jmeno), vek(vek)
	{

	}

	virtual void Zvuk() = 0;
};

// Child class
// Inherited class
// Subclass
class Pejsek : public Zviratko
{
public:
	bool bouda;

	Pejsek(string jmeno, int vek, bool bouda) : Zviratko(jmeno, vek), bouda(bouda)
	{

	}

	void Zvuk() override
	{
		printf("Haf haf\n");
	}
};

class Kocicka : public Zviratko
{
public:
	int pocetZivotu;

	Kocicka(string jmeno, int vek) : Zviratko(jmeno, vek), pocetZivotu(7)
	{

	}

	void Zvuk()
	{
		printf("Mnau\n");
	}
};

class Auticko
{
	string vyrobce;
	double kapacitaBaterie;
};

// typ reference pro predavani parametru
// jednodussi jako ukazatel
void Vypis(const Zviratko& zviratko)
{
	printf("jmeno: %s \n", zviratko.jmeno.c_str());
}

template<class T>
class MujZasobnik
{
private:
	int index = 0;
	T data[5];

public:

	void Vloz(T prvek)
	{
		data[index] = prvek;
		++index;
	}

	T Vyloz()
	{
		--index;

		T prvek = data[index];

		return prvek;
	}

	int PocetPrvku()
	{
		return index;
	}
};

int main()
{
	MujZasobnik<int> zasobnikCisel;

	zasobnikCisel.Vloz(1);
	zasobnikCisel.Vloz(2);
	zasobnikCisel.Vloz(3);

	printf("Pocet prvku: %d \n", zasobnikCisel.PocetPrvku());
	printf("prvek: %d\n", zasobnikCisel.Vyloz());
	printf("prvek: %d\n", zasobnikCisel.Vyloz());
	printf("prvek: %d\n", zasobnikCisel.Vyloz());

	Pejsek pejsek("Rex", 4, false);
	Kocicka kocicka("Zofka", 3);

	Vypis(pejsek);
	Vypis(kocicka);

	MujZasobnik<Zviratko*> zasobnikZviratek;
	zasobnikZviratek.Vloz(&pejsek);
	zasobnikZviratek.Vloz(&kocicka);

	Zviratko* zviratko = nullptr;

	zviratko = zasobnikZviratek.Vyloz();
	zviratko->Zvuk();
	
	zviratko = zasobnikZviratek.Vyloz();
	zviratko->Zvuk();
}
