int main()
{
    double a = 3.0;
    double b = 4.0;
    double c = 5.0;

    // je pravouhly ?
    // v realnem kodu pozor na rovnost, muze dojit k 
    // chybe pri zaokrouhlovani

    // && "je a zaroven"
    if ((a + b > c) && (a + c > b) && (b + c > a))
    {
        puts("Existuje");

        if (a * a + b * b == c * c)
        {
            puts("Je pravouhly");
        }
    }

    if (a + b > c)
    {
        if (a + c > b)
        {
            if (b + c > a)
            {
                puts("Existuje");

                if (a * a + b * b == c * c)
                {
                    puts("Je pravouhly");
                }
            }
        }
    }
}
