#include <assert.h>

// funkce ma parametry
// parametry jsou prakticky lokalni promenne na zasobniku
void prohod(int* pa, int* pb)
{
    int tmp = *pa;
    *pa = *pb;
    *pb = tmp;
}

int main()
{
    int a = 2;
    int b = 3;
    
    // vytvori se kopie argumentu
    prohod(&a, &b);
    
	assert(a == 3);
	assert(b == 2);
    
    return 0;
}
