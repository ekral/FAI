#include <stdio.h>

int main()
{
	// Nebudeme resit osetreni nespravneho vstupu kvuli zjednodusseni.

	int x = 0;
	int y = 0;

	printf("Enter x value:");
	scanf_s("%d", &x ); // adresa promenne x
	
	printf("\n");
	
	printf("Enter y value:");
	scanf_s("%d", &y ); // adresa promenne y
	
	// Spocitat BMI
	// budeme potrebovat operaci deleni s desetinnou carkou
	double bmi = 0.0; // typ s desetinnou carkou
	bmi = 10.0 / 3.0; // vyzkousejte 10 / 3

	printf("\n");
	printf("Bmi is %lf", bmi);

	return 0;
}
