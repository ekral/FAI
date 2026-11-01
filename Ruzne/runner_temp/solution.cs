class Solution
{
    public static void KlientskyKod()
    {
        TridaA a = new TridaA(1);
        System.Diagnostics.Debug.Assert(a.X == 1);

        TridaB b = new TridaB(1, 2);
        System.Diagnostics.Debug.Assert(b.A.X == 1);
    }
}

class TridaA(int x)
{
    public int X {get; set;} = x;
}

class TridaB(int x, int y)
{
    public TridaA A {get; set;} = new TridaA(x);
}