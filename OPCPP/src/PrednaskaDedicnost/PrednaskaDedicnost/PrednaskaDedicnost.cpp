#include <string>
#include <stdio.h>

using namespace std;

// Rodic
// Base class

class Osoba
{
private:
	int id;
protected:
	double hodnoceni;
public:
	string jmeno;

	Osoba(int id, string jmeno) : id(id), jmeno(jmeno)
	{
		puts("Konstruktor Osoby");
	}

	~Osoba()
	{
		puts("Destruktor Osoby");
	}

	int GetId()
	{
		return id;
	}
};

// Potomek
// Derived class

// Potomek dedi od rodice

class Student : public Osoba
{
public:
	string skupina;

	Student(int id, string jmeno, string skupina) : Osoba(id, jmeno), skupina(skupina)
	{
		puts("Konstruktor Studenta");
	}

	~Student()
	{
		puts("Destruktor Studenta");
	}

	void Vypis()
	{
		printf("%d %s %s %lf\n", GetId(), jmeno.c_str(), skupina.c_str(), hodnoceni);
	}
};


class Zamestnanec : public Osoba
{
public:
	string kancelar;

	Zamestnanec(int id, string jmeno, string kancelar) : Osoba(id, jmeno), kancelar(kancelar)
	{
		puts("Konstruktor Zamestnance");
	}

	~Zamestnanec()
	{
		puts("Destruktor Yamestnance");
	}
};

int main()
{
	// Klientsky kod

	Student student(1, "Stepanka", "ISRXY");
	student.Vypis();

	Zamestnanec zamestnanec(1, "Kral", "51-822");

	Osoba* osoba = &student;
}

