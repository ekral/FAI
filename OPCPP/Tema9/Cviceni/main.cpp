#include <stdio.h>
#include "Platno.h"
#include "AnimovanaUsecka.h"
#include "Jehlan.h"

void Vypis(Usecka* u)
{
	Bod2d Stred = u->Stred();
	printf("Usecka: [%lf, %lf] [%lf, %lf] \n", u->P1.x, u->P1.y, u->P2.x, u->P2.y);
	printf("Stred: [%lf, %lf]\n", Stred.x, Stred.y);
}

int main()
{
	Platno platno(20, 70, '-', 'x');

	AnimovanaUsecka animovanaUsecka1(Bod2d(30.0, 10.0), Bod2d(35.0, 15.0), 0.2, 10 * 360.0);
	AnimovanaUsecka animovanaUsecka2(Bod2d(20.0, 10.0), Bod2d(15.0, 15.0), 0.4, 1 * 360.0);

	Jehlan jehlan(Bod3d(5.0, 1.0, -4.0), Bod3d(1.0, 1.0, -4.0), Bod3d(3.0, 1.0, -7.0), Bod3d(2.0, 5.0, -6.0));

	for (int i = 0; i < 10000; i++)
	{
		platno.Vymaz();

		//animovanaUsecka1.Nakresli(platno);
		//animovanaUsecka2.Nakresli(platno);
		jehlan.Nakresli(platno);

		platno.Zobraz();

		//animovanaUsecka1.DalsiPozice();
		//animovanaUsecka2.DalsiPozice();
	}

	int znak = getchar();
}
