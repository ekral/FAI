#include <stdio.h>

struct Platno
{
private:
	static constexpr int pocetRadku = 20;
	static constexpr int pocetSloupcu = 70;
	char matice[pocetRadku][pocetSloupcu]; // TODO prevest na jednorozmerne pole alokovane na hadle, abychom mohli zvolit rozmery platna pri vytvareni platna

public:
	char pozadi;

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

	void NakresliBod(int x, int y, char znak)
	{
		int i = pocetRadku - y - 1;
		int j = x;
		matice[i][j] = znak;
	}
};

int main()
{
	Platno platno('-');
	platno.NakresliBod(0, 0, 'O');
	platno.NakresliBod(0, 19, '1');
	platno.NakresliBod(69, 19, '2');
	platno.NakresliBod(69, 0, '3');

	platno.Zobraz();

	getchar();
}
