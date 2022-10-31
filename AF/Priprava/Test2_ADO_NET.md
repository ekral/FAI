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

A v následujícím příkladu vytvořené databáze vložíme nový řádek:
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

## ExecuteReader



## ExecuteScalar
