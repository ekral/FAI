#include <stdio.h>
#include <math.h>

struct Student
{
	char* jmeno;
	double dochazka;
	double test;
};

int main()
{
	struct Student s1 = { "Sandra", 90.0, 70.0 };
	struct Student s2 = { "Filip", 60.0, 65.0 };
	struct Student s3 = { "Josef", 95.0, 45.0 };
	struct Student s4 = { "Samuel", 95.0, 60.0 };

	struct Student studenti[] = { s1, s2, s3, s4 };
	int n = sizeof(studenti) / sizeof(struct Student);

	// Vypiste jmena studentu v poli, kteri splnili zapocet
	// Maji dochazku >= 80.0 a test > 50

	for (int i = 0; i < n; i++)
	{
		struct Student s = studenti[i];

		if (s.dochazka >= 80 && s.test > 50)
		{
			printf("Student splnil zapocet: %s\n", s.jmeno);
		}
	}
}
