# Přáce s databází pomocí ADO.NET

Následující příkaz používají provider *Microsoft.Data.Sqlite* pro souborovou databázi Sqlite, návod na instalaci najdete například v dokumentaci Microsoftu [Microsoft.Data.Sqlite overview](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli).

## Vytvoření databáze

Dokumentaci vytvoříme pomocí následujícího príkazu. *Connection* implemetuje rozhraní *IDispose* a proto se nám připojení uzavře automaticky s využití Dispose patternu a nemusíme jej už ručně uzavírat.

```csharp
await using SqliteConnection connection = new SqliteConnection(connectionString);
await connection.OpenAsync();
```

Poté si můžeme vytvořit proměnnou command pro provádění SQL příkazů:

```csharp
SqliteCommand command = connection.CreateCommand();
```

## ExecuteNonQuery

Pro vytvoření databáze, vložení nového řádku, nebo aktualizaci hodnot sloupců, tedy příkazy, které neprovádějí dotaz a nevrací hodnoty používáme metodu ```csharp command.ExecuteNonQueryAsync()```. 

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

A v následujícím příkladu vytvořené databáze vložíme nový řádek. Proměnná count bude obsahovat počet změněných řádků tabulky.

```csharp
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

V předchozím příkladu jsem použili parametry Commandu. Parametry můžeme zadávat buď s generickými SQL typy nebo s konkrétními typy pro danou databází s metodou, kterou poskytuje konkrétní provider. 

Použití parametrů proskytuje kontrolu typu a validaci zadané hodnoty parametru a pomáhá zabránit útoku technikou SQL Injection. Použití parametrů nemá výrazný vliv na rychlost dotazu.


## ExecuteReader

Pokud chceme provést dotaz na data tabulky a načíst jednotlivé řádky a sloupce, tak použijeme reader jak je uvedeno v následujícím kódu. 

```csharp
List<Model> models = new List<Model>();

SqliteDataReader reader = await command.ExecuteReaderAsync();

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
