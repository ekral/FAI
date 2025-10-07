namespace ConsoleApp4
{
    class MujTyp
    {
        public int x;
    }

    internal class Program
    {
        static void Zmen(ref MujTyp t)
        {
            t = new MujTyp();
            t.x = 0;
        }
        static void Main(string[] args)
        {
            MujTyp t = new MujTyp();
            t.x = 7;
            Zmen(ref t);
            Console.WriteLine(t.x);
        }
    }
}
