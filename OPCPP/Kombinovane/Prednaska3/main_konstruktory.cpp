#include <stdio.h>

struct Platno
{
	static constexpr int pocetRadku = 20;
	static constexpr int pocetSloupcu = 50;
	char pozadi;

	char matice[pocetRadku][pocetSloupcu];

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

};

// funkci pro vypis rozmeru obdelniku na konzoli

struct Obdelnik
{
	double a;
	double b;

	// konstruktor je specialni clenska funkce
	// jmenuje se stejne jako trida a nema zadny navratovy typ ani void
	Obdelnik() : a(2.0), b(3.0) // member initializer list
	{
	}

	Obdelnik(double a, double b) : a(a), b(b) // member initializer list
	{
	}
};

void Vypis(Obdelnik* ob)
{
	printf("a: %lf, b: %lf\n", ob->a, ob->b);
};

// definice funkce Vypis ktera vypise printf("a: %lf b: %lf\n", o.a, o.b);

struct Trojuhelnik
{
	double a;
	double b;
	double c;

	// definujte parametricky konstruktor
	// tak abych v klientskem kodu musel vzdy zadat delky vsech stran
	// vyuzijte member initializer list

	Trojuhelnik(double a, double b, double c) : a(a), b(b), c(c)
	{
		
	}
};

int main()
{
	Trojuhelnik t1(4.0, 5.0, 6.0);
	Obdelnik o1;
	Vypis(&o1);
	Obdelnik o2(5.0, 7.0);
	Vypis(&o2);

	Platno platno('-');
}
