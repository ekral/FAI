namespace ConsoleApp3
{
    struct Trida
    {
        public int x;
    }

    internal class Program
    {
        static void Zmen(Trida t)
        {
            t.x = 0;        
        }

        static void Main(string[] args)
        {
            Trida t = new Trida();
            t.x = 7;

            Zmen(t);

            Console.WriteLine(t.x);
        }
    }
}
