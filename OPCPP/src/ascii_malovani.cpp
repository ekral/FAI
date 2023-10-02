#include <stdio.h>
#include <assert.h>

// doplnte parametricky konstruktor
// vyuzijte member initializer list
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
	static constexpr int maxX = 30;
	static constexpr int maxY = 20;
	static constexpr int totalChars = maxX * maxY;

	char data[maxX * maxY];
public:
	char pozadi;
	char popredi;

	Platno(char pozadi, char popredi) : pozadi(pozadi), popredi(popredi)
	{
		vymaz();
	}

	void vymaz()
	{
		for (int i = 0; i < totalChars; i++)
		{
			data[i] = popredi;
		}
	}

	void nakresli(Bod2d bod)
	{
		// zapise do dat bod dle souradnic
	}

	void zobraz()
	{
		int pos = 0;

		for (int i = 0; i < maxY; i++)
		{
			for (int i = 0; i < maxX; i++)
			{
				char znak = data[pos];
				++pos;

				putchar(znak);
			}

			putchar('\n');
		}
	}


};
// main nemente
int main()
{
	Bod2d bod1(2.0, 3.0);

	Platno platno('O', 'X');
	platno.nakresli(bod1);
	platno.zobraz();
}


