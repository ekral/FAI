#include <stdio.h>
#define _USE_MATH_DEFINES
#include <math.h>
#include <Windows.h>

// u struktury je vse public, dokud to nezmenime na private
// u tridy je vse private, dokud to nezmenime na public

struct Bod2d
{
	double x;
	double y;

	// doplnit parametricky konstruktor
	Bod2d(double x, double y) : x(x), y(y)
	{

	}
};

struct Platno
{
private:
	int pocetRadku = 16; // tohle uz mozna nebude static
	int pocetSloupcu = 50;

	char* pole; // Mozna zmenime na jednorozmerne pole na halde
public:
	char pozadi;

	Platno(int pocetRadku, int pocetSloupcu, char pozadi) : pocetRadku(pocetRadku), pocetSloupcu(pocetSloupcu), pozadi(pozadi)
	{
		pole = new char[pocetRadku * pocetSloupcu + pocetRadku + 1];
		Vymaz();
	}

	~Platno()
	{
		delete[] pole;
	}

	void Vymaz()
	{
		int index = 0;

		for (int i = 0; i < pocetRadku; i++)
		{
			for (int j = 0; j < pocetSloupcu; j++)
			{
				pole[index] = pozadi;
				++index;
			}

			pole[index] = '\n';
			++index;
		}

		pole[index] = '\0';
	}

	void Zobraz()
	{
		SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE), COORD{ 0, 0 });
		puts(pole);
	}

	void NakresliBod(double x, double y, char znak)
	{
		int indexSloupce = round(x);
		int indexRadku = pocetRadku - round(y) - 1;

		int index = indexRadku * (pocetSloupcu + 1) + indexSloupce;

		pole[index] = znak;
	}

	void NakresliUsecku(Bod2d p1, Bod2d p2, char znak)
	{
		double dx = p2.x - p1.x;
		double dy = p2.y - p1.y;

		double max = fabs(dx) > fabs(dy) ? fabs(dx) : fabs(dy);

		double x = p1.x;
		double y = p1.y;

		for (double i = 0; i < max; i++)
		{
			x += dx / max;
			y += dy / max;

			NakresliBod(x, y, znak);
		}
	}
};

Bod2d Rotace(Bod2d p, double stupne)
{
	double theta = stupne * M_PI / 180;
	double xt = p.x * cos(theta) - p.y * sin(theta);
	double yt = p.x * sin(theta) + p.y * cos(theta);

	return Bod2d(xt, yt);
}

int main()
{
	Platno platno(16, 50, '-');

	Bod2d p1(0.0, 0.0);
	Bod2d p2(14.0, 0.0);

	double uhel = 0.0;

	bool nahoru = true;

	for (int i = 0; i < 10000; i++)
	{
		Bod2d pt = Rotace(p2, uhel);

		platno.Vymaz();

		platno.NakresliUsecku(p1, pt, 'x');

		platno.Zobraz();

		uhel = uhel + (nahoru ? 1.0 : -1);

		if (uhel >= 90.0)
		{
			nahoru = false;
			uhel = 90.0;
		}
		else if (uhel <= 0)
		{
			nahoru = true;
			uhel = 0.0;
		}

	}
}
