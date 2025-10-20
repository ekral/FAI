using Duende.IdentityModel;
using Duende.IdentityModel.Client;
using System.Net.Http.Json;
using System.Text.Json;

namespace ConsoleApp;

internal class Program
{
    static async Task<string> GetToken2()
    {
        using var client = new HttpClient();
        var document = await client.GetDiscoveryDocumentAsync("http://127.0.0.1:8080/realms/myrealm");
        var deviceAuth = await client.RequestDeviceAuthorizationAsync(new DeviceAuthorizationRequest
        {
            Address = document.DeviceAuthorizationEndpoint,
            ClientId = "myclient"
        });

        Console.WriteLine($"Login via {deviceAuth.VerificationUriComplete} or ");
        //Console.WriteLine(QRCoder.AsciiQRCodeHelper.GetQRCode(deviceAuth.VerificationUriComplete.Replace("localhost", "192.168.0.249"), QRCoder.QRCodeGenerator.ECCLevel.Q));

        while (true)
        {
            var check = await client.RequestDeviceTokenAsync(new DeviceTokenRequest
            {
                Address = document.TokenEndpoint,
                ClientId = "myclient",
                DeviceCode = deviceAuth.DeviceCode!,
            });

            if (!check.IsError)
            {
                return check.AccessToken!;
            }

            if (check.Error is OidcConstants.TokenErrors.AuthorizationPending or OidcConstants.TokenErrors.SlowDown)
            {
                await Task.Delay(deviceAuth.Interval * 1000);
                continue;
            }

            throw new Exception(check.Error);
        }
    }

    static async Task Main(string[] args)
    {
        using var client = new HttpClient();

        var token = await GetToken2();

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var result = await client.GetStringAsync("https://localhost:7274/data");

        Console.WriteLine($"result: {result}");
        Console.ReadKey();
    }
}
