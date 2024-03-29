#include <stdio.h>
#define _USE_MATH_DEFINES
#include <math.h>
#include <cmath>
#include <Windows.h>

struct Bod2d
{
	double x;
	double y;

	Bod2d(double x, double y) : x(x), y(y)
	{

	}

	Bod2d operator + (Bod2d& other)
	{
		return Bod2d(x + other.x, y + other.y);
	}

	Bod2d operator - (Bod2d& other)
	{
		return Bod2d(x - other.x, y - other.y);
	}
};

// navrhnete strukturu Usecka, vyuzijte strukturu Bod2d
struct Usecka
{
	Bod2d P1;
	Bod2d P2;

	Usecka(Bod2d p1, Bod2d p2) : P1(p1), P2(p2)
	{

	}

	Usecka(double x1, double x2, double y1, double y2)
		: P1(x1, y1), P2(x2, y2)
	{

	}

	Bod2d Stred()
	{
		double x = P1.x + ((P2.x - P1.x) / 2);
		double y = P1.y + ((P2.y - P1.y) / 2);
		return Bod2d(x, y);
	}
};

void Vypis(Usecka* u) {
	Bod2d Stred = u->Stred();
	printf("Usecka: [%lf, %lf] [%lf, %lf] \n", u->P1.x, u->P1.y, u->P2.x, u->P2.y);
	printf("Stred: [%lf, %lf]\n", Stred.x, Stred.y);

}


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

		double maxd = std::abs(dx) > std::abs(dy) ? std::abs(dx) : std::abs(dy);

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

Bod2d RotaceKolemBodu(Bod2d p, Bod2d stred, double stupne)
{
	return Rotace(p - stred, stupne) + stred;
}

enum class Stav
{
	Nahoru,
	Dolu
};

int main()
{
	Stav stav = Stav::Nahoru;

	Platno platno(20, 70, '-');

	Usecka u1(Bod2d(30.0, 10.0), Bod2d(35.0, 15.0));
	Usecka u2(Bod2d(25.0, 8.0), Bod2d(30.0, 12.0));

	HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
	SetConsoleTextAttribute(hConsole, 2);

	double uhel = 0.0;
	double speed = 0.4;

	int pocetOtocek = 0;
	double maxUhel = 10 * 360.0;

	int maxPocetOtacek = 5;

	while (pocetOtocek < maxPocetOtacek)
	{
		platno.Vymaz();

		Bod2d stred = u1.Stred();

		Bod2d pt1 = RotaceKolemBodu(u1.P1, stred, uhel);
		Bod2d pt2 = RotaceKolemBodu(u1.P2, stred, uhel);
		
		platno.NakresliUsecku(pt1, pt2, '1');

		Bod2d stred2 = u2.Stred();

		Bod2d pt3 = RotaceKolemBodu(u2.P1, stred2, uhel);
		Bod2d pt4 = RotaceKolemBodu(u2.P2, stred2, uhel);

		platno.NakresliUsecku(pt3, pt4, '2');

		platno.Zobraz();

		Stav novy;

		switch (stav)
		{
		case Stav::Nahoru:

			if (uhel >= maxUhel)
			{
				novy = Stav::Dolu;

				uhel -= speed;
			}
			else
			{
				novy = Stav::Nahoru;

				uhel += speed;
			}
			break;

		case Stav::Dolu:

			if (uhel <= 0)
			{
				novy = Stav::Nahoru;

				uhel += speed;
				++pocetOtocek;
			}
			else
			{
				novy = Stav::Dolu;

				uhel -= speed;
			}

			break;
		}

		stav = novy;

		printf("%10d\n", pocetOtocek);

	}

	getchar();
}
