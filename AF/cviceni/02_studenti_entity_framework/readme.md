# 02 Databáze studentů v Entity Frameworku

**autor: Erik Král ekral@utb.cz**

---

## Databáze studentů

Vytvořte databázi studentů v Entity Frameworku a konzolovou aplikaci, kde

Student bude mít sloupce:
- StudentId
- Jmeno
- Studuje

Vytvořte metody pro:

- Seed databáze - vytvoří databází a vloží výchozí studenty.
- AddStudent - vloží nového studenta do databáze a vypíše jeho přiřazený primární klíč.
- GetAllStudents - vrátí všechny studenty.
- GetActiveStudents - vrátí studenty, kteří mají Studuje true.
- GetStudentById - vrátí studenta podle id.
- UpdateStudent - změní studenta dle id v databází.
- DeleteStudent - odstraní studenta dle id.

Vytvořte konzolovou aplikaci umožňující spustit vytvořené metody.

## Samostatný úkol Produkty

Vytvořte databází produktů kde Produkt bude mít:
- ProduktId
- Nazev
- Cenu

Vytvořte CRUD (Create, Read, Update a Delete) operace. Načtěte a vypište průměrnou cenu produktů v databází. 
