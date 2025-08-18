int main()
{
    // student psal tri testy, mohl ziskat 0 az 100%
    double t1 = 40;
    double t2 = 60;
    double t3 = 60;

    // 1. Student musel napsat vsechny na vice nez 50
    if (t1 > 50 && t2 > 50 && t3 > 50)
    {
        puts("Splnil vsechny tri");
    }

    // 3. Student musel napsat dva ze tri testu na vice nez 50
    if ((t1 > 50 && t2 > 50) || (t1 > 50 && t3 > 50) || (t2 > 50 && t3 > 50))
    {
        puts("Splnil alespon dva");
    }

    // 2. Student musel napsat jeden test na vice nez 50
    if (t1 > 50 || t2 > 50 || t3 > 50)
    {
        puts("Splnil alespon jede");
    }

    int pocet = 0;

    if (t1 > 50) ++pocet;
    if (t2 > 50) ++pocet;
    if (t3 > 50) ++pocet;

    if (pocet > 1)
    {
        puts("Splnil");
    }
}
