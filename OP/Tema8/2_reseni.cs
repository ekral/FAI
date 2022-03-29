// Ukol 2: S pomocí Dictionary vytvořte a na konzoli zobrazte histogram vyskytu slov v libovolnem textovem souboru

using Spectre.Console;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        string line = "Ahoj Ahoj Ahoj jak se jak se mas; mas,mas";
        string[] slovaArray = line.Split(new char[] {',',' ', '.', ';' });

        Dictionary<string, int> slova = new Dictionary<string, int>();

        foreach (string slovo in slovaArray)
        {
            if (!string.IsNullOrWhiteSpace(slovo))
            {
                if (slova.ContainsKey(slovo))
                {
                    slova[slovo]++;
                }
                else
                {
                    slova[slovo] = 1;
                }
            }
        }

        Color[] palette = new Color[] { Color.Blue, Color.Blue1, Color.Blue3 };
        int index = 0;

        AnsiConsole.Write(new BarChart()
            .Width(60)
            .Label("[green bold underline]Vyskyt slov[/]")
            .CenterLabel()
            .AddItems(slova, (item) => new BarChartItem(
                item.Key, item.Value, palette[index = ++index % palette.Count()])));
    }
}
