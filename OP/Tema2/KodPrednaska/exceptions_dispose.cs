using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp14
{
    class Program
    {

        static void Metoda1()
        {
            string path = "text.txt";

            if (File.Exists(path))
            {
                try
                {
                    string text = File.ReadAllText(path);
                }
                catch(FileNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch(IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static async Task Metoda2Async()
        {
            string url = "https://geek-jokes.sameerkumar.website/api?format=json";

            HttpClient client = new HttpClient();
            
            try
            {
                string jsonString = await client.GetStringAsync(url);
                Console.WriteLine(jsonString);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Dispose();
            }

        }

        static async Task Metoda3Async()
        {
            string url = "https://geek-jokes.sameerkumar.website/api?format=json";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string jsonString = await client.GetStringAsync(url);
                    Console.WriteLine(jsonString);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        static async Task Main(string[] args)
        {
            Metoda1();

            await Metoda2Async();
            await Metoda3Async();

        }
    }
}
