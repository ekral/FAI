class Solution
{
    public static void KlientskyKod()
    {
        Rodic rodic = new Rodic(1, 2);
        rodic.VypisX(); // vypíše "x: 1, y: 2"

        Potomek potomek = new Potomek(3, 4);
        potomek.VypisY(); // vypíše "y: 4"
    }
}

class Rodic(int x, int y)
{
    private int x = x;
    protected int y = y;

    public void VypisX()
    {
        TestConsole.WriteLine($"x: {x}, y: {y}");
    }

}

class Potomek(int x, int y) : Rodic(x, y)
{
    public void VypisY()
    {
        TestConsole.WriteLine($"y: {y}");
    }
}