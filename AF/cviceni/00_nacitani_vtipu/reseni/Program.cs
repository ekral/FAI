namespace MojePrvniAplikace
{
    internal class Program
    {
        // Zadavani hodnoceni jako metodu
        static int ZadejHodnoceni()
        {
            int hodnoceni;
        
            while(!int.TryParse(Console.ReadLine(), out hodnoceni) || hodnoceni < 1 || hodnoceni > 5)
            {
                Console.WriteLine("Zadej cislo 1 az 5");
            }
        
            return hodnoceni;
        }

        static async Task Main(string[] args)
        {
            using HttpClient client = new HttpClient();
            string url = "https://v2.jokeapi.dev/joke/Programming?format=txt";
            string fileName = "vtipy.txt";

            bool konec = false;

            while(!konec)
            { 
                Console.WriteLine("1. Stáhnout a uložit vtip");
                Console.WriteLine("2. Načít vtipy dle hodoceni");
                Console.WriteLine("3. konec");

                string? vyber = Console.ReadLine();
              
                switch (vyber)
                {
                    case "1":
                        {
                            string joke = await client.GetStringAsync(url);
                            string jedenRadek = joke.Replace('\n', ' ');
                            Console.WriteLine(jedenRadek);

                            int hodnoceni = ZadejHodnoceni();

                            if (hodnoceni > 0)
                            {
                                // ulozit do souboru

                                File.AppendAllText(fileName, hodnoceni.ToString());
                                File.AppendAllText(fileName, Environment.NewLine);
                                File.AppendAllText(fileName, jedenRadek);
                                File.AppendAllText(fileName, Environment.NewLine);

                            }
                        }

                        break;
                    case "2":
                        {
                            // nacteni radku ze souboru
                            int hodnoceni = ZadejHodnoceni();

                            string[] radky = File.ReadAllLines(fileName);

                            for (int i = 0; i < radky.Length; i += 2)
                            {
                                string hodnoceniString = radky[i];
                                string vtip = radky[i + 1];

                                if(hodnoceni.ToString() == hodnoceniString)
                                {
                                    Console.WriteLine(vtip);
                                }
                            }

                        }
                        break;
                    case "3":
                        konec = true;
                        break;
                }
            }
        }
    }
}
