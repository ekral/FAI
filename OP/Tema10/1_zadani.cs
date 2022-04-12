// Úkol 1:
// Načtěte vtip z následující adresy, umožněte uživateli zadat hodnocení vtipu 1 nejhorší až 5 nejlepší a uložte jej do databáze vtipů. 
// Program umožní načít a ohodnotit nový vtip a zobrazit seznam již ohodnocených vtipů seřazení od nejlepšího hodnocení po nejhorší.
// https://geek-jokes.sameerkumar.website/api?format=plain

using Spectre.Console;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp27
{
    enum Volba
    {
        NactiVtip,
        VypisVtipy,
        Konec
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            Volba volba = AnsiConsole.Prompt(new SelectionPrompt<Volba>().AddChoices(Volba.NactiVtip, Volba.VypisVtipy, Volba.Konec));

            switch (volba)
            {
                case Volba.NactiVtip:
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            string text = await client.GetStringAsync("https://geek-jokes.sameerkumar.website/api?format=plain");
                            Console.WriteLine(text);
                        }
                        catch (HttpRequestException ex)
                        {

                        }
                    }
                    break;
                case Volba.VypisVtipy:
                    break;
                case Volba.Konec:
                    break;
                default:
                    break;
            }
           
        }
    }
}
