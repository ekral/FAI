class Solution 
{
    public static void Run(int n)
    {
        int faktorial = 1;

        for (int i = 2; i <= n; i++)
        {
            faktorial *= i;
        }

        TestConsole.WriteLine($"faktorial: {faktorial}");
    }
}