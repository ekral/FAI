using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleApp5
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string text = "ahoj";

            // Do aplikace s vypoctem splatky pridejte ukladani do souboru, 
            // ukladejte datum a cas, dluh, urok, pocet let a vysi splatky

            // Vytvorte dve varianty
            
            // Jednoducha:
            File.AppendAllText("hypoteky.txt", text);

            // S pouzitim using, kdy muzete nechat soubor otevreny pro zapis a pridavat radky

            using (StreamWriter writer = new StreamWriter("c:\\hypoteky.txt", append: true))
            {
                writer.WriteLine(text);
            }

            string url = "https://geek-jokes.sameerkumar.website/api?format=json";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string jsonString = await client.GetStringAsync(url);
                    Console.WriteLine(HttpUtility.HtmlDecode(jsonString));
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
