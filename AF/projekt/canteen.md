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
musí být vyučující schopen spustit celý projekt lokálně včetně databáze a Keycloak.

### Požadavky na řešení

- .NET 10
- Jazyk použitý ve zdrojovém kódu bude **angličtina**.
- Projekt využívá **.NET Aspire**:
  - Vytváří databázi (např. SQL Server).
  - Používá Identity nástroj **Keycloak** k zabezpečení aplikace.
  - Využívá **Service Discovery**, bez pevně zadaných IP adres.
  - Obsahuje **Http Command** pro reset databáze (smazání, vytvoření, seed testovacích dat).
- Projekt používá **Entity framework pro práci s databází**.
- Projekt používá **DTO (Data Transfer Objects)** nezávislé na entitách.
- Kód se neopakuje (DTO jsou definována pouze na jednom místě).
- Projekt využívá **Server-Sent Events (SSE)** pro serverem iniciované notifikace
  o změnách v objednávkách pro studenta a kuchařku.

### Dokumentace a odevzdání
- Student odevzdá pouze zdrojové kódy bez binárních a dočasných souborů (jak je projekt například uložený na Githubu).
- Součástí odevzdaného projektu bude i stručná dokumentace ve formátu Markdown (readme.md), která vysvětlí použitá architektonická rozhodnutí a případné problémy při řešení a připomínky.
- Odevzdává se zazipovaný soubor se zdrojovými soubory a dokumentací.

---

## 🏗️ Architektura

### Základní struktura řešení

- `UTB.Minute.Db` – entity a `DbContext`
- `UTB.Minute.DbManager` – WebAPI pro Http Command, reset a seed databáze  
  (reference na `UTB.Minute.Db`)
- `UTB.Minute.Contracts` – DTO (Data Transfer Objects)
- `UTB.Minute.WebAPI` – společné WebAPI pro všechny klienty včetně
  Server-Sent Events (SSE) notifikací (reference na `UTB.Minute.Db` a `UTB.Minute.Contracts`)
- `UTB.Minute.WebAPI.Tests` - test WebAPI využívající použitou databázi, například SQL Server (reference na `UTB.Minute.WebAPI`).     
- `UTB.Minute.AdminClient` – Blazor Server aplikace pro vedení menzy  
  (reference na `UTB.Minute.Contracts`)
- `UTB.Minute.CanteenClient` – Blazor Server aplikace pro studenty a kuchařky  
  (reference na `UTB.Minute.Contracts`)

---

# 📝 Objednávací systém v menze – checklist a hodnocení

Tento checklist slouží:
- **studentům** jako kontrolní seznam před odevzdáním
- **vyučujícím** jako jednotná hodnoticí kritéria

⚠️ **Důležité pravidlo**  
Pokud se projekt **nesestaví nebo nespustí**, hodnotí se odevzdání **0 body**  
(bez ohledu na částečnou implementaci funkcionality).

---

## 📤 Půlsemestrální odevzdání (20 bodů)

Studenti odevzdávají pouze **backend a WebAPI**  
*(bez klientských aplikací a bez SSE)*

---

### Projekty a struktura řešení (0–3 body)
- [ ] Všechny požadované projekty existují a jsou správně pojmenované (2 body)  
  (`UTB.Minute.Db`, `DbManager`, `Contracts`, `WebAPI`, `WebAPI.Tests`)
- [ ] Správné reference mezi projekty (1 bod)

---

### Datový model a DTO (0–5 bodů)
- [ ] Entity a jejich vazby odpovídají zadání (1 bod)
- [ ] Správně navržený `DbContext` (1 bod)
- [ ] Stav objednávky řešen enumem (1 bod)
- [ ] DTO jsou definována pouze v `UTB.Minute.Contracts` (1 bod)
- [ ] WebAPI nevrací entity přímo (1 bod)

---

### Funkčnost WebAPI jeho testy (0–6 bodů)

#### Jídla (0–2 body)
- [ ] Vytvoření a čtení jídel a jejich testy (1 bod)
- [ ] Úprava jídla + deaktivace a jejich testy (1 bod)

#### Menu (0–2 body)
- [ ] Vytvoření a čtení položek menu a jejich testy (1 bod)
- [ ] Úprava a smazání položek menu a jejich testy (1 bod)

#### Objednávky (0–2 body)
- [ ] Vytvoření a čtení objednávek a jejich testy (1 bod)
- [ ] Změna stavu objednávky a jeho test (1 bod)

---

### Aspire integrace (0–4 body)
- [ ] Databáze vytvořena a konfigurována přes Aspire (1 bod)
- [ ] Http Command pro reset databáze (1 bod)
- [ ] Seed testovacích dat funguje (1 bod)
- [ ] Service Discovery bez pevných adres (1 bod)

---

### Testy a dokumentace (0–2 body)
- [ ] Stručná dokumentace projektu (README.md) (2 body)

---

### Srážkové body (záporné)
- [ ] Nepoužití angličtiny nebo starší verze než .NET 10  
- [ ] Bugy, warningy, porušení nefunkčních požadavků 

---

✅ **Součet: 20 bodů**

---

## 🏁 Semestrální odevzdání (40 bodů)

Studenti odevzdávají **kompletní funkční systém**  
*(backend + klienti + SSE + zabezpečení)*

---

### Projekty a integrace (0–6 bodů)
- [ ] `AdminClient` a `CanteenClient` napojené na WebAPI (3 body)
- [ ] Backend plně funkční a použitý oběma klienty (3 body)

---

### Student – funkcionalita klienta (0–6 bodů)
- [ ] Zobrazení menu pro aktuální den (2 body)
- [ ] Objednání jídla + snížení počtu porcí (2 body)
- [ ] Vyprodaná jídla vizuálně odlišena (2 body)

---

### Kuchařka – funkcionalita klienta (0–6 bodů)
- [ ] Zobrazení nedokončených objednávek (2 body)
- [ ] Změna stavu objednávky (hotová / zrušená / dokončená) (2 body)
- [ ] Neplatné přechody jsou blokovány (2 body)

---

### Vedení menzy – funkcionalita klienta (0–5 bodů)

#### Jídla (0–3 body)
- [ ] Vytváření jídel (1 bod)
- [ ] Úprava jídel (1 bod)
- [ ] Deaktivace jídla (1 bod)

#### Menu (0–2 body)
- [ ] Vytváření položek menu (1 bod)
- [ ] Úprava položek menu (1 bod)

---

### SSE notifikace (0–7 bodů)
- [ ] Funkční SSE endpoint (3 body)
- [ ] Notifikace pro studenta i kuchařku (2 body)
- [ ] Automatická aktualizace UI (2 body)

---

### Autentizace a autorizace (0–6 bodů)
- [ ] Keycloak spuštěn přes Aspire (2 body)
- [ ] Backend zabezpečen podle rolí (2 body)
- [ ] UI reaguje na roli uživatele (2 body)

---

### Dokumentace (0–4 body)
- [ ] Aktualizovaná dokumentace k finálnímu řešení (4 body)

---

### Srážkové body (záporné)
- [ ] Nepoužití angličtiny nebo starší verze než .NET 10  
- [ ] Bugy, warningy, porušení nefunkčních požadavků  

---

✅ **Součet: 40 bodů**


---

## 🧮 Shrnutí bodování

| Část | Body |
|------|------|
| Průběžné testy | 0–40 |
| Půlsemestrální odevzdání | 0–20 |
| Semestrální odevzdání | 0–40 |
| **Celkem** | **0–100** |
