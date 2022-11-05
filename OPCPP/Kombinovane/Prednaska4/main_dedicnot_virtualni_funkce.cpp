#include <stdio.h>
#include <string>

using namespace std;

class Zviratko
{
public:
	string jmeno;

	Zviratko(string jmeno) : jmeno(jmeno)
	{

	}

	virtual string Zvuk()
	{
		return "?";
	}
 };

class Pejsek : public Zviratko
{
public:
	bool bouda;

	Pejsek(string jmeno, bool bouda)
		: Zviratko(jmeno), bouda(bouda)
	{

	}

	string Zvuk() override
	{
		return "haf haf";
	}
};

class Kocicka : public Zviratko
{
public:
	double naplneniZachodu;

	Kocicka(string jmeno, double naplneniZachodu)
		: Zviratko(jmeno), naplneniZachodu(naplneniZachodu)
	{

	}

	string Zvuk() override
	{
		return "mnau";
	}
};

// novy typ reference, zjednodusuje predavani parametru funkcim bez kopirovani
// jednodussi syntaxe nez ukazatel, ale je mozne jej pouzit u pretizenych operatoru
// reference nemuze byt null
void Vypis(Zviratko& zviratko)
{
	printf("jmenuju se %s a delam %s\n", zviratko.jmeno.c_str(), zviratko.Zvuk().c_str());
}

void Vypis(Zviratko* zviratko)
{
	printf("jmenuju se %s a delam %s\n", zviratko->jmeno.c_str(), zviratko->Zvuk().c_str());
}

int main()
{
	// U normalnich nevirtualnich clenskych funkci
	// se zvoli funkce v dobe prekladu dle typu
	// ukazatele nebo reference

	// U virtualnich funkci se rozhodne az za behu 
	// programu dle typu objektu

	Pejsek pejsek("Alf", true);
	Kocicka kocicka("Micka", 0.0);

	pejsek.jmeno.append(" a neco");

	Vypis(pejsek);
	Vypis(kocicka);

	Vypis(&pejsek);
	Vypis(&kocicka);
}
