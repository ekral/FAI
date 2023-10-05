# Model pizzy a migrace

Podle [návodu v přípravě](https://github.com/ekral/FAI/blob/master/AF/Priprava/01_EF_zaklady.md) proveďte následující úkoly.

1) Nadefinujte třídu Pizza z projektu [Pizza.Kiosk](https://github.com/ekral/Utb.PizzaKiosk) a pomoci Entity Frameworku a migrací vytvořte Sqlite databázi. Do databáze vložte tři pizzy jako výchozí data při vytvoření databáze.

   - Definujte třídu Pizza.

   - Definujte potomka třídy DbContext a DbSet pro tabulku Pizza.

   - Definujte metodu OnConfiguring a nakonfigurujte Sqlite.

   - Definujte metodu OnCreating a přidejte výchozí pizzy.

   - Vytvořte a proveďte migraci (vytvořte databázi).

3) Vytvořte kód který vypíše všechny pizzy z databáze.

4) Vytvořte kód, který vypíše pouze pizzy levnější než 150.

5) Vytvořte kód, který vloží do databáze novou pizzu.
