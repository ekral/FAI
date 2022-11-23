#pragma once
#include "Bod2d.h"

struct Usecka
{
	Bod2d P1;
	Bod2d P2;

	Usecka(Bod2d p1, Bod2d p2) : P1(p1), P2(p2)
	{

	}

	Usecka(double x1, double x2, double y1, double y2)
		: P1(x1, y1), P2(x2, y2)
	{

	}

	Bod2d Stred()
	{
		double x = P1.x + ((P2.x - P1.x) / 2);
		double y = P1.y + ((P2.y - P1.y) / 2);
		return Bod2d(x, y);
	}
};
