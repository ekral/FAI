struct Bod2d
{
	double x;
	double y;

	Bod2d(double x, double y) : x(x), y(y)
	{

	}
	
	// Bod2d& je reference

	Bod2d operator + (const Bod2d& druhy)
	{
		return Bod2d(x + druhy.x, y + druhy.y);
	}

	Bod2d& operator += (const Bod2d& druhy)
	{
		x += druhy.x;
		y += druhy.y;

		return *this;
	}

	Bod2d soucet(const Bod2d& druhy)
	{
		return Bod2d(x + druhy.x, y + druhy.y);
	}
};


int main()
{
	Bod2d A(2.0, 3.0);
	Bod2d B(3.0, 4.0);

	Bod2d C = A.soucet(B);
	Bod2d D = A + B;

	A += B += C;


}