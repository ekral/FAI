# Úkol 1 – Aspire Host

V tomto cvičení se naučíme, jak vytvořit aplikaci řízenou technologií **Aspire**.

Nejprve si stáhneme image **SQL Serveru** a spustíme jej v prostředí **Docker Desktop**. Následně vytvoříme databázi a pomocí **Service Discovery** získáme connection string.

Součástí projektu bude:

- **Aspire Application Host**, který spustí databázový server i aplikační projekty a umožní spustit HTTP command `reset-db`.
- **Minimal Web API projekt**, který bude sloužit k resetu databáze (databázi smaže, znovu vytvoří a naplní daty – vytvoří tabulku **Kniha** a vloží do ní záznamy). Projekt obsahuje také `POST` endpoint `reset-db` pro reset databáze.
- **Konzolová aplikace**, pomocí které si na konzoli zobrazíme obsah tabulky **Kniha** uložené v databázi.

## 📋 Postup

1. Vytvořte nový projekt `Aspire Empty App` a pro vytvořený projekt:
    - Zaktualizujte případné zastaralé nuget balíčky.
    - Přidejte muget balíček `Aspire.Hosting.SqlServer` viz [návod pro SQL Server](https://aspire.dev/integrations/databases/efcore/sql-server).
    - Přidejte do kódu 




