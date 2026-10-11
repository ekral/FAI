class Submission 
{
    public static void Test(int x, int y)
    {
        int mocnina = x;

        for (int i = 0; i < y; i++)
        {
            mocnina *= x;
        }

        TestConsole.WriteLine($"mocnina: {mocnina}");
    }
}