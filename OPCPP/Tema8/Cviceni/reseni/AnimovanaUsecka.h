#pragma once

#include "Bod2d.h"
#include "Usecka.h"
#include "Algoritmy.h"
#include "Platno.h"

enum class Stav
{
	Nahoru,
	Dolu
};

struct AnimovanaUsecka
{
	Usecka usecka;
	Bod2d pt1;
	Bod2d pt2;
	Stav stav = Stav::Nahoru;
	double uhel = 0.0;
	double speed = 0.4;
	double maxUhel = 10 * 360.0;

	AnimovanaUsecka(Bod2d p1, Bod2d p2, double uhel, double speed, double maxUhel) 
		: usecka(p1, p2), pt1(p1), pt2(p2), uhel(uhel), speed(speed), maxUhel(maxUhel)
	{

	}

	void DalsiPozice()
	{
		Stav novy = stav;

		switch (stav)
		{
		case Stav::Nahoru:

			if (uhel >= maxUhel)
			{
				novy = Stav::Dolu;
				uhel -= speed;
			}
			else
			{
				novy = Stav::Nahoru;
				uhel += speed;
			}
			break;

		case Stav::Dolu:

			if (uhel <= 0)
			{
				novy = Stav::Nahoru;
				uhel += speed;
			}
			else
			{
				novy = Stav::Dolu;
				uhel -= speed;
			}

			break;
		}

		stav = novy;

		Bod2d stred = usecka.Stred();

		pt1 = RotaceKolemBodu(usecka.P1, stred, uhel);
		pt2 = RotaceKolemBodu(usecka.P2, stred, uhel);
	}

	void Nakresli(Platno& platno)
	{
		platno.NakresliUsecku(pt1, pt2, 'x');
	}
};
