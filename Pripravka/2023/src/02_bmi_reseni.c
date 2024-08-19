#include <stdio.h>

int main()
{
	// Nebudeme resit osetreni nespravneho vstupu kvuli zjednodusseni.

	int heightInCentimeters = 180;
	int weightInKilograms = 65;

	printf("Zadej vysku v centimetrech: ");
	scanf_s("%d", &heightInCentimeters ); // adresa promenne heightInCentimeters 
	
	printf("\n");
	
	printf("Zadej hmotnost v kilogramech: ");
	scanf_s("%d", &weightInKilograms ); // adresa promenne weightInKilograms
	
	// Vypocet bmi
	
	double heightInMeters = heightInCentimeters / 100.0; // operace deleni s plovouci carkou
	double bmi = weightInKilograms / (heightInMeters * heightInMeters);

	printf("\n");

	printf("Vyska [cm]: %d, hmotnost [kg]: %d, bmi: %lf", heightInCentimeters, weightInKilograms, bmi);

	return 0;
}
