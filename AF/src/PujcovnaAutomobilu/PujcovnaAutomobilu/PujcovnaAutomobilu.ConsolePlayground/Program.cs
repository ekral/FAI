
using PujcovnaAutomobilu.Models;
using System.Net.Http.Json;
using System.Text.Json;

Automobil automobil = new Automobil() 
{ 
    Id = 1, 
    Model = "Tesla Model X", 
    Pujceno = true 
};

string retezec = JsonSerializer.Serialize(automobil);

//Console.WriteLine(retezec);

Automobil? deserializovany = JsonSerializer.Deserialize<Automobil>(retezec);

using HttpClient client = new HttpClient();

//Automobil? automobil2 = await client.GetFromJsonAsync<Automobil>("https://localhost:7191/Automobil/5");

HttpResponseMessage response = await client.GetAsync("https://localhost:7191/Automobil/1");

if(response.IsSuccessStatusCode)
{
    if(await response.Content.ReadFromJsonAsync<Automobil>() is Automobil automobil1)
    {

    }
}

Console.ReadLine();

