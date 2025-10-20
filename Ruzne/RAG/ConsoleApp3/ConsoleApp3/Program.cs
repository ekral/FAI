namespace ConsoleApp3
{
    class MujTyp
    {
        public int x;
    }

    internal class Program
    {
        static void Zmen(int[] pole)
        {
            pole = [0];        
        }

        static void Main(string[] args)
        {
            int[] pole = [1, 2, 3];

            Zmen(pole);

            Console.WriteLine(string.Join(",", pole));
        }
    }
}
