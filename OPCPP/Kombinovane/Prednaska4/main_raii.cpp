#include <stdio.h>

class Platno
{
private:
	int n;
	char pozadi;
	char* data;
public:
	Platno(Platno& other) = delete;

	Platno(int pocetRadku, int pocetSloupcu, char pozadi) 
		: n(pocetRadku * pocetSloupcu), pozadi(pozadi)
	{
		data = new char[n];
		printf("Alokoval jsem pamet\n");
	}

	~Platno()
	{
		printf("Uvolnuji pamet\n");
		delete[] data;
	}
};

int main()
{
	if (true)
	{
		Platno platno(20, 30, '-');
		//Platno kopie = platno;
	}
}
