using Duende.IdentityModel;
using Duende.IdentityModel.Client;
using System.Net.Http.Json;
using System.Text.Json;

namespace ConsoleApp;

internal class Program
{
    //credentials flow
    static async Task<string> GetToken()
    {
        using var client = new HttpClient();
        var document = await client.GetDiscoveryDocumentAsync("http://127.0.0.1:8080/realms/myrealm");

        var resp = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
        {
            ClientId = "myclient",
            UserName = "ekral",
            Password = "heslo",
            Address = document.TokenEndpoint,
        });
        return resp.AccessToken!;
    }

    static async Task Main(string[] args)
    {
        using var client = new HttpClient();

        var token = await GetToken();

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var result = await client.GetStringAsync("https://localhost:7274/data");

        Console.WriteLine($"result: {result}");
        Console.ReadKey();
    }
}
