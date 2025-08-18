// MojeDalsiAplikace.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <stdio.h>

int main()
{
    
    for (int i = 0; i < 10; i++)
    {
        for (int j = 0; j < i + 1; j++)
        {
            putchar('*');
        }

        putchar('\n');
    }
    
    // s pomoci cyklu for vypiste na terminal nasledujici obrazec
    // *
    // **
    // ***
    // ****
    // *****
    // ******
    // *******
    // ********
    // *********
    // **********

}
