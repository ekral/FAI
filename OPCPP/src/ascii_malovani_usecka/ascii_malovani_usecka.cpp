#include <stdio.h>

struct Bod2d
{
	double x;
	double y;

	Bod2d(double x, double y) : x(x), y(y)
	{
	
	}
};

class Platno
{
private:
	static constexpr int columnCount = 30;
	static constexpr int rowCount = 20;
	static constexpr int totalChars = columnCount * rowCount;
	
	char data[totalChars];
public:
	static constexpr int maxColumnIndex = columnCount - 1;
	static constexpr int maxRowIndex = rowCount - 1;
	char pozadi;
	char popredi;

	Platno(char pozadi, char popredi) : pozadi(pozadi), popredi(popredi), data{ 0 }
	{
		Vymaz();
	}

	void Vymaz()
	{
		for (int i = 0; i < totalChars; i++)
		{
			data[i] = pozadi;
		}
	}
	
	void NakresliBod(double x, double y)
	{
		int pos = ((rowCount - y - 1) * columnCount) + x;

		data[pos] = popredi;
	}

	void Zobraz()
	{
		int pos = 0;

		for (int i = 0; i < rowCount; i++)
		{
			for (int j = 0; j < columnCount; j++)
			{
				char znak = data[pos];
				++pos;

				putchar(znak);
			}

			putchar('\n');
		}
	}

};

int main()
{
	Bod2d p1(2.0, 3.0);
	Bod2d p2(5.0, 6.0);

	Platno platno('-', 'x');

	bool konec = true;

	do
	{
		platno.Vymaz();
		platno.NakresliBod(2, 3);

		platno.popredi = 'O';
		platno.NakresliBod(0, 0);

		platno.popredi = '1';
		platno.NakresliBod(platno.maxColumnIndex, 0);

		platno.popredi = '2';
		platno.NakresliBod(platno.maxColumnIndex, platno.maxRowIndex);

		platno.popredi = '3';
		platno.NakresliBod(0, platno.maxRowIndex);

		platno.popredi = 'x';
		// platno.NakresliUsecku(p1, p2);

		platno.Zobraz();

		// zpracujeme vstupy z klavesnice

		// spocitame nove pozice
	} while (!konec);
}
