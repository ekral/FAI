#include <stdio.h>
#include <math.h>

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

	void NakresliBod(double x, double y, char znak)
	{
		int indexSloupce = round(x);
		int indexRadku = pocetRadku - round(y) - 1;

		matice[indexRadku][indexSloupce] = znak;
	}

};

int main()
{
	Platno platno('-');
	platno.NakresliBod(0, 0, 'O');
	platno.NakresliBod(49, 15, 'P');
	platno.Zobraz();
}
