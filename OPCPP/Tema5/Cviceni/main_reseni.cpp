#include <stdio.h>
#define _USE_MATH_DEFINES
#include <math.h>
#include <Windows.h>

struct Bod2d
{
	double x;
	double y;

	Bod2d(double x, double y) : x(x), y(y)
	{

	}

};

struct Platno
{
private:
	// Ukol: změnit z použití matice na zásobníku na použití řetězce znaků včetně znaků pro nový řádek v jednorozměrném poli na haldě.
	int pocetRadku;
	int pocetSloupcu;
	char* data;

public:
	char pozadi;

	Platno(int pocetRadku, int pocetSloupcu, char pozadi) : pozadi(pozadi), pocetRadku(pocetRadku), pocetSloupcu(pocetSloupcu)
	{
		int pocet = (pocetRadku * (pocetSloupcu + 1)) + 1;
		data = new char[pocet];
		Vymaz();
	}

	~Platno()
	{
		delete[] data;
	}

	void Vymaz()
	{
		int index = 0;

		for (int i = 0; i < pocetRadku; i++)
		{
			for (int j = 0; j < pocetSloupcu; j++)
			{
				data[index] = pozadi;
				++index;
			}

			data[index] = '\n';
			++index;
		}

		data[index] = '\0';
	}

	void Zobraz()
	{
		SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE), COORD{ 0, 0 });

		puts(data);
	}

	void NakresliBod(double x, double y, char znak)
	{
		int xRound = (int)round(y);
		int yRound = (int)round(x);

		int indexRadku = pocetRadku - xRound - 1;
		int indexSloupce = yRound;

		int index = (indexRadku * (pocetSloupcu + 1)) + indexSloupce;

		data[index] = znak;
	}

	void NakresliUsecku(Bod2d p1, Bod2d p2, char znak)
	{
		double dx = p2.x - p1.x;
		double dy = p2.y - p1.y;

		double maxd = dx > dy ? dx : dy;

		double x = p1.x;
		double y = p1.y;

		for (int i = 0; i <= maxd; i++)
		{
			NakresliBod(x, y, znak);

			x += (dx / maxd);
			y += (dy / maxd);
		}
	}
};

Bod2d Rotace(Bod2d p, double stupne)
{
	double theta = stupne / 180 * M_PI;

	double xt = p.x * cos(theta) - p.y * sin(theta);
	double yt = p.x * sin(theta) + p.y * cos(theta);

	return Bod2d(xt, yt);
}

enum Stav
{
	Nahoru,
	Dolu
};

int main()
{
	Stav stav = Nahoru;

	Platno platno(20, 70, '-');

	Bod2d p1(0.0, 0.0);
	Bod2d p2(20.0, 0.0);

	double uhel = 0.0;

	while (uhel < 90)
	{
		Bod2d pt = Rotace(p2, uhel);

		platno.Vymaz();
		platno.NakresliUsecku(p1, pt, 'x');
		platno.Zobraz();

		switch (stav)
		{
		case Nahoru:
			uhel += 1.0;

			break;
		case Dolu:

			break;
		}
	}
}
