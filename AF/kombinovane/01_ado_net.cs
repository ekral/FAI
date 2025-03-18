using Microsoft.Data.Sqlite;

namespace ConsoleAppPrvniPrednaska
{
    internal class Program
    {
        public static async Task CreateTable()
        {
            await using SqliteConnection connection = new SqliteConnection("DataSource=studenti.db");

            await connection.OpenAsync();

            await using SqliteCommand command = connection.CreateCommand();

            command.CommandText =
            @"
                CREATE TABLE Studenti
                (
                    StudentId INTEGER PRIMARY KEY,
                    Jmeno TEXT,
                    Kredity INTEGER
                ) 
            ";

            await command.ExecuteNonQueryAsync();
        }

        public static async Task InsertStudent(string jmeno, int kredity)
        {
            await using SqliteConnection connection = new SqliteConnection("DataSource=studenti.db");

            await connection.OpenAsync();

            await using SqliteCommand command = connection.CreateCommand();

            command.Parameters.Add("@Name", SqliteType.Text).Value = jmeno;
            command.Parameters.Add("@Kredity", SqliteType.Integer).Value = kredity;

            command.CommandText =
            @$"
                INSERT INTO Studenti (Jmeno, Kredity) VALUES (@Name, @Kredity) Returning StudentId
            ";

            if(await command.ExecuteScalarAsync() is long studentId)
            {
                Console.WriteLine($"studentId: {studentId}");
            }
        }


        public static async Task VypisStudenty()
        {
            await using SqliteConnection connection = new SqliteConnection("DataSource=studenti.db");

            await connection.OpenAsync();

            await using SqliteCommand command = connection.CreateCommand();

            command.CommandText =
            @$"
                SELECT StudentId, Jmeno, Kredity FROM Studenti
            ";

            await using SqliteDataReader reader = await command.ExecuteReaderAsync();

            if(reader.HasRows)
            {
                while(await reader.ReadAsync())
                {
                    int studentId = reader.GetInt32(0);
                    string jmeno = reader.GetString(1);
                    int kredity = reader.GetInt32(2);

                    Console.WriteLine($"StudentId: {studentId} jmeno: {jmeno} kredity: {kredity}");
                }
            } 
        }

        public static async Task VypisPrumernyKredit()
        {
            await using SqliteConnection connection = new SqliteConnection("DataSource=studenti.db");

            await connection.OpenAsync();

            await using SqliteCommand command = connection.CreateCommand();

            command.CommandText =
            @$"
                SELECT AVG(Kredity) FROM Studenti
            ";

            object? prumerObjekt = await command.ExecuteScalarAsync();

            if(prumerObjekt is double prumer)
            {
                Console.WriteLine($"Prumer: {prumerObjekt}");
            }
        }
        static async Task Main(string[] args)
        {
            await CreateTable();

            await InsertStudent("Matylda", 12);
            await InsertStudent("Jiri", 15);
            await InsertStudent("Karel", 16);

            await VypisStudenty();
            await VypisPrumernyKredit();
            

        }
    }
}
