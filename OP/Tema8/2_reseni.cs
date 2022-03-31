// Ukol 2: S pomocí Dictionary vytvořte a na konzoli zobrazte histogram vyskytu slov v libovolnem textovem souboru

using Spectre.Console;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Domaci ukol: nacitat po jednom znaku

        Dictionary<string, int> slovnik = new Dictionary<string, int>();
        
        using StreamReader reader = new StreamReader("text.txt");
        
        string line = null;

        while ((line = reader.ReadLine()) != null)
        {
            string[] slova = line.Split(new char[] { ',', ' ', '.', ';', '"', '[', ']', '„', '”', '?', '!' });

            foreach (string slovo in slova)
            {
                if (!string.IsNullOrWhiteSpace(slovo))
                {
                    if (slovnik.ContainsKey(slovo))
                    {
                        slovnik[slovo]++;
                    }
                    else
                    {
                        slovnik[slovo] = 1;
                    }
                }
            }
        }

        //foreach (var pair in slovnik.OrderBy(p => p.Value))
        //{
        //    System.Console.WriteLine($"{pair.Key}\t\t {pair.Value}");
        //}

        Color[] palette = new Color[] { Color.Blue, Color.Blue1, Color.Blue3 };
        int index = 0;

        AnsiConsole.Write(new BarChart()
           .Width(60)
           .Label("[green bold underline]Vyskyt slov[/]")
           .CenterLabel()
           .AddItems(slovnik.OrderByDescending(p => p.Value), (item) => new BarChartItem(
               item.Key, item.Value, palette[index = ++index % palette.Length])));;
    }
}
