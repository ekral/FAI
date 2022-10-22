#include <stdio.h>

struct Platno
{
	static constexpr int pocetRadku = 20;
	static constexpr int pocetSloupcu = 50;
	char pozadi;

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

};

int main()
{
	Platno platno('-');
}
