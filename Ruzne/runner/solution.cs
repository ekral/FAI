using System;

public class Solution
{
    public void Test(double a, double b, int c)
    {
        double diskriminant = b * b - 4 * a * c;

        TestConsole.WriteLine($"diskriminant: {diskriminant:F1}");
    }
}