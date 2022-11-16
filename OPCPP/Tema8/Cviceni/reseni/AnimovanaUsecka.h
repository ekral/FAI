#pragma once
#include "Usecka.h"
#include "Transformace.h"

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
	double speed;
	double maxUhel;
	double uhel;
	Stav stav;

	AnimovanaUsecka(Bod2d p1, Bod2d p2, double speed, double maxUhel) 
		: usecka(p1, p2), pt1(p1), pt2(p2), speed(speed), maxUhel(maxUhel), uhel(0.0), stav(Stav::Nahoru)
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
};
