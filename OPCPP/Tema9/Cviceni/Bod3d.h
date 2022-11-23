#pragma once

struct Bod3d
{
	double x;
	double y;
	double z;

	Bod3d(double x, double y, double z) : x(x), y(y), z(z)
	{

	}

	Bod3d operator + (Bod3d& other)
	{
		return Bod3d(x + other.x, y + other.y, z + other.z);
	}

	Bod3d operator - (Bod3d& other)
	{
		return Bod3d(x - other.x, y - other.y, z - other.z);
	}
};
