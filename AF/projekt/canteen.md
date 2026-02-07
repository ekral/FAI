# Objednávací systém v menze

Semestrální projekt do předmětu **Aplikační frameworky**.  

Cílem projektu je návrh a implementace objednávacího systému pro menzu
s využitím nástrojů a frameworků **.NET Aspire, Minimal WebAPI, Entity Framework a Blazor**.

## 🧠 Zadání projektu

Objednávací systém pro menzu umožní objednávání minutek (jídel připravovaných na objednávku), kdy student si objedná jídlo v menze ve webové aplikaci běžící na dotykovém panelu a kuchařky jej začnou připravovat a budou měnit stav objednávky ve webové aplikaci běžící na dotykovém panelu. Student bude o stavu objednané minutky informován ve webové aplikaci.

### Funkční požadavky:

- Vedení menzy:
    - Jídla
        - Zobrazuje seznam jídel (popis a cena jídla).
        - Vytváří nové jídla.
        - Upravuje jídla. Jídlo se neodstraňuje, jen se označí že není aktivní.
    - Menu
        - Zobrazuje všechny položky menu (datum, jídlo, počet dostupných porcí) pro všechny dny.
        - Vytváří novou položku menu.
        - Upravuje položky menu.
        - Odstraňuje položky menu.
- Kuchařka v menze
    - Objednávky
        - Zobrazí seznam objednávek které nejsou dokončené.
        - Označí, že je objednávka s daným číslem hotová, zrušená nebo dokončená (vydaná studentovi nebo byl student informován o zrušení).
- Student
    - Objednávky
        - Student si zobrazí menu pro aktuální den (vyprodaná jídla budou přeškrnutá).
        - Student si objedná si jídlo z aktuálního menu (sníží se počet dostupných porcí jídla). 
        
Stavy objednávy:
- Připravuje se.
- Hotová (připraveno k vyzvednutí).
- Zrušená.
- Dokončená.

Nefukční požadavky

Díky použití nástrojů [Aspire](https://aspire.dev/get-started/what-is-aspire/) bude mít vyučující možnost spustit vytvoření projekt lokálně včetně použité databáze a KeyCloacku. 

Solution musí být přeložitelný a spustitelný bez chyb s využitím Aspire niže popsaných bodů jinak bude mít projekt **hodnocení 0 bodů**.

- Projekt s pomocí nástrojů [Aspire](https://aspire.dev/get-started/what-is-aspire/):
    - Vytvoří databázi, například [SQL Server](https://aspire.dev/integrations/databases/efcore/sql-server/sql-server-get-started/).
    - Použije Identity nástroj [KeyCloack](https://aspire.dev/integrations/security/keycloak/) k zabezpečení aplikace.
    - Využije Aspire [Service Discovery](https://aspire.dev/fundamentals/service-discovery/), aby nebylo nutné nastavovat v kódu konkrétní ip adresy.
    - Použije [Http Command](https://aspire.dev/fundamentals/http-commands/#http-command-apis) pro restart databáze při kterém se vymaže existující databaze, vytvoří se nová a vloží se testovací data. 
- V projektu se využijí DTO (Data Transfer Objects) nezávislé na Entitách pro přenos dat.
- V projektu se nebude opakovat kód, například DTO budou nadefinované jen na jednom místě.

## 🏗️ Architektura

- Základní struktura řešení:
    - UTB.Minute.Db - bude obsahovat Entity a DataContext.
    - UTB.Minute.DbManager - bude obsahovat WebApi pro Http Command.
    - UTB.Minute.WebAPI - bude obsahovat webové služby. Bude mít referenci na projekt UTB.Minute.Dba UTB.Minute.Contracts.
    - UTB.Minute.AdminClient - Blazor Server Interactivity projekt, klient pro vedení menzy pro editaci jídel a menu. Bude mít referenci na projekt UTB.Minute.WebAPI a UTB.Minute.Contracts.
    - UTB.Minute.CanteenClient - Blazor Server Interactivity projekt, klient pro studenty a kuchařky v menze. Bude mít referenci na projekt UTB.Minute.WebAPI a UTB.Minute.Contracts.


Půlsemestráln


    

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

