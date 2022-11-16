#include <stdio.h>
#include "Platno.h"
#include "AnimovanaUsecka.h"

void Vypis(Usecka* u) 
{
	Bod2d Stred = u->Stred();
	printf("Usecka: [%lf, %lf] [%lf, %lf] \n", u->P1.x, u->P1.y, u->P2.x, u->P2.y);
	printf("Stred: [%lf, %lf]\n", Stred.x, Stred.y);
}

int main()
{
	Platno platno(20, 70, '-');

	AnimovanaUsecka animovanaUsecka1(Bod2d(30.0, 10.0), Bod2d(35.0, 15.0), 0.2, 10 * 360.0);
	AnimovanaUsecka animovanaUsecka2(Bod2d(20.0, 10.0), Bod2d(15.0, 15.0), 0.4,  1 * 360.0);

	for (int i = 0; i < 10000; i++)
	{
		platno.Vymaz();

		platno.NakresliUsecku(animovanaUsecka1.pt1, animovanaUsecka1.pt2, '1');
		platno.NakresliUsecku(animovanaUsecka2.pt1, animovanaUsecka2.pt2, '1');

		platno.Zobraz();

		animovanaUsecka1.DalsiPozice();
		animovanaUsecka2.DalsiPozice();
	}

	int znak = getchar();
}
