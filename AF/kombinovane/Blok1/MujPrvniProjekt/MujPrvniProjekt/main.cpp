#include <stdio.h>

int main()
{
	int x = 2;
	&x; // adresa promenne, & adresni operator (operator reference)
	size_t pocet = sizeof(x); // kolik bajtu v pamati zabere promenna
	pocet = sizeof(int); // kolik bajtu v pameti zabere typ.

	printf("x ma hodnotu %d\n", x);
	printf("x je ulozena na adrese %p\n", &x);
	printf("x zabira v pameti %llu\n", sizeof(x));

	int vyskaCm = 180;
	int hmotnostKg = 65;

	double vyskaMetry = (double)vyskaCm / 100.0;

	printf("vyska[m]: %lf\n", vyskaMetry);
	printf("vaha[kg]: %d\n", hmotnostKg);

	double bmi = hmotnostKg / (vyskaMetry * vyskaMetry);

	printf("BMI: %lf\n", bmi);




}