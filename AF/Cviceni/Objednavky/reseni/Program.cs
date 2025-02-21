using Microsoft.Data.Sqlite;
using System.Runtime.InteropServices;

namespace MojeDruhaAplikace
{
    internal class Program
    {
        static string GetConnectionString()
        {
            return "DataSource=databaze.db";
        }

        static async Task VytvorDatabaziAsync()
        {
            using (SqliteConnection connection = new SqliteConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                SqliteTransaction transaction = connection.BeginTransaction();

                using (SqliteCommand command = connection.CreateCommand())
                {
                    try
                    {
                        command.Transaction = transaction;

                        command.CommandText = @"
                        CREATE TABLE Zakaznik(
                            ZakaznikId INTEGER PRIMARY KEY,
                            Jmeno TEXT
                        )
                        ";

                        await command.ExecuteNonQueryAsync();

                        command.CommandText = @"
                        CREATE TABLE Objednavka(
                            ObjednavkaId INTEGER PRIMARY KEY,
                            Cena REAL,
                            ZakaznikId INTEGER,
                            FOREIGN KEY (ZakaznikId) REFERENCES Zakaznik(ZakaznikId)
                        )
                        ";

                        await command.ExecuteNonQueryAsync();

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);

                        await transaction.RollbackAsync();
                    }
                }
            }
        }

        static async Task PridejZakaznika(string jmeno)
        {
            await using (SqliteConnection connection = new SqliteConnection(GetConnectionString()))
            {
                await connection.OpenAsync();


                await using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                INSERT INTO Zakaznik (Jmeno) 
                VALUES (@Jmeno) 
                RETURNING ZakaznikId
            ";

                    command.Parameters.Add("@Jmeno", SqliteType.Integer).Value = jmeno;

                    object? result = await command.ExecuteScalarAsync();

                    if (result is Int64 majitelId)
                    {
                        Console.WriteLine($"ZakaznikId: {majitelId}");
                    }

                }
            }
        }

        static async Task PridejObjednavkuAsync(double cena, int zakaznikId)
        {
            await using (SqliteConnection connection = new SqliteConnection(GetConnectionString()))
            {
                await connection.OpenAsync();

                await using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                        INSERT INTO Objednavka (Cena, ZakaznikId) 
                        VALUES (@Cena, @ZakaznikId) 
                        RETURNING ObjednavkaId
                        ";

                    command.Parameters.Add("@Cena", SqliteType.Real).Value = cena;
                    command.Parameters.Add("@ZakaznikId", SqliteType.Integer).Value = zakaznikId;

                    object? result = await command.ExecuteScalarAsync();

                    if (result is Int64 objednavkaId)
                    {
                        Console.WriteLine($"ObjednavkaId: {objednavkaId}");
                    }
                }
            }
        }

        public static async Task VypisObjednavkyAsync()
        {
            await using (SqliteConnection connection = new SqliteConnection(GetConnectionString()))
            {
                await connection.OpenAsync();

                await using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                        SELECT Objednavka.Cena, Zakaznik.Jmeno
                        FROM Objednavka
                        INNER JOIN Zakaznik ON Objednavka.ZakaznikId=Zakaznik.ZakaznikId
                    ";
                    var reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var cena = reader.GetInt32(0);
                            var jmeno = reader.GetString(1);

                            Console.WriteLine($"Objednavka | Cena: {cena} | Zakaznik: {jmeno}");
                        }
                    }
                }
            }

        }
        static async Task Main(string[] args)
        {
            bool konec = false;
            do
            {
                Console.WriteLine("1 Vytvořit databazi");
                Console.WriteLine("2 Přidej nového zakaznika");
                Console.WriteLine("3 Přidej novou objednávku");
                Console.WriteLine("4 Vypis objednavky vcetne jmena zakaznika.");
                Console.WriteLine("k konec");

                string? volba = Console.ReadLine();


                switch (volba)
                {
                    case "1":
                        await VytvorDatabaziAsync();
                        break;
                    case "2":
                        Console.WriteLine("Zadej jmeno");
                        string? jmeno = Console.ReadLine();
                        if (jmeno != null)
                        {
                            await PridejZakaznika(jmeno);
                        }
                        break;
                    case "3":
                        Console.WriteLine("Zadej cenu:");

                        double cena;
                            
                        while(!double.TryParse(Console.ReadLine(), out cena))
                        {
                            Console.WriteLine("Neplatny format ceny");
                        }

                        Console.WriteLine("Zadej id Zakaznika:");

                        int zakaznikId;
                        
                        while(!int.TryParse(Console.ReadLine(), out zakaznikId))
                        {
                            Console.WriteLine("Neplatny format id");
                        }

                        await PridejObjednavkuAsync(cena, zakaznikId);

                        break;
                    case "4":
                        await VypisObjednavkyAsync();
                        break;
                    case "k":
                        konec = true;
                        break;
                    default:
                        Console.WriteLine("Neplatna volba");
                        break;
                }

            } while (!konec);
        }
    }
}
    
