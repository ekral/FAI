using System;

class Submission
{
    public static void Test(int zustatek)
    {
        int i = 1;

        while (zustatek > i )
        {
            zustatek -= i;
            i++;
        }

        TestConsole.WriteLine($"pocet kroku: {i - 1}");
    }
}