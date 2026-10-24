using System;

public class Submission
{
    public static void Test(int cislo)
    {
        int pocetDeleni = 0;

        while (cislo > 0)
        {
            pocetDeleni++;
            cislo /= 2;
        }

        TestConsole.WriteLine($"pocet deleni: {pocetDeleni}");
    }
}