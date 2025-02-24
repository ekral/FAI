namespace ConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MujClient client = new MujClient(new HttpClient());
            

            Console.WriteLine("Hello, World!");
        }
    }
}
