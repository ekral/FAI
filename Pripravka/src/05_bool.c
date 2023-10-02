#include <stdio.h>
#include <stdbool.h>

int main()
{
    // jak funguje && a || v jazyce C
	int test1 = 45;
    int test2 = 45;
    int test3 = 80;
    
    // 0 je nepravda (false)
    // cokoliv co neni 0 je pravda (true)
    
    bool v1 = test1 > 50;
    int v2 = test1 > 50;
    int v3 = 0 && 1;
    int v4 = 0 && -1;
    int v5 = 1 && 1;
    int v6 = 1 && -1;
    int v7 = true && false;
    
    printf("test1 > 50 = %d\n", test1 > 50);
    printf("0 && 1 = %d\n", 0 && 1);
    printf("0 && -1 = %d\n", 0 && -1);
    printf("1 && 1 = %d\n", 1 && 1);
    printf("1 && -1 = %d\n", 1 && -1);
    printf("-9 && 5 = %d\n", -9 && 5);
    printf("7 && -1 = %d\n", 7 && -1);
    // V predmetu jste psali tri testy.
    // Vypiste text "uspel" pokud alespon dva z testu jsou za vice 
    // nez 50 bodu.
    
    // reseni s && (a zaroven) a || (nebo)
    if(test1>50 && test2>50 || test2>50 && test3>50 || test1>50 && test3>50)
        puts("Student uspel");
    else puts("Student neuspel");
	return 0;
}
