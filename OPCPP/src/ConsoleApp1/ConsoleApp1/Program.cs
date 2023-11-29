namespace ConsoleApp1
{
    struct Ctverec
    {
        public double n;

        public Ctverec(double n)
        {
            this.n = n;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Ctverec[] ctverec = new Ctverec[3];

            Ctverec c1 = new Ctverec(2);
            Console.WriteLine(c1.n);
        }
    }
}