using Microsoft.Data.Sqlite;

namespace ConsoleAppAdoNet
{
    internal class Program
    {
        public static void CreateTable()
        {
            using SqliteConnection connection = new SqliteConnection("DataSource=studenti.db");
            connection.Open();

            using SqliteCommand command = connection.CreateCommand();

            command.CommandText = @"
                CREATE TABLE Studenti
                (
                    StudentId INTEGER PRIMARY KEY,
                    Jmeno TEXT
                ) 
            ";

            // Kdyz prikaz nic nevraci tak pouziju ExecuteNonQuery
            command.ExecuteNonQuery();
        }

        public static void AddStudent(string jmeno)
        {
            using SqliteConnection connection = new SqliteConnection("DataSource=studenti.db");
            connection.Open();

            using SqliteCommand command = connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Studenti (Jmeno) VALUES (@Jmeno)
            ";
            
            command.Parameters.Add("@Jmeno", SqliteType.Text).Value = jmeno;

            // Kdyz prikaz nic nevraci tak pouziju ExecuteNonQuery
            command.ExecuteNonQuery();
        }

        public static void VypisStudenty()
        {
            using SqliteConnection connection = new SqliteConnection("DataSource=studenti.db");
            connection.Open();

            using SqliteCommand command = connection.CreateCommand();

            command.CommandText = @"
                SELECT StudentId, Jmeno
                FROM Studenti
            ";

            // Kdyz dotaz vraci hodnotu tak muzu pouzit reader
            var reader = command.ExecuteReader();

            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    int studentId = reader.GetInt16(0);
                    string jmeno = reader.GetString(1);

                    Console.WriteLine($"{studentId}: {jmeno}");
                }
            }
        }

        public static void MaxJmeno()
        {
            using SqliteConnection connection = new SqliteConnection("DataSource=studenti.db");
            connection.Open();

            using SqliteCommand command = connection.CreateCommand();

            command.CommandText = @"
                SELECT MAX(Jmeno)
                FROM Studenti
            ";

            // Kdyz dotaz vraci jen jednu hodnotu tak muzu pouzit ExecuteScalar
            object? result = command.ExecuteScalar();

            if (result is string jmeno)
            {
                Console.WriteLine(jmeno);
            }
        }

        public static void MaxJmeno2()
        {
            using SqliteConnection connection = new SqliteConnection("DataSource=studenti.db");
            connection.Open();

            using SqliteCommand command = connection.CreateCommand();

            command.CommandText = @"
                SELECT MAX(Jmeno)
                FROM Studenti
            ";

            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string jmeno = reader.GetString(0);

                    Console.WriteLine($"{jmeno}");
                }
            }

            connection.Close();
        }


        static void Main(string[] args)
        {
            //CreateTable();
            //AddStudent("Jiri");
            //AddStudent("Pavel");
            //AddStudent("Alena");

            VypisStudenty();

            MaxJmeno();
        }
    }
}
