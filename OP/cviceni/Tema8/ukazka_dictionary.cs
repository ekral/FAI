// Ukol 1: S pomocí Dictionary vytvořte a na konzoli zobrazte histogram vyskytu slov v libovolnem textovem souboru

using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        string line = "Ahoj Ahoj Ahoj jak se jak se mas; mas,mas dnes dnes je hezky";
        string[] slovaArray = line.Split(new char[] { ',', ' ', '.', ';' });

        // nebo muzu prochazet znaky postupne
        Dictionary<string, int> slovnik = new Dictionary<string, int>();
        if (!slovnik.ContainsKey("ahoj"))
        {
            slovnik["ahoj"] = 1;
            slovnik["ahoj"]++;
        }

        if(slovnik.TryGetValue("cau", out int hodnota))
        {

        }

        System.Console.WriteLine(slovnik["ahoj"]);
    }
}
