#include <stdio.h>

struct Platno
{
	char pozadi;
	static constexpr int pocetRadku = 20;
	static constexpr int pocetSloupcu = 70;

	char matice[pocetRadku][pocetSloupcu];

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

	void NakresliBod(int x, int y, char znak)
	{
		matice[y][x] = znak;
	}
};

int main()
{
	Platno platno(' ');
	platno.NakresliBod(5, 5, 'x');
	platno.NakresliBod(0, 0, '1');
	platno.NakresliBod(platno.pocetSloupcu - 1, 0, '2');
	platno.NakresliBod(0, platno.pocetRadku - 1, '3');
	platno.NakresliBod(platno.pocetSloupcu - 1, platno.pocetRadku - 1, '4');
	// platno.NakresliUsecku(Bod(2, 3), Bod(7, 8);)
	for (int i = 0; i < platno.pocetRadku; i++)
	{
		for (int j = 0; j < platno.pocetSloupcu; j++)
		{
			putchar(platno.matice[i][j]);
		}
		putchar('\n');
	}
}
