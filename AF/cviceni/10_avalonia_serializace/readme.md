# Úkol 10: Avalonia serializace studentů

**autor: Erik Král ekral@utb.cz**

---

## 1) File Save Dialog

- Stáhněte si [výchozí solution](https://download-directory.github.io/?url=https%3A%2F%2Fgithub.com%2Fekral%2FFAI%2Ftree%2Fmaster%2FAF%2Fcviceni%2F10_avalonia_serializace%2Fsrc1).
- Nastavte menu Projekt -> Konfigurovat projekty po spuštění -> Více počátečních projektů v pořadí:
  1) Studenti.WebApi Spustit.
  2) Studenti.WebClient Spustit.
  3) Studenti.AvaloniaClient.Desktop Spustit
- Implementujte serializaci studentů do formátu JSON a jeho ukládání do souboru pomocí File Save dialogu.

## 2) Unit testy View Modelu

- Stáhněte si [výchozí solution](https://download-directory.github.io/?url=https%3A%2F%2Fgithub.com%2Fekral%2FFAI%2Ftree%2Fmaster%2FAF%2Fcviceni%2F10_avalonia_serializace%2Fsrc2).
- Nastavte menu Projekt -> Konfigurovat projekty po spuštění -> Více počátečních projektů v pořadí:
  1) Studenti.WebApi Spustit.
  2) Studenti.WebClient Spustit.
  3) Studenti.AvaloniaClient.Desktop Spustit
- Přidejte do solutionu projekt typu `xUnit` a přidejte v něm referenci na projekt `Studenti.AvaloniaClient`.
- Implementujte Unit testy pro ViewModel.