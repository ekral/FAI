# Přáce s databází pomocí ADO.NET

Následující příkaz používají provider *Microsoft.Data.Sqlite* pro souborovou databázi Sqlite, návod na instalaci najdete například v dokumemntaci Microsoftu [Microsoft.Data.Sqlite overview](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli).

## Vytvoření databáze

Dokumentaci vytvoříme pomocí následujícího príkazu. *Connection* implemetuje rozhraní *IDispose* a proto se nám připojení uzavře automaticky s využití Dispose patternu a nemusíme jej už ručně uzavírat.

```csharp
await using SqliteConnection connection = new SqliteConnection(connectionString);
await connection.OpenAsync();
```

## ExecuteReader

## ExecuteNonQuery

## ExecuteScalar
