namespace ConsoleApp2
{

    internal class Program
    {
        static void Zmen(out int[] pole)
        {
            pole = [0];
        }

        static void Main(string[] args)
        {
            int[] pole = [1, 2, 3];
            Zmen(out pole);    

            System.Console.WriteLine(string.Join(",", pole));
        }
    }
}
