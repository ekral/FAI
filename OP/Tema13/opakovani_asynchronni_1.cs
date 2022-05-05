using System;
using System.Threading.Tasks;

namespace ConsoleApp36
{
    class Program
    {
        static Task<int> Metoda1()
        {
            return Task.Run(() => { System.Threading.Thread.Sleep(1000); return 1; });
        }

        static async Task<int> KlientskaMetoda1()
        {
            int x = await Metoda1();
            return x;
        }

        static async Task KlientskaMetoda2()
        {
            int x = await Metoda1();
            Console.WriteLine(x);
        }

        static async Task Main(string[] args)
        {
            await KlientskaMetoda2();
            Console.WriteLine("Konec programu");
            //// zavolejte KlientskouMetodu1 a vypiste navratovou hodntou typu 1
            //Task<int> z = Metoda1();

            //int y = await Metoda1();
            //Console.WriteLine(y);

            //Task<int> task = Task.Run(() => { System.Threading.Thread.Sleep(1000); return 1; });
            //// Vypiste hodnotu promenne kterou vraci task, tedy hodnotu 1
            //// Aplikace vytuhne:
            //// Console.WriteLine(promenna.Result);
            //// Vypiste hodnotu tak aby aplikace nevytuhla
            //int x = await  task;
            //Console.WriteLine(x);
        }
    }
}
