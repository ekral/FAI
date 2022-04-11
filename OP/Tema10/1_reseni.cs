using Spectre.Console;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tema10
{

    class Program
    {

        static async Task<string> NactiVtipAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string text = await client.GetStringAsync("https://geek-jokes.sameerkumar.website/api?format=plain");

                    return text;
                }
                catch (HttpRequestException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        enum Volba
        {
            NactiVtip,
            VypisVtipy,
            Konec,
        }

        static async Task Main(string[] args)
        {
            Dictionary<string, int> vtipy = new Dictionary<string, int>();

            bool konec = false;

            do
            {
                Console.Clear();

                Volba volba = AnsiConsole.Prompt(new SelectionPrompt<Volba>().AddChoices(Volba.NactiVtip, Volba.VypisVtipy, Volba.Konec));

                switch (volba)
                {
                    case Volba.NactiVtip:
                        {
                            string text = await NactiVtipAsync();
                            AnsiConsole.MarkupLine($"[Green]{text}[/]");
                            
                            int hodnoceni = AnsiConsole.Prompt(new SelectionPrompt<int>().AddChoices(1, 2, 3, 4, 5));

                            vtipy[text] = hodnoceni;
                        }
                        break;

                    case Volba.VypisVtipy:
                        foreach (var vtip in vtipy.OrderByDescending(vtip => vtip.Value))
                        {
                            AnsiConsole.MarkupLine($"[Red]{vtip.Value}[/] {vtip.Key}");
                        }
                        Console.ReadKey(true);
                        break;

                    case Volba.Konec:
                        konec = true;
                        break;
                }

            } while (!konec);
        }
    }
}
