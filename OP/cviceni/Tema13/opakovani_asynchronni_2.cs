using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp37
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (HttpClient client = new HttpClient())
            {
                // s pomoci metody GetStringAsync nactete z adresy https://geek-jokes.sameerkumar.website/api?format=plain
                string text = await client.GetStringAsync("https://geek-jokes.sameerkumar.website/api?format=plain");
                // vtip a vypiste ho na konzoli
                Console.WriteLine(text);
            }
        }
    }
}
