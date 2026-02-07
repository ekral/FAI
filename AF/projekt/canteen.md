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

- Projekt využívá **.NET Aspire**:
  - Vytváří databázi (např. SQL Server).
  - Používá Identity nástroj **Keycloak** k zabezpečení aplikace.
  - Využívá **Service Discovery**, bez pevně zadaných IP adres.
  - Obsahuje **Http Command** pro reset databáze (smazání, vytvoření, seed testovacích dat).
- Projekt používá **DTO (Data Transfer Objects)** nezávislé na entitách.
- Kód se neopakuje (DTO jsou definována pouze na jednom místě).
- Projekt využívá **Server-Sent Events (SSE)** pro serverem iniciované notifikace
  o změnách v objednávkách pro studenta a kuchařku.

---

## 🏗️ Architektura

### Základní struktura řešení

- `UTB.Minute.Db` – entity a `DbContext`
- `UTB.Minute.DbManager` – WebAPI pro Http Command, reset a seed databáze  
  (reference na `UTB.Minute.Db`)
- `UTB.Minute.Contracts` – DTO (Data Transfer Objects)
- `UTB.Minute.WebAPI` – společné WebAPI pro všechny klienty včetně
  Server-Sent Events (SSE) notifikací  
  (reference na `UTB.Minute.Db` a `UTB.Minute.Contracts`)
- `UTB.Minute.AdminClient` – Blazor Server aplikace pro vedení menzy  
  (reference na `UTB.Minute.Contracts`)
- `UTB.Minute.CanteenClient` – Blazor Server aplikace pro studenty a kuchařky  
  (reference na `UTB.Minute.Contracts`)

---

# 📝 Objednávací systém v menze – checklist a hodnocení

## 📤 Půlsemestrální odevzdání (20 bodů)

Studenti odevzdávají pouze **backend a WebAPI** (bez klientských aplikací a SSE).

### Projekty (0–4 body)
- [ ] `UTB.Minute.Db`, `UTB.Minute.DbManager`, `UTB.Minute.Contracts`, `UTB.Minute.WebAPI` jsou odevzdány a správně strukturované  

### Datový model (0–4 body)
- [ ] Entity a vazby správně navrženy  
- [ ] `DbContext` odpovídá požadavkům  
- [ ] DTO definována pouze v `UTB.Minute.Contracts`  

### Funkčnost WebAPI (0–4 body)
- [ ] CRUD pro Jídla  
- [ ] CRUD pro Menu  
- [ ] CRUD pro Objednávky (přidávání, změna stavu)  

### Aspire integrace (0–4 body)
- [ ] Databáze vytvořena přes Aspire  
- [ ] Reset a seed dat funguje přes Http Command  
- [ ] Service Discovery funguje  

### Kvalita kódu a architektury (0–4 body)
- [ ] Architektura odpovídá zadání  
- [ ] DTO používány správně, žádná duplicita  
- [ ] Kód čitelný, logicky strukturovaný  

---

## 🏁 Semestrální odevzdání (40 bodů)

Studenti odevzdávají **kompletní funkční systém**, backend + klienti + SSE.

### Projekty (0–4 body)
- [ ] `AdminClient` a `CanteenClient` připojené na WebAPI  
- [ ] Backend plně funkční  

### Student (0–6 body)
- [ ] Vidí menu pro aktuální den  
- [ ] Může objednávat jídlo  
- [ ] Vyprodané položky jsou přeškrtnuté  

### Kuchařka (0–5 body)
- [ ] Vidí seznam aktuálních objednávek  
- [ ] Mění stav objednávky (hotová, zrušená, dokončená)  

### Vedení menzy (0–5 body)
- [ ] CRUD pro Jídla  
- [ ] CRUD pro Menu (včetně deaktivace jídla)  

### Stav objednávky (0–4 body)
- [ ] Přechody stavů objednávky správně implementovány: Připravuje se → Hotová → Zrušená → Dokončená  

### SSE notifikace (0–5 body)
- [ ] SSE endpoint funguje  
- [ ] Notifikace dorazí studentovi i kuchařce  
- [ ] UI se aktualizuje v reálném čase  

### Autentizace a autorizace (0–6 body)
- [ ] Keycloak spuštěn přes Aspire  
- [ ] Backend chráněn, role správně přiřazeny  
- [ ] Klienti chrání routy a UI prvky podle role  

### Kvalita kódu a architektury (0–5 body)
- [ ] Architektura odpovídá zadání  
- [ ] DTO používány správně, žádná duplicita  
- [ ] Kód čitelný, logicky strukturovaný  
- [ ] Bez mrtvého kódu nebo citlivých dat  

### Aspire integrace (0–2 body)
- [ ] Service Discovery funguje  
- [ ] Http Commands a konfigurace správně nastaveny  

---

## ✅ Finální kontrola
- [ ] Projekt se spustí na čistém stroji přes Aspire  
- [ ] Všechny funkce dostupné a testovatelné  

## 🧮 Shrnutí bodování

| Část | Body |
|------|------|
| Průběžné testy | 0–40 |
| Půlsemestrální odevzdání | 0–20 |
| Semestrální odevzdání | 0–40 |
| **Celkem** | **0–100** |
