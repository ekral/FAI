# Objednávací systém v menze

Semestrální projekt do předmětu **Aplikační frameworky**.  

Cílem projektu je návrh a implementace objednávacího systému pro menzu
s využitím nástrojů a frameworků **.NET Aspire, Minimal WebAPI, Entity Framework a Blazor**.

Objednávací systém pro menzu umožní objednávání minutek (jídel připravovaných na objednávku), kdy student si objedná jídlo v menze ve webové aplikaci běžící na dotykovém panelu a kuchařky jej začnou připravovat a budou měnit stav objednávky ve webové aplikaci běžící na dotykovém panelu. Student bude o stavu objednané minutky informován ve webové aplikaci.

Případy použití:

Vedení menzy:
    - Jídla
        - Zobrazuje seznam jídel (popis a cena jídla).
        - Vytváří nové jídlo.
        - Upravuje jídla. Jídlo se neodstraňuje, jen se označí že není aktivní.
    - Menu
        - Zobrazuje všechny položky menu (datum, jídlo, počet dostupných porcí) pro všechny dny.
        - Vytváří novou položku menu.
        - Upravuje položky menu.
        - Odstraňuje položky menu.
Kuchařka v menze
    - Objednávky
        - Zobrazí seznam objednávek které nejsou dokončené.
        - Označí, že je objednávka s daným číslem hotová, zrušená nebo dokončená (vydaná studentovi nebo byl student informován o zrušení).
Student
    - Objednávky
        - Student si zobrazí menu pro aktuální den.
        - Student si objedná si jídlo z aktuálního menu. 
        
---

## 👥 Tým

| Jméno | Role | GitHub |
|-----|-----|--------|
| | | |
| | | |
| | | |

---

## 🧠 Zadání projektu

Projekt je realizován ve dvou fázích:

### 1️⃣ Půlsemestrální odevzdání
- Datový model v Entity Framework
- Seedování databáze pomocí Minimal WebAPI
- Nastavení .NET Aspire (včetně commandu pro seedování)
- Společné WebAPI pro všechny klienty

### 2️⃣ Semestrální odevzdání
- Administrátorský webový klient
- Klient pro provoz menzy (dotykový panel)
- Klient pro studenty (menu a objednávky)
- Kompletní funkční systém

---

## 🏗️ Architektura

Popis architektury systému je uveden v souboru  
📄 `docs/architecture.md`

Stručný přehled:
- Backend: Minimal WebAPI + Entity Framework
- Frontend: Blazor Web
- Orchestrace: .NET Aspire

---

## 🗄️ Datový model

- Entity jsou definovány v projektu `Menza.Domain`
- Konfigurace EF Core je v `Menza.Infrastructure`

📄 Podrobný popis: `docs/architecture.md`

---

## 🌱 Seedování databáze

Seedování databáze je realizováno:
- pomocí samostatného projektu `Menza.Seed`
- spuštěním commandu přes .NET Aspire

