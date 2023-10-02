#include <stdio.h>
#include <stdexcept>

class BankovniUcet
{
private:
	double zustatek; // clenska promenna

public:
	// konstruktor typicky inicializuje clenske promenne, jmenuje se stejne jako trida
	BankovniUcet() : zustatek(0) // member initializer list
	{
		
	}

	// clenske funkce
	double VratZustatek()
	{
		return zustatek;
	}

	void Vloz(double castka)
	{
		zustatek += castka;
	}

	void Vyber(double castka)
	{
		if (castka > zustatek) throw std::invalid_argument("Castka musi byt mensi nez zustatek.");

		zustatek -= castka;
	}
};

int main()
{
	BankovniUcet ucet;
	ucet.Vloz(10000.0);
	ucet.Vyber(200000.0);
	//ucet.zustatek = 100000000;

	printf("zustatek: %lf\n", ucet.VratZustatek());
}