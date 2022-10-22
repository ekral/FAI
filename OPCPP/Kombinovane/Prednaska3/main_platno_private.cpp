#include <stdio.h>

struct Platno
{
	static constexpr int pocetRadku = 16; // tohle uz mozna nebude static
	static constexpr int pocetSloupcu = 50;
	char pozadi;

	char matice[pocetRadku][pocetSloupcu]; // Mozna zmenime na jednorozmerne pole na halde

	Platno(char pozadi) : pozadi(pozadi)
	{
		Vymaz();
	}

	void Vymaz()
	{
		for (int i = 0; i < pocetRadku; i++)
		{
			for (int j = 0; j < pocetSloupcu; j++)
			{
				matice[i][j] = pozadi;
			}
		}
	}

	void Zobraz()
	{
		for (int i = 0; i < pocetRadku; i++)
		{
			for (int j = 0; j < pocetSloupcu; j++)
			{
				putchar(matice[i][j]);
			}

			putchar('\n');
		}
	}

};

int main()
{
	Platno platno('-');
	platno.Zobraz();
	
}
