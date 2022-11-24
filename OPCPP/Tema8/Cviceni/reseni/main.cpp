#include <stdio.h>
#define _USE_MATH_DEFINES
#include <math.h>
#include <cmath>
#include <Windows.h>
#include "Usecka.h"
#include "Platno.h"
#include "Algoritmy.h"
#include "AnimovanaUsecka.h"

int main()
{
	HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
	SetConsoleTextAttribute(hConsole, 2);

	Platno platno(20, 70, '-');

	// pridejte druhou usecku
	AnimovanaUsecka u1(Bod2d(25.0, 8.0), Bod2d(30.0, 12.0), 0.0, 0.2, 10 * 360.0);

	for (int i = 0; i < 10000; i++)
	{

		platno.Vymaz();

		u1.Nakresli(platno);

		platno.Zobraz();

		u1.DalsiPozice();

	}

	getchar();
}
