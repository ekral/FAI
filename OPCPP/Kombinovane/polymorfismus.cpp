#include <string>
#include <stdio.h>
#include <vector>

using namespace std;

// Parent class
// Base class
// Superclass

class Zviratko
{
private:
	string jmeno;
public:
	int vek;

	Zviratko(string jmeno, int vek) : jmeno(jmeno), vek(vek)
	{

	}

	virtual void Zvuk()
	{
		printf("Nevime jaky zvuk dela abstraktni zviratko.\n");
	}
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

int main()
{
	Pejsek pejsek("Rex", 4, false);
	Kocicka kocicka("Zofka", 3);

	// upcasting
	//Zviratko* zviratko = &pejsek;
	//zviratko->Zvuk();

	std::vector<Zviratko*> farma;
	
	farma.push_back(&pejsek);
	farma.push_back(&kocicka);

	for (Zviratko* z : farma)
	{
		z->Zvuk();
	}
}
