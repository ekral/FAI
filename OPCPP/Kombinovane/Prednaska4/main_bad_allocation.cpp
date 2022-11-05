#include <new>

int main()
{
	std::size_t n = 8000000000;
	int* p = new(std::nothrow) int[n];
	if (p != nullptr)
	{
		for (int i = 0; i < n; i++)
		{
			p[i] = 0;
		}
	}
	delete[] p;
}
