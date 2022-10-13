using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace SharedProject
{
    public class DatabaseService
    {
        private const string path = "database.db";
        private readonly string connectionString;
        
        public DatabaseService()
        {
            SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder();
            builder.DataSource = path;

            connectionString = builder.ToString();
        }
        /// <summary>
        /// Verifies that the database exists. And if it does not exist, it creates a new database.
        /// </summary>
        public async Task<bool> EnsureCreatedAsync()
        {
            if (File.Exists(path)) return false;

            await using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            
            command.CommandText = 
            @"CREATE TABLE Mortgage 
            (
                Id INTEGER PRIMARY KEY,
                LoanAmount DOUBLE,
                InterestRate DOUBLE,
                LoanTerm INTEGER
            )";

            await command.ExecuteNonQueryAsync();

            return true;
        }

        public async Task<Model> GetByIdAsync(int id)
        {
            throw  new NotImplementedException();
        }

        public async Task<List<Model>> GetAll()
        {
            await using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            // vrati vsechny radky tabulky Mortgage
            command.CommandText = "SELECT Id, LoanAmount, InterestRate, LoanTerm FROM Mortgage";

            List<Model> models = new List<Model>();

            var reader = await command.ExecuteReaderAsync();
            
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    int id = reader.GetInt32(reader.GetOrdinal("Id"));
                    double loanAmount = reader.GetDouble(reader.GetOrdinal("LoanAmount"));
                    double interestRate = reader.GetDouble(reader.GetOrdinal("InterestRate"));
                    int loanTerm = reader.GetInt32(reader.GetOrdinal("LoanTerm"));

                    Model model = new Model(loanAmount, interestRate, loanTerm) { Id = id };
                    
                    models.Add(model);
                }
            }
            return models;
        }
        
        public async Task InsertAsync(Model model)
        {
            await using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
             INSERT INTO Mortgage
             (LoanAmount, InterestRate, LoanTerm)
             VALUES (@LoanAmount, @InterestRate, @LoanTerm)
            ";

            command.Parameters.Add("@LoanAmount", SqliteType.Real).Value = model.LoanAmount;
            command.Parameters.Add("@InterestRate", SqliteType.Real).Value = model.InterestRate;
            command.Parameters.Add("@LoanTerm", SqliteType.Integer).Value = model.LoanTerm;
            
            int count = command.ExecuteNonQuery();
        }

        public async Task UpdateAsync(Model model) 
        {
            throw new NotImplementedException();
        }

        public void DeleteAsync(Model model) 
        {
            throw new NotImplementedException();
        }
    }
}
