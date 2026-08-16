#include <stdio.h>

int main()
{
	int pole[5] = { 2,8,9,6,3 };
	int n = 5;

	// vypiste na terminal hodnotu prvniho prvku v poli
	printf("prvni: %d\n", pole[0]);
	// vypiste na terminal hodnotu posledniho prvku v poli
	printf("posledni: %d\n", pole[4]);

	// Vypiste prvky v poli v opacnem poradi
	// 3,6,9,8,2

	for (int i = 0; i < n; i++)
	{
		int prvek = pole[4 - i];
		printf("%d\n", prvek);
	}

	// DÚ: zmente prvky v poli tak, aby byly v opacnem poradi

}
