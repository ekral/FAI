int[] pole = new int[4] { 1, 2, 3, 4 };
MojeFunkce(pole);

Console.WriteLine(string.Join(",", pole));

static void MojeFunkce(int[] neco)
{
    neco[0] = 7;
    neco[1] = 7;
    neco[2] = 7;
    neco[3] = 7;
}
