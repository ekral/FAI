# 📘 Semestrální projekt – Aplikační frameworky  
## 🌐 Webový systém pro knihovnu

---

## 🎯 1. Cíl projektu

Cílem semestrálního projektu je **navrhnout a implementovat komplexní webovou aplikaci**
pro správu knihovny s využitím aplikačních frameworků probíraných v předmětu.

Projekt navazuje na znalosti jazyka **C# a objektově orientovaného programování**
a zaměřuje se na jejich **praktickou aplikaci v reálném kontextu**.

Důraz je kladen zejména na:
- návrh doménového modelu
- architekturu aplikace
- práci s daty
- návrh a implementaci API
- testovatelnost a čitelnost kódu

Použití nástrojů založených na umělé inteligenci (AI) je **povoleno**.  
Student však musí být schopen **odevzdané řešení vysvětlit, obhájit a upravit**.

---

## 📚 2. Zadání – systém knihovny

Navrhněte a implementujte webový systém pro správu knihovny.

Aplikace musí pracovat s následujícími doménovými pojmy:

- **Kniha** – bibliografický záznam (název, autor, ISBN, apod.)
- **Výtisk knihy** – konkrétní fyzická kopie knihy
- **Čtenář**
- **Výpůjčka**

Model musí **rozlišovat knihu jako titul a její jednotlivé fyzické výtisky**.

---

## ⚙️ 3. Funkční požadavky

Aplikace musí umožňovat:
- evidenci knih a jejich výtisků
- evidenci čtenářů
- vypůjčení knihy konkrétnímu čtenáři
- vrácení knihy
- kontrolu dostupnosti výtisků
- zobrazení aktuálních i historických výpůjček

Systém musí zabránit:
- vypůjčení již vypůjčeného výtisku
- vzniku nekonzistentních stavů dat

---

## 🧠 4. Individuální rozšíření (povinné)

Každý student (nebo tým) si zvolí **jedno individuální rozšíření**, které bude plně
integrováno do aplikace.

Rozšíření musí ovlivnit:
- doménový model
- databázi
- API
- alespoň jeden test

Příklady rozšíření:
- rezervace knih
- více poboček knihovny
- role uživatelů (čtenář / knihovník)
- pokuty za pozdní vrácení
- digitální knihy
- statistiky výpůjček

---

## 🛠️ 5. Technické požadavky

### 5.1 Backend
- **ASP.NET Core Minimal Web API**
- RESTful návrh endpointů
- použití DTO objektů (nevracet entity přímo)
- validace vstupních dat
- správné použití HTTP status kódů

### 5.2 Datová vrstva
- **Entity Framework Core (Code First)**
- databázové migrace
- seed dat
- správně definované vztahy mezi entitami
- omezení na úrovni databáze

### 5.3 Frontend
- **Blazor (Server nebo WebAssembly)**
- formuláře pro práci s daty
- asynchronní komunikace s API
- zobrazení chybových stavů vrácených API

### 5.4 Testování
- **xUnit**
- unit testy doménové logiky
- alespoň jeden integrační test API
- testy musí ověřovat smysluplné scénáře

---

## 📦 6. Odevzdání

Student odevzdá:
1. Zdrojový kód aplikace (Git repozitář)
2. Soubor `README.md`, který bude obsahovat:
   - popis architektury
   - doménový model
   - seznam API endpointů
   - popis individuálního rozšíření
   - zdůvodnění klíčových návrhových rozhodnutí

---

## 🧮 7. Hodnoticí rubrika

| Oblast | Kritéria | Body |
|------|---------|-----:|
| Návrh doménového modelu | Smysluplné entity, vztahy, OOP | 20 |
| Entity Framework Core | Migrace, vztahy, omezení, seed | 15 |
| Web API | Návrh endpointů, DTO, validace | 15 |
| Frontend (Blazor) | Funkčnost, práce se stavem | 15 |
| Testování | Smysluplné testy, pokrytí logiky | 15 |
| Individuální rozšíření | Kvalita návrhu a integrace | 10 |
| Čitelnost a struktura | Architektura, naming, organizace | 5 |
| Obhajoba projektu | Porozumění a schopnost vysvětlení | 10 |
| **Celkem** |  | **105 → 100** |

---

## 🎤 8. Obhajoba projektu

Součástí hodnocení je **krátká ústní obhajoba**, během které student odpovídá
na otázky týkající se návrhu a implementace projektu.

Typická témata obhajoby:
- doménový model a vztahy
- práce s databází a konzistencí dat
- návrh Web API
- řešení chybových stavů
- testování a rozšiřitelnost aplikace

---

## ✅ 9. Doporučení

Cílem projektu není napsat co nejvíce kódu, ale **vytvořit řešení, kterému student rozumí
a které je schopen obhájit**.
