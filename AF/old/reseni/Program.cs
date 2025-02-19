using Microsoft.Data.Sqlite;
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;

namespace MujPatyProjekt
{
    internal class Program
    {
        static string GetConnectionString()
        {
            return "Data Source=objednavky.db";
        }

        static async Task VytvorDatabaziAsync()
        {
            await using (SqliteConnection connection = new SqliteConnection(GetConnectionString()))
            {
                await connection.OpenAsync();

                SqliteTransaction transaction = connection.BeginTransaction();

                await using (SqliteCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;

                    try
                    {
                        command.CommandText = @"
                        CREATE TABLE Zakaznik
                        (
                            ZakaznikId INTEGER PRIMARY KEY,
                            Jmeno TEXT
                        )
                    ";

                        int count = await command.ExecuteNonQueryAsync();

                        command.CommandText = @"
                        CREATE TABLE Objednavka
                        (
                            ObjednavkaId INTEGER PRIMARY KEY,
                            ZakaznikId INTEGER,
                            Cena REAL,
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
        static async Task PridejObjednavku(double cena, int id)
        {
            await using (SqliteConnection connection = new SqliteConnection(GetConnectionString()))
            {
                await connection.OpenAsync();

                await using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                        INSERT INTO Objednavka (ZakaznikId, Cena) 
                        VALUES (@Id, @Cena) 
                        RETURNING ObjednavkaId
                    ";

                    command.Parameters.Add("@Cena", SqliteType.Real).Value = cena;
                    command.Parameters.Add("@Id", SqliteType.Integer).Value = id;

                    object? result = await command.ExecuteScalarAsync();

                    if (result is Int64 majitelId)
                    {
                        Console.WriteLine($"ObjednavkaId: {majitelId}");
                    }
                }
            }
        }

        static async Task VypisDatabazi()
        {
            await using (SqliteConnection connection = new SqliteConnection(GetConnectionString()))
            {
                await connection.OpenAsync();

                await using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                        SELECT Objednavka.ObjednavkaId, Zakaznik.Jmeno, Objednavka.Cena
                        FROM Objednavka
                        INNER JOIN Zakaznik ON Objednavka.ZakaznikId=Zakaznik.ZakaznikId
                    ";

                    SqliteDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            int objednavkaId = reader.GetInt32(0);
                            string jmeno = reader.GetString(1);
                            double cena = reader.GetDouble(2);

                            Console.WriteLine($"{objednavkaId} {jmeno} {cena}");
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
                Console.WriteLine("Menu:");
                Console.WriteLine("1 - Vytvor databazi");
                Console.WriteLine("2 - Vloz noveho zakaznika");
                Console.WriteLine("3 - Vloz novou objednavku");
                Console.WriteLine("4 - Vypis objednavky vcetne jmena zakaznika");
                Console.WriteLine("k - konec");

                string? volba = Console.ReadLine();
                
                switch (volba)
                {
                    case "1":
                        await VytvorDatabaziAsync();
                        break;
                    case "2":
                        Console.WriteLine("Zadej jmeno");
                        string? jmeno = Console.ReadLine();

                        if (jmeno is not null)
                        {
                            await PridejZakaznika(jmeno);
                        }
                        break;
                    case "3":
                        Console.WriteLine("Zadej cenu");
                        double cena = double.Parse(Console.ReadLine());
                        Console.WriteLine("Zadej id zakaznika");
                        int id = int.Parse(Console.ReadLine());

                        await PridejObjednavku(cena,id);
                        
                        break;
                    case "4":
                        await VypisDatabazi();

                        break;
                    case "k":
                        konec = true;
                        break;
                    default:
                        Console.WriteLine("Neplatna volba");
                        break;
                }
            }
            while (!konec);
        }
    }
}
