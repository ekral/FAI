using System;

static class Solution
{
    public static void Test(int a, int b)
    {
        if(a > b)
        {
            TestConsole.WriteLine($"vetsi je a: {a}");
        }
        else if(b > a)
        {
            TestConsole.WriteLine($"vetsi je b: {b}");
        }
        else
        {
            TestConsole.WriteLine($"jsou stejne: a: {a} b: {b}");
        }
    }
}