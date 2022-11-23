#pragma once
#define _USE_MATH_DEFINES
#include <math.h>
#include "Bod2d.h"
#include "Bod3d.h"

Bod2d Rotace(Bod2d p, double stupne)
{
	double theta = stupne / 180 * M_PI;

	double xt = p.x * cos(theta) - p.y * sin(theta);
	double yt = p.x * sin(theta) + p.y * cos(theta);

	return Bod2d(xt, yt);
}

Bod2d RotaceKolemBodu(Bod2d p, Bod2d stred, double stupne)
{
	return Rotace(p - stred, stupne) + stred;
}

Bod2d Projekce(Bod3d bod, double f)
{
	double x = f * -bod.x / bod.z;
	double y = f * -bod.y / bod.z;

	return Bod2d(x, y);
}