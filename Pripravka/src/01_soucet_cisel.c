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
	
	int z = x + y;

	printf("\n");
	printf("Value of the variable z is %d", z);

	return 0;
}
