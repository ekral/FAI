using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using System.Runtime.CompilerServices;

namespace ConsoleAppAssesmentEvaluation
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ConfigurationBuilder builder = new();
            // User secret je v csproj souboru
            builder.AddUserSecrets("41fcc22a-46d5-48d1-bc58-a697b564c327");
            IConfigurationRoot configuration = builder.Build();
            string key = configuration["ApiKey"] ?? throw new InvalidOperationException();

            string systemPrompt = """ 
                Jsi učitel programování a tvým úkolem je ohodnotit kód studenta v jazyce C# na základě zadaných rubrik a jestli kód odpovídá zadání úkolu.
                
                Rubriky:
                - správnost řešení (A až F),
                - čitelnost kódu (A až F),
                - efektivitu kódu (A až F)

                Vstup:
                - Dostaneš jednotlivé soubory .NET projektu (soubory .cs a .csproj) jako textové části user promptu.
                Výstup:
                - Vrať textové hodnocení obsahující zdůvodnění v jednotlivých rubrikách a celkové hodnocení (A až F) jak průměr jednotlivých rubrik.
                """;

            // prvni a posledni radek musi zacinat stejne.
            string assignementDescription = """
                Zadání úkolu:
                - Cílem úkolu je výpočet BMI (Body Mass Index) na základě zadané výšky a váhy uživatele. 
                - Dále program program zobrazí true/false hodnotu, která indikuje:
                    - zda má uživatel zhubnout, 
                    - zda má zdravou váhu a 
                    - zda má nezdravou váhu.
                - Program má být interaktivní a zobrazuje výsledky hned po zadání vstupních hodnot.
                - Program by měl mít ošetřené vstupy buď v uživatelském rozhraní (html atributy) nebo v kódu tak, aby nedošlo k chybě při zadání nečíselných hodnot nebo například aby nedošlo k dělení nulou. Stačí pouze jedno z nich.
                - Program je vytvořen jako Blazor Webassembly aplikace.
                """;

            string path = @"C:\Users\erik\source\repos\FAI\PA\seminar\projects\BlazorAppBmi";

            var client = new OpenAIClient(key);
            var chatClient = client.GetChatClient("gpt-5");
            
            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where(f => !f.Contains("\\obj\\") && !f.Contains("\\bin\\") && (f.EndsWith(".cs") || f.EndsWith(".csproj") || f.EndsWith(".razor")));
            
            var parts = new List<ChatMessageContentPart>();

            foreach (var file in files)
            {
                string fileName = Path.GetFileName(file);
                string content = File.ReadAllText(file);
                string relativePath = Path.GetRelativePath(path, file);

                var part = ChatMessageContentPart.CreateTextPart($"""
                    Tento soubor je součást řešení úkolu s názvem "{fileName}" a relativí cestou "{relativePath}" a jeho obsah je:
                    ----------------- ZAČÁTEK SOUBORU {fileName} -----------------
                    {content}
                    ----------------- KONEC SOUBORU {fileName} -----------------
                    """);

                parts.Add(part);
            }

            var userChatMessage = new UserChatMessage(parts);

            var response = await chatClient.CompleteChatAsync([new SystemChatMessage([systemPrompt, assignementDescription]), userChatMessage]);

            Console.WriteLine(response.Value.Content.First().Text);

            //await foreach (var message in chatClient.CompleteChatStreamingAsync([new SystemChatMessage([systemPrompt, assignementDescription]), userChatMessage]))
            //{
            //    Console.Write(message.ContentUpdate.First().Text);
            //}
        }
    }
}
