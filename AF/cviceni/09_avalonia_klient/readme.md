# Úkol 09: Avalonia klient

**autor: Erik Král ekral@utb.cz**

---

- Nainstalujte si [Avalonia Framework](https://docs.avaloniaui.net/docs/get-started/install) a nastavte [vaše vývojové prostředí](https://docs.avaloniaui.net/docs/get-started/set-up-an-editor). Poznámka: Pro Visual Studio stačí nainstalovat extension [Avalonia for Visual Studio 2022](https://marketplace.visualstudio.com/items?itemName=AvaloniaTeam.AvaloniaVS).
- Stáhněte si [výchozí solution](https://download-directory.github.io/?url=https%3A%2F%2Fgithub.com%2Fekral%2FFAI%2Ftree%2Fmaster%2FAF%2Fcviceni%2F09_avalonia_klient%2Fsrc).
- Do solution přidejte nový projekt Avalonia C# Project (Desktopová aplikace s frameworkem MVVM Toolkit).
- Nastavte menu Projekt -> Konfigurovat projekty po spuštění -> Více počátečních projektů v pořadí:
  1) Studenti.WebApi Spustit.
  2) Studenti.WebClient Spustit.
  3) Studenti.AvaloniaClient.Desktop Spustit
- Implementujte desktopového klienta pro editaci jednoho studenta.
