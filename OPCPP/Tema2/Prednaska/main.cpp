#include <stdio.h>

int main()
{
	int a = 1;
	float b = 1.0f;
	double c = 1.0; // dvakrat vic presnejsi nez float
	char znak1 = 'a';
	char znak2 = 97;
	char znak3 = '\a';
	bool d = true; // pravda
	bool e = false;// nepravda
	putchar(1);

	int pocet_bytu_x = sizeof(a);
	int pocet_bytu_int = sizeof(int);
	
	printf("hodnota: %d, adresa: %p, pocet bytu: %llx", a, &a, sizeof(a));

	int t1 = 60;
	int t2 = 70;

	// splnil nebo nesplnil zapocet
	// z kazdeho ze dvou testu musel student ziskat vice nez 50 bodu

	bool splnil = t1 > 50 && t2 > 50;
	if (splnil)
	{

	}
	// a zaroven
	if (t1 > 50 && t2 > 50)
	{
		printf("splnil\n");
		printf("gratuluji\n");
	}
	else
	{
		printf("nesplnil\n");
	}

	// nebo
	if (t1 > 50 || t2 > 50)
	{
		printf("splnil\n");
	}
}
