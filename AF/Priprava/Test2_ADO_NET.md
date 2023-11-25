# Přáce s databází pomocí ADO.NET

Následující příklady používají provider *Microsoft.Data.Sqlite* pro souborovou databázi *Sqlite*, návod na instalaci najdete například v dokumentaci Microsoftu [Microsoft.Data.Sqlite overview](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli).

V následujícíh příkladech budeme používat třídu Model:

```csharp
public class Model
{
    public int Id { get; set; }
    public double LoanAmount { get; set; }
    public double InterestRate { get; set; }
    public int LoanTerm { get; set; }
}
```
## Připojení k databázi

K databázi se připojíme pomocí následujícího príkazu. Pokud Sqlite databáze již neexistuje, tak se vytvoří nová. Třída *SqliteConnection* implemetuje rozhraní *IDispose* a proto se nám připojení uzavře automaticky s využití Dispose patternu a nemusíme jej už ručně uzavírat.

```csharp
await using SqliteConnection connection = new SqliteConnection(connectionString);
await connection.OpenAsync();
```

Poté si můžeme vytvořit proměnnou command pro provádění SQL příkazů:

```csharp
SqliteCommand command = connection.CreateCommand();
```

## ExecuteNonQuery

Pro vytvoření databáze, vložení nového řádku, nebo aktualizaci hodnot sloupců, tedy příkazy, které neprovádějí dotaz a nevrací hodnoty používáme metodu ```command.ExecuteNonQueryAsync()```. 

V následujícím kódu vytvoříme databází:

```csharp
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

await command.ExecuteNonQueryAsync();
```

A v následujícím příkladu do vytvořené databáze vložíme nový řádek. Proměnná ```count``` bude obsahovat počet změněných řádků tabulky.

```csharp
Model model = new Model()
{
    LoanAmount = 6000000.0,
    InterestRate = 6.0,
    LoanTerm = 30
};

command.CommandText =
@$"
    INSERT INTO Mortgage (LoanAmount, InterestRate, LoanTerm)
    VALUES (@LoanAmount, @InterestRate, @LoanTerm)
";

command.Parameters.Add("@LoanAmount", SqliteType.Real).Value = model.LoanAmount;
command.Parameters.Add("@InterestRate", SqliteType.Real).Value = model.InterestRate;
command.Parameters.Add("@LoanTerm", SqliteType.Integer).Value = model.LoanTerm;

int count = await command.ExecuteNonQueryAsync();
```

V předchozím příkladu jsme použili parametry Commandu. Parametry můžeme zadávat buď s generickými SQL typy nebo s konkrétními typy pro danou databází s metodou, kterou poskytuje konkrétní provider. 

Použití parametrů proskytuje kontrolu typu a validaci zadané hodnoty parametru a pomáhá zabránit útoku technikou SQL Injection. Použití parametrů nezpomaluje provedení dotazu, spíše nám může pomoct dotaz lépe sestavit dle konkrétního typu a díky tomu by provedení dotazu by mohlo být v některých případech efektivnější.


## ExecuteReader

Pokud chceme provést dotaz na data tabulky a načíst jednotlivé řádky a sloupce, tak použijeme reader jak je uvedeno v následujícím kódu. 

```csharp
List<Model> models = new List<Model>();

SqliteDataReader reader = await command.ExecuteReaderAsync();

command.CommandText = "SELECT Id, LoanAmount, InterestRate, LoanTerm FROM Mortgage";

if(reader.HasRows)
{
    while(await reader.ReadAsync())
    {
        Model model = new Model();

        model.Id = reader.GetInt32(reader.GetOrdinal("Id"));
        model.LoanAmount = reader.GetDouble(reader.GetOrdinal("LoanAmount"));
        model.InterestRate = reader.GetDouble(reader.GetOrdinal("InterestRate"));
        model.LoanTerm = reader.GetInt32(reader.GetOrdinal("LoanTerm"));

        models.Add(model);
    }
}
```

## ExecuteScalar

Prokud provedeme SQL dotaz, který vrátí pouze jednu hodnotu, například ```SELECT AVG(LoanAmount) FROM Mortgage```, tak můžeme použít ```reader``` podobně jako v minulém příkladu a načíst první sloupec prvního řádku nebo můžeme použít metodu ```command.ExecuteScalarAsync()```.

```csharp
command.CommandText =
@$"
    SELECT AVG(LoanAmount) FROM Mortgage
";

object? result = await command.ExecuteScalarAsync();

if(result is double average)
{
    return average;
}
```
