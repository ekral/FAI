#include <stdio.h>
#include <math.h>
#include <numbers>
#include <Windows.h>

struct Bod2D
{
	double x;
	double y;

	Bod2D(double x, double y) : x(x), y(y)
	{

	}
};

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
		SetConsoleCursorPosition( GetStdHandle(STD_OUTPUT_HANDLE), COORD { 0, 0 } );

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

	void NakresliUsecku(Bod2D p1, Bod2D p2, char znak)
	{
		double dx = p2.x - p1.x;
		double dy = p2.y - p1.y;

		double max = dx > dy ? dx : dy;

		double x = p1.x;
		double y = p1.y;

		for (double i = 0; i < max; i++)
		{
			NakresliBod(round(x), round(y), znak);
			
			x += dx / max;
			y += dy / max;

		}

	}
};

Bod2D Rotace(Bod2D p, double stupne)
{
	double theta = stupne * std::numbers::pi / 180;

	double xt = p.x * cos(theta) - p.y * sin(theta);
	double yt = p.x * sin(theta) + p.y * cos(theta);

	return Bod2D(xt, yt);
}

int main()
{
	Platno platno('-');

	double uhel = 0.0;

	while (uhel < 45.0)
	{
		Bod2D p1(0.0, 0.0);
		Bod2D p2(25.0, 0.0);

		platno.Vymaz();

		Bod2D pt = Rotace(p2, uhel);

		platno.NakresliUsecku(p1, pt, 'x');

		platno.Zobraz();

		++uhel;
	}
}
