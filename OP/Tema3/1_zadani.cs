using System;

namespace ConsoleApp7
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            string text = "ahoj";

            // Do aplikace s vypoctem splatky pridejte ukladani do souboru, 
            // ukladejte datum a cas, dluh, urok, pocet let a vysi splatky

            string url = "https://geek-jokes.sameerkumar.website/api?format=json";

            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                //System.Net.Http.HttpResponseMessage response = await client.GetAsync(url);

                //if(response.IsSuccessStatusCode)
                //{
                //    string joke = await response.Content.ReadAsStringAsync();
                //    Console.WriteLine(joke);
                //}

                string jsonString = await client.GetStringAsync(url);
                Console.WriteLine(jsonString);
            }

            // ve slozenych zavorkach
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter("text.txt", append: true))
            {
                writer.WriteLine(text);
                writer.WriteLine("jak se mas");
            }

            if (true)
            {
                // do konce metody
                using System.IO.StreamWriter writer = new System.IO.StreamWriter("text.txt", append: true);

                writer.WriteLine(text);
                writer.WriteLine("jak se mas");
            }

            // jednoduche, otevre, zapise a zavre
            System.IO.File.AppendAllText("text2.txt", text + System.Environment.NewLine + "jak se mas");
            
            // slozitejsi, pracuje s polem bytu, nejvic moznosti (napriklad soubezny zapis do souboru z vice vlaken atd.), prepisovani casti souboru a dalsi.
            using (FileStream fileStream = new FileStream("text2.txt", FileMode.Append))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text + Environment.NewLine);
                fileStream.Write(bytes);

                ReadOnlySpan<byte> span = "jak se mas\n"u8;
                fileStream.Write(span); // novejsi zpusob C# 11

                fileStream.Write("konec"u8);
            }
            
        }
    }
}
