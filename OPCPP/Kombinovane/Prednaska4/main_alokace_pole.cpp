// hint - naznak
int x, *p, pole[];

int main()
{
	int n = 1000;
	int* p = new int[n];

	if (p != nullptr)
	{
		for (int i = 0; i < n; i++)
		{
			p[i] = 0;
		}
		delete[] p;
	}
}
