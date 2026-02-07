# Objednávací systém v menze

Semestrální projekt do předmětu **Aplikační frameworky**.

Cílem projektu je návrh a implementace objednávacího systému pro menzu
s využitím nástrojů a frameworků **.NET Aspire, Minimal WebAPI, Entity Framework Core a Blazor**.

---

## 🧠 Zadání projektu

Objednávací systém pro menzu umožňuje objednávání minutek (jídel připravovaných na objednávku).
Student si objedná jídlo ve webové aplikaci běžící na dotykovém panelu.
Kuchařky následně jídlo připravují a mění stav objednávky ve své webové aplikaci.
Student je o stavu objednávky informován v reálném čase.

---

## Funkční požadavky

### Vedení menzy

#### Jídla
- Zobrazuje seznam jídel (název, popis, cena).
- Vytváří nová jídla.
- Upravuje jídla. Jídlo se neodstraňuje, pouze se označí jako neaktivní.

#### Menu
- Zobrazuje položky menu (datum, jídlo, počet dostupných porcí) pro všechny dny.
- Vytváří nové položky menu.
- Upravuje položky menu.
- Odstraňuje položky menu.

### Kuchařka v menze

#### Objednávky
- Zobrazuje seznam objednávek, které nejsou dokončené.
- Mění stav objednávky na:
  - hotová,
  - zrušená,
  - dokončená (vydaná studentovi nebo student informován o zrušení).

### Student

#### Objednávky
- Zobrazuje menu pro aktuální den (vyprodaná jídla jsou přeškrtnutá).
- Objednává jídlo z aktuálního menu (sníží se počet dostupných porcí).

### Stavy objednávky
- Připravuje se
- Hotová (připraveno k vyzvednutí)
- Zrušená
- Dokončená

---

## Nefunkční požadavky

Díky použití nástrojů [Aspire](https://aspire.dev/get-started/what-is-aspire/)
musí být vyučující schopen spustit celý projekt lokálně včetně databáze a Keycloaku.

### Požadavky na řešení

- Projekt využívá **.NET Aspire**:
  - Vytváří databázi (např. SQL Server).
  - Používá Identity nástroj **Keycloak** k zabezpečení aplikace.
  - Využívá **Service Discovery**, bez pevně zadaných IP adres.
  - Obsahuje **Http Command** pro reset databáze (smazání, vytvoření, seed testovacích dat).
- Projekt používá **DTO (Data Transfer Objects)** nezávislé na entitách.
- Kód se neopakuje (DTO jsou definována pouze na jednom místě).

---

## 🏗️ Architektura

### Základní struktura řešení

- `UTB.Minute.Db` – entity a `DbContext`
- `UTB.Minute.DbManager` – WebAPI pro Http Command, reset a seed databáze
- `UTB.Minute.Contracts` – DTO (Data Transfer Objects)
- `UTB.Minute.WebAPI` – společné WebAPI pro všechny klienty  
  (reference na `UTB.Minute.Db` a `UTB.Minute.Contracts`)
- `UTB.Minute.AdminClient` – Blazor Server aplikace pro vedení menzy
- `UTB.Minute.CanteenClient` – Blazor Server aplikace pro studenty a kuchařky

---

# 📊 Hodnocení předmětu

Celkové hodnocení v předmětu je **100 bodů**:

- **40 bodů** – průběžné testy  
- **20 bodů** – půlsemestrální odevzdání  
- **40 bodů** – semestrální odevzdání  

---

## 📤 Půlsemestrální odevzdání (20 bodů)

Odevzdávají se projekty:

- `UTB.Minute.Db`
- `UTB.Minute.DbManager`
- `UTB.Minute.Contracts`
- `UTB.Minute.WebAPI`
- funkční databáze, reset a seedování dat
- **bez autentizace a autorizace**

> ⚠️ **Podmínka hodnocení**  
> Celé řešení musí být **plně spustitelné přes Aspire**, včetně databáze,
> seedování dat a Service Discovery.  
> Nesplnění této podmínky znamená **0 bodů**.

### Hodnoticí rubrika

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
- autentizace a autorizace pomocí **Keycloak**

> ⚠️ **Nutná podmínka**  
> Celé řešení musí být **plně spustitelné přes Aspire**, včetně databáze,
> seedování dat a Keycloak autentizace.  
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

## 🧮 Shrnutí bodování

| Část | Body |
|------|------|
| Průběžné testy | 0–40 |
| Půlsemestrální odevzdání | 0–20 |
| Semestrální odevzdání | 0–40 |
| **Celkem** | **0–100** |
