#include<stdio.h>

int main()
{
	double hmotnost = 5.0;
	double vyska = 1.83;

	double bmi = hmotnost / (vyska * vyska);
	printf("bmi: %.1lf\n", bmi);
	
	if (bmi < 18.0)
	{
		printf("priber\n");
	}
	else if (bmi > 25.0)
	{
		printf("zhubni\n");
	}
	else
	{
		printf("Idealni vaha\n");
	}
	
	// Vypiste text idealni vaha, pokud je bmi v rozsahu <18, 25>
	// pomoci jednoho prikazu a logickeho operatoru && (a zaroven)
	if ((bmi >= 18.0) && (bmi <= 25.0))
	{
		printf("Idealni vaha\n");
	}

	// Cviceni: Vypiste idealni vaha pomoci dvou vnorenych if
	if (bmi >= 18.0)
	{
		if (bmi <= 25.0)
		{
			printf("Idealni vaha\n");
		}
	}

	// pomoci jednoho prikazu vypiste "Nemas idealni vahu" a spuste alarm
	// pokud neni bmi v rozsahu <18, 25>
	// || (nebo)

	if (bmi < 18.0 || bmi > 25)
	{
		printf("Nemas idealni vahu\a\n");
	}
}