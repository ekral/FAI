#pragma once

struct Bod2d
{
	double x;
	double y;

	Bod2d(double x, double y) : x(x), y(y)
	{

	}

	Bod2d operator + (Bod2d& other)
	{
		return Bod2d(x + other.x, y + other.y);
	}

	Bod2d operator - (Bod2d& other)
	{
		return Bod2d(x - other.x, y - other.y);
	}
}; 
