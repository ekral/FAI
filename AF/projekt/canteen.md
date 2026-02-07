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

### Nefukční požadavky

Díky použití nástrojů [Aspire](https://aspire.dev/get-started/what-is-aspire/) bude mít vyučující možnost spustit vytvoření projekt lokálně včetně použité databáze a KeyCloacku. 

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
    - UTB.Minute.DbManager - bude obsahovat WebApi pro Http Command a reset databáze a seedování.
    - UTB.Minute.Contracts - bude obsahovat DTOs (Data Trasfer Objects).
    - UTB.Minute.WebAPI - společné WebAPI pro všechny klienty. Bude mít referenci na projekt UTB.Minute.Dba UTB.Minute.Contracts.
    - UTB.Minute.AdminClient - Blazor Server Interactivity projekt, klient pro vedení menzy pro editaci jídel a menu. Bude mít referenci na projekt UTB.Minute.WebAPI a UTB.Minute.Contracts.
    - UTB.Minute.CanteenClient - Blazor Server Interactivity projekt, klient pro studenty a kuchařky v menze. Bude mít referenci na projekt UTB.Minute.WebAPI.
    
# 📊 Hodnocení předmětu

## 📤 Půlsemestrální odevzdání (20 bodů)

Odevzdávají se projekty:

- `UTB.Minute.Db`
- `UTB.Minute.DbManager`
- `UTB.Minute.Contracts`
- `UTB.Minute.WebAPI`

> ⚠️ **Podmínka hodnocení**  
> Celé řešení musí být **plně spustitelné přes Aspire**, včetně databáze,
> Keycloak autentizace a seedování dat.  
> Nesplnění této podmínky znamená **0 bodů**.

### Hodnotící rubrika

| Kritérium | Popis | Body |
|----------|------|------|
| Architektura řešení | Dodržení předepsané struktury projektů | 0–4 |
| Datový model | Entity, vazby a `DbContext` (EF Core) | 0–4 |
| DTO a Contracts | DTO oddělené od entit, sdílené v `UTB.Minute.Contracts` | 0–4 |
| WebAPI | Funkční Minimal WebAPI, základní CRUD | 0–4 |
| Aspire integrace | DB, Service Discovery, Http Command (reset + seed) | 0–4 |
| **Celkem** |  | **0–20** |

---

## 🏁 Semestrální odevzdání (40 bodů)

Odevzdává se **kompletní funkční systém**:

- `UTB.Minute.AdminClient`
- `UTB.Minute.CanteenClient`
- plně funkční backend

> ⚠️ **Nutná podmínka**  
> Celé řešení musí být **plně spustitelné přes Aspire**, včetně databáze,
> Keycloak autentizace a seedování dat.  
> Nesplnění této podmínky znamená **0 bodů**.

### 🔧 Backend (20 bodů)

| Kritérium | Popis | Body |
|----------|------|------|
| Funkční požadavky API | Jídla, menu, objednávky, stavy | 0–6 |
| Stavový model objednávek | Připravuje se / hotová / zrušená / dokončená | 0–4 |
| Bezpečnost | Integrace Keycloak, role uživatelů | 0–4 |
| Kvalita kódu | Žádná duplicita, správné použití DTO | 0–3 |
| Aspire best practices | Service Discovery, Http Commands | 0–3 |
| **Celkem backend** |  | **0–20** |

### 🖥️ Klientské aplikace (20 bodů)

| Kritérium | Popis | Body |
|----------|------|------|
| AdminClient | Správa jídel a menu | 0–6 |
| CanteenClient – student | Zobrazení menu, objednání jídla | 0–6 |
| CanteenClient – kuchařka | Přehled objednávek, změna stavů | 0–4 |
| UX a funkčnost | Přehlednost, použití na dotykovém panelu | 0–4 |
| **Celkem klienti** |  | **0–20** |

---

## 🧮 Shrnutí bodování v předmětu

| Část | Body |
|------|------|
| Průběžné testy | 0–40 |
| Půlsemestrální odevzdání | 0–20 |
| Semestrální odevzdání | 0–40 |
| **Celkem** | **0–100** |
