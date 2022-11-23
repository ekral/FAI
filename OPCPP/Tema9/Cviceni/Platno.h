#pragma once
#define _USE_MATH_DEFINES
#include <math.h>
#include <stdio.h>
#include <Windows.h>
#include "Bod2d.h"

struct Platno
{
private:
	// Ukol: zmìnit z použití matice na zásobníku na použití øetìzce znakù vèetnì znakù pro nový øádek v jednorozmìrném poli na haldì.
	int pocetRadku;
	int pocetSloupcu;
	char* data;

public:
	char pozadi;
	char popredi;

	Platno(Platno&) = delete;

	Platno(int pocetRadku, int pocetSloupcu, char pozadi, char popredi) 
		: pocetRadku(pocetRadku), pocetSloupcu(pocetSloupcu), pozadi(pozadi), popredi(popredi)
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
		HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
		SetConsoleTextAttribute(hConsole, 2);

		SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE), COORD{ 0, 0 });

		puts(data);
	}

	void NakresliBod(double x, double y)
	{
		int xRound = (int)round(y);
		int yRound = (int)round(x);

		int indexRadku = pocetRadku - xRound - 1;
		int indexSloupce = yRound;

		int index = (indexRadku * (pocetSloupcu + 1)) + indexSloupce;

		data[index] = popredi;
	}

	void NakresliUsecku(Bod2d p1, Bod2d p2)
	{
		double dx = p2.x - p1.x;
		double dy = p2.y - p1.y;

		double maxd = fabs(dx) > fabs(dy) ? fabs(dx) : fabs(dy);

		double x = p1.x;
		double y = p1.y;

		for (int i = 0; i <= maxd; i++)
		{
			NakresliBod(x, y);

			x += (dx / maxd);
			y += (dy / maxd);
		}
	}
};

