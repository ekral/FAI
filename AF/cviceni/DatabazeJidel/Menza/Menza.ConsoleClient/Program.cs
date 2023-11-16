using Menza.Models;
using System;
using System.Net.Http.Json;

namespace Menza.ConsoleClient 
{
    internal class Program
    {
        static readonly HttpClient client = new();

        static async Task Main(string[] args)
        {
            Jidlo[]? jidla = await client.GetFromJsonAsync<Jidlo[]>("https://localhost:7007");

            if(jidla is not null)
            {
                foreach (Jidlo jidlo in jidla)
                {
                    await Console.Out.WriteLineAsync($"{jidlo.Id} {jidlo.Nazev} {jidlo.Cena}");
                }
            }

            // 🍌 
            // Uzivatel zada na konzoli id
            // A pomoci http clienta provedte dotaz a vypiste jidlo dle id
            
            Console.WriteLine("Zadej id pizzy");
            
            string? retezec = Console.ReadLine();

            if (retezec is not null)
            {
                if (int.TryParse(retezec, out int id))
                {
                    Console.WriteLine($"Zadane id: {id}");

                    HttpResponseMessage response = await client.GetAsync($"https://localhost:7007/{id}");

                    if(response.IsSuccessStatusCode)
                    {
                        Jidlo? jidlo = await response.Content.ReadFromJsonAsync<Jidlo>();

                        if (jidlo is not null)
                        {
                            Console.WriteLine(jidlo.Nazev);
                            Console.ReadKey();
                        }
                    }
                }
            }
        }

      
    }
}