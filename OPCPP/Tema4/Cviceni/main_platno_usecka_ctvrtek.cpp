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
		SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE), COORD{ 0, 0 });

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

	void NakresliUsecku(Bod2d p1, Bod2d p2, char znak)
	{
		double dx = p2.x - p1.x;
		double dy = p2.y - p1.y;
		
		double maxd = dx > dy ? dx : dy;

		double x = p1.x;
		double y = p1.y;

		for (int i = 0; i <= maxd; i++)
		{
			NakresliBod(round(x), round(y), znak);

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

int main()
{
	Platno platno('-');

	Bod2d p1(0.0, 0.0);
	Bod2d p2(20.0, 0.0);

	double uhel = 0.0;

	while (uhel < 90)
	{
		Bod2d pt = Rotace(p2, uhel);
		
		platno.Vymaz();
		platno.NakresliUsecku(p1, pt, 'x');
		platno.Zobraz();

		uhel += 1.0;
	}

	getchar();
}
