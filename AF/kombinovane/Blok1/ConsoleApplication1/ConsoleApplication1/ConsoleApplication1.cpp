// ConsoleApplication1.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <stdio.h>
#include <stdlib.h>
#include <vector>

class Platno
{
private:
    int m;
    int n;
    std::vector<char> data; // dynamicke pole
public:
    Platno(int m, int n) : m(m), n(n), data(m*n, 0) // pouzijte member initializer list a proc je to lepsi
    {
        printf("data: %zu", data.size());
    }
    
    ~Platno() // destruktor
    {
        
    }
};

int main()
{
    Platno platnoA(10, 20);
    Platno platnoB = platnoA;

}


