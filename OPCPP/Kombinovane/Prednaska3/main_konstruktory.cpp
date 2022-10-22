#include <stdio.h>
#include <math.h>

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
	static constexpr int pocetRadku = 16; // tohle uz mozna nebude static
	static constexpr int pocetSloupcu = 50;
	char matice[pocetRadku][pocetSloupcu]; // Mozna zmenime na jednorozmerne pole na halde
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

	void NakresliBod(double x, double y, char znak)
	{
		int indexSloupce = round(x);
		int indexRadku = pocetRadku - round(y) - 1;

		matice[indexRadku][indexSloupce] = znak;
	}

	void NakresliUsecku(Bod2d p1, Bod2d p2, char znak)
	{
		double dx = p2.x - p1.x;
		double dy = p2.y - p1.y;

		double max = dx > dy ? dx : dy;

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

int main()
{
	Platno platno('-');
	// platno.matice[0][0] = '2'; // tohle chci pred vyvojarem v klientskem kodu skryt
	
	platno.NakresliBod(0, 0, 'O');
	platno.NakresliBod(49, 15, 'P');

	Bod2d p1(0.0, 0.0);
	Bod2d p2(7.0, 10.0);

	platno.NakresliUsecku(p1, p2, 'x');

	platno.Zobraz();
}
