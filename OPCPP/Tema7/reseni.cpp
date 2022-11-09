#include <stdio.h>
#define _USE_MATH_DEFINES
#include <math.h>
#include <cmath>
#include <Windows.h>
#include <conio.h>

struct Bod2d
{
	double x;
	double y;

	Bod2d(double x, double y) : x(x), y(y)
	{

	}
};

// navrhnete strukturu Usecka, vyuzijte strukturu Bod2d
struct Usecka
{
	double uhel = 0.0;
	double speed = 0.5;

	Bod2d P1;
	Bod2d P2;

	Usecka(Bod2d p1, Bod2d p2, double speed) : P1(p1), P2(p2), speed(speed)
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

// Vypis(&u1);
// Chci vypsat P1, P2 a stred
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
	p.x -= stred.x;
	p.y -= stred.y;

	Bod2d pt = Rotace(p, stupne);

	pt.x += stred.x;
	pt.y += stred.y;

	return pt;
}

enum class Stav
{
	Nahoru,
	Dolu
};

int main()
{

	Stav stav = Stav::Nahoru;

	int max_x = 70;
	int max_y = 20;
	Platno platno(max_y, max_x, '-');

	Usecka u1(Bod2d(31, 6), Bod2d(39, 14.0), 0.5);
	Usecka u2(Bod2d(32, 7), Bod2d(38, 11.0), -1.2);

	HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
	SetConsoleTextAttribute(hConsole, 2);

	int pocetOtocek = 0;
	int maxPocetOtacek = 5;
	double maxUhel = 10 * 360.0;

	while (pocetOtocek < maxPocetOtacek)
	{
		
		platno.Vymaz();

		Bod2d p1t = RotaceKolemBodu(u1.P1, u1.Stred(), u1.uhel);
		Bod2d p2t = RotaceKolemBodu(u1.P2, u1.Stred(), u1.uhel);

		// #include <conio.h>
		if (_kbhit())
		{
			int znak = _getch();
			switch (znak)
			{
				case 'a':
					u1.P1.x -= 1.0;
					u1.P2.x -= 1.0;
					break;
				case 'd':
					u1.P1.x += 1.0;
					u1.P2.x += 1.0;
					break;
				case 's':
					u1.P1.y -= 1.0;
					u1.P2.y -= 1.0;
					break;
				case 'w':
					u1.P1.y += 1.0;
					u1.P2.y += 1.0;
					break;
			}
		}
		platno.NakresliUsecku(p1t, p2t, '1');
		platno.NakresliUsecku(RotaceKolemBodu(u2.P1, u2.Stred(), u2.uhel), RotaceKolemBodu(u2.P2, u2.Stred(), u2.uhel), '2');

		platno.Zobraz();

		u2.uhel += u2.speed;

		Stav novy;

		switch (stav)
		{
		case Stav::Nahoru:

			if (u1.uhel >= maxUhel)
			{
				novy = Stav::Dolu;

				u1.uhel -= u1.speed;
			}
			else
			{
				novy = Stav::Nahoru;

				u1.uhel += u1.speed;
			}
			break;

		case Stav::Dolu:

			if (u1.uhel <= 0)
			{
				novy = Stav::Nahoru;

				u1.uhel += u1.speed;
				++pocetOtocek;
			}
			else
			{
				novy = Stav::Dolu;

				u1.uhel -= u1.speed;
			}

			break;
		}

		stav = novy;

		printf("%10d\n", pocetOtocek);

	}

	getchar();
}
