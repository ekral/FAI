using Microsoft.Data.Sqlite;

namespace ConsoleAppAdoNet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            await using SqliteConnection connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();
             connection.
            Console.WriteLine("Hello, World!");
        }
    }
}
