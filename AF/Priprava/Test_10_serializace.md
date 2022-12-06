Dokumentace: [Serialization in .NET](https://learn.microsoft.com/en-us/dotnet/standard/serialization/)

## Serialization

Objekty v paměti nejsou kompatibilní mezi hardwarovými platformami (AMR, x86, x64) a už vůbec mezi různými programovacími jazyky. Serializace je proces převodu objektu v paměti na jiný formát kompatibilní mezi různými platformami. Například text ve formátu JSON, který můžeme uložit do souboru nebo přenést po síti. 

Máme několik možností:

- člověkem čitelný textový formát, nejběžnější je JSON používaný pro REST webové služby založené na HTTP protokolu. Dříve se používal XML formát pro SOAP služby.
- binární formát pro vysoký výkon. Příkladem je protokol gRPC.

Dotnet obsahuje mimo jiné zabudovanou podporu pro serializaci a deserializaci do formátu JSON.

```powershell
dotnet add package Microsoft.Extensions.DependencyInjection
```
