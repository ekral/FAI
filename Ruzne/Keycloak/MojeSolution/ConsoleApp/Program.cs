using System.Net.Http.Json;
using System.Text.Json;

namespace ConsoleApp;

internal class Program
{
    static async Task<string> GetToken()
    {
        using var client = new HttpClient();
        var resp = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:8080/realms/mujrealm/protocol/openid-connect/token")
        {
            Content = new FormUrlEncodedContent([
                new KeyValuePair<string,string>("client_id","myclient"),
        new KeyValuePair<string,string>("client_secret","BT2eDE1TZOigBkdTfxA55q2jtH7K6T2z"),
        new KeyValuePair<string,string>("grant_type","client_credentials"),
        ])
        });

        var element = await resp.Content.ReadFromJsonAsync<JsonElement>();
        return element.GetProperty("access_token").GetString()!;
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
