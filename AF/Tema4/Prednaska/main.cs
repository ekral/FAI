using Microsoft.Data.Sqlite;

Database database = new Database("mydatabase.db");

if (database.EnsureCreated())
{
    database.Insert(new Model { LoanAmount = 8000000.0, InterestRate = 6.0, LoanTerm = 30 });
    database.Insert(new Model { LoanAmount = 800000.0, InterestRate = 7.0, LoanTerm = 20 });
    database.Insert(new Model { LoanAmount = 4000000.0, InterestRate = 6.2, LoanTerm = 25 });
    database.Insert(new Model { LoanAmount = 10000000.0, InterestRate = 6.0, LoanTerm = 5 });
}

double avg = database.AverageLoanAmount();
Console.WriteLine($"Loan Amount Average: {avg}");

List<Model> models = database.GetAll();
foreach (Model model in models)
{
    Console.WriteLine($"{model.LoanAmount, 18:C1} {model.InterestRate, 4:F1} {model.LoanTerm, 3}");
}

class Database
{
    private readonly string path;
    private readonly string connectionString;

    public Database(string path)
    {
        this.path = path;
        SqliteConnectionStringBuilder stringBuilder = new SqliteConnectionStringBuilder()
        {
            DataSource = path
        };

        connectionString = stringBuilder.ToString();
    }

    public double AverageLoanAmount()
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
        @"
        SELECT Avg(LoanAmount) 
        FROM Mortgage
        ";

        object? avg = command.ExecuteScalar();
        return avg is null ? 0 : (double)avg;
    }

    public void Insert(Model model)
    {
        using var connection = new SqliteConnection(connectionString);
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
    public bool EnsureCreated()
    {
        if (File.Exists(path)) return false;

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
        @"
        CREATE TABLE Mortgage 
        (
            Id INTEGER PRIMARY KEY,
            LoanAmount DOUBLE,
            InterestRate DOUBLE,
            LoanTerm INTEGER
        )
        ";

        command.ExecuteNonQuery();

        return true;
    }

    public List<Model> GetAll()
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, LoanAmount, InterestRate, LoanTerm FROM Mortgage";
        
        var reader = command.ExecuteReader();

        List<Model> models = new List<Model>();

        if(reader.HasRows)
        {
            while(reader.Read())
            {
                int id = reader.GetInt32(reader.GetOrdinal("Id"));
                double loanAmmount = reader.GetDouble(reader.GetOrdinal("LoanAmount"));
                double interestRate = reader.GetDouble(reader.GetOrdinal("InterestRate"));
                int loanTerm = reader.GetInt32(reader.GetOrdinal("LoanTerm"));

                Model model = new Model()
                {
                    Id = id,
                    LoanAmount = loanAmmount,
                    InterestRate = interestRate,
                    LoanTerm = loanTerm
                };

                models.Add(model);
                
            }
        }

        return models;
    }
}

class Model
{
    public int Id { get; set; }
    public double LoanAmount { get; set; }
    public double InterestRate { get; set; }
    public int LoanTerm { get; set; }
}
