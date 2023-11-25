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

	Osoba(int id, string jmeno, double hodnoceni) : id(id), jmeno(jmeno), hodnoceni(hodnoceni)
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

	Student(int id, string jmeno, string skupina, double hodnoceni) : Osoba(id, jmeno, hodnoceni), skupina(skupina)
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

	Zamestnanec(int id, string jmeno, string kancelar, double hodnoceni) : Osoba(id, jmeno, hodnoceni), kancelar(kancelar)
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

	Student student(1, "Stepanka", "ISRXY", 80.0);
	student.Vypis();

	Zamestnanec zamestnanec(1, "Kral", "51-822", 70.0);

	Osoba* osoba = &student;
}

