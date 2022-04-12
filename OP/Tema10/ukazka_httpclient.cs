using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tema10
{
    class Program
    {
        // TODO osetrit vyjimky
        static async Task<string> StahniVtipAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("https://geek-jokes.sameerkumar.website/api?format=plain");

                if (response.IsSuccessStatusCode)
                {
                    string vtip = await response.Content.ReadAsStringAsync();
                    return vtip;
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ReasonPhrase);
                }
            }

            return "chyba serveru";
        }

        static async Task<string> StahniVtip2Async()
        {
            using (HttpClient client = new HttpClient())
            {
                string vtip = await client.GetStringAsync("https://geek-jokes.sameerkumar.website/api?format=plain");
                return vtip;
            }
        }

        static async Task Main(string[] args)
        {
            List<Task<string>> tasky = new List<Task<string>>
            {
                StahniVtip2Async(),StahniVtipAsync(),StahniVtipAsync()
            };
           
            Task<string[]> task = Task.WhenAll(tasky);
            
            try
            {
                string[] vtipy = await task;
                foreach (string vtip in vtipy)
                {
                    Console.WriteLine(vtip);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"hlavni: {ex.Message}");
                foreach (var vyjimky in task.Exception.InnerExceptions)
                {
                    Console.WriteLine(vyjimky.Message);
                }
            }
        }
    }
}
