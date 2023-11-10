﻿using Menza.Models;
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

            Console.ReadLine();
        }

      
    }
}