#pragma once
#include "Bod3d.h"
#include "Platno.h"
#include "Transformace.h"

struct Jehlan
{
	Bod3d pa;
	Bod3d pb;
	Bod3d pc;
	Bod3d pv;

	Jehlan(Bod3d a, Bod3d b, Bod3d c, Bod3d v) : pa(a), pb(b), pc(c), pv(v)
	{
	}

	void Nakresli(Platno& platno)
	{
		double f = 20.0;

		Bod2d a = Projekce(pa, f);
		Bod2d b = Projekce(pb, f);
		Bod2d c = Projekce(pc, f);
		Bod2d v = Projekce(pv, f);

		platno.NakresliUsecku(a, b);
		platno.NakresliUsecku(b, c);
		platno.NakresliUsecku(c, a);
		platno.NakresliUsecku(a, v);
		platno.NakresliUsecku(b, v);
		platno.NakresliUsecku(c, v);

		char puvodni = platno.popredi;

		platno.popredi = 'A';
		platno.NakresliBod(a.x, a.y);
		platno.popredi = 'B';
		platno.NakresliBod(b.x, b.y);
		platno.popredi = 'C';
		platno.NakresliBod(c.x, c.y);
		platno.popredi = 'V';
		platno.NakresliBod(v.x, v.y);

		platno.popredi = puvodni;
	}
};