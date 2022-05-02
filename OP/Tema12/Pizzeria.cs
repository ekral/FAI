// https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli
// Microsoft.Data.Sqlite

// Dotykovy objednavkovy system pro pizzerii
// Objednat pizzu a dalsi produkty

// 1. Trida Produkt (pizza, pizza tycinky atd.) plus pridavky

// trida Kosik
// tridy Objednavka a Polozka objednavky
// pokud se zmeni cena Produktu tak se nezmeni cena v objednavce

// Nevytvarejte UI, ale jen si vytvorte testy
// Zvolte minimalni funkcionalitu

using Microsoft.Data.Sqlite;

namespace PizzeriaKonzole
{
    class Database
    {
        public void CreateDatabase()
        {
            using (var connection = new SqliteConnection("Data Source=mojedb.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"CREATE TABLE employees
                                      (
                                            Id INTEGER PRIMARY KEY,
                                            Cena DECIMAL,
                                            Pocet VARCHAR
                                      )";
                command.ExecuteNonQuery();
            }
        }
    }

    public enum Kategorie
    {
        Jidlo,
        Pridavek
    }

    public class Produkt
    {
        public int Kod { get; set; }
        public string Nazev { get; set; }
        public decimal Cena { get; set; }
        public Kategorie Kategorie { get; set; }

        public Produkt(int kod, string nazev, decimal cena, Kategorie kategorie)
        {
            Kod = kod;
            Nazev = nazev;
            Cena = cena;
            Kategorie = kategorie;
        }
    }

    public class Pridavek
    {
        private int pocet;

        public Produkt Produkt { get; }

        /// <summary>
        /// Počet přídavku. Musí být kladné číslo. 
        /// </summary>
        public int Pocet 
        { 
            get => pocet;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException($"Pocet pridavku musi byt kladny, pro odebrani pouzij metodu PolozkaKosiku.OdeberPridavek");

                pocet = value;
            }
        }

        public Pridavek(Produkt produkt, int pocet)
        {
            Produkt = produkt;
            Pocet = pocet;
        }
    }

    public class PolozkaKosiku
    {
        public Produkt Produkt { get; set; }

        private List<Pridavek> pridavky = new List<Pridavek>();
        public IReadOnlyCollection<Pridavek> Pridavky => pridavky;


        public PolozkaKosiku(Produkt produkt)
        {
            Produkt = produkt;
        }

        public void PridejPridavek(Pridavek pridavek)
        {
            pridavky.Add(pridavek);
        }

        public void OdeberPridavek(Pridavek pridavek)
        {
            pridavky.Remove(pridavek);
        }

    }

    public class Kosik
    {
        private List<PolozkaKosiku> polozky = new List<PolozkaKosiku>();
        public IReadOnlyCollection<PolozkaKosiku> Polozky => polozky;

        public void PridejPolozku(PolozkaKosiku polozka)
        {
            polozky.Add(polozka);
        }
        
        public void OdeberPolozku(PolozkaKosiku polozka)
        {
            polozky.Remove(polozka);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Produkt pizza = new Produkt(1, "MARGHERITA", 132.0m, Kategorie.Jidlo);
            Produkt ananas = new Produkt(2, "ananas 120g", 25.0m, Kategorie.Pridavek);
            Produkt ancovicky = new Produkt(2, "ančovičky 40g", 55.0m, Kategorie.Pridavek);

            PolozkaKosiku polozkaKosiku = new PolozkaKosiku(pizza);

            Pridavek pridavekAnanas = new Pridavek(ananas, 2);
            polozkaKosiku.PridejPridavek(pridavekAnanas);

            Pridavek pridavekAncovicky = new Pridavek(ancovicky, 1);
            polozkaKosiku.PridejPridavek(pridavekAncovicky);

            foreach (Pridavek pridavek in polozkaKosiku.Pridavky)
            {
                Console.WriteLine($"{pridavek.Produkt.Nazev} cena za kus: {pridavek.Produkt.Cena} pocet: {pridavek.Pocet}");
            }

            pridavekAnanas.Pocet = 1;

            polozkaKosiku.OdeberPridavek(pridavekAncovicky);

            Kosik kosik = new Kosik();
            kosik.PridejPolozku(polozkaKosiku);

            // TODO Odeslani objednavky
            // Kopie produktu identifikace podle Id
            // Info o platbe
            // Ulozeni do DB

            
        }
    }
}
