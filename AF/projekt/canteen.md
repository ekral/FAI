# Objednávací systém v menze

Semestrální projekt do předmětu **[název předmětu]**.  
Cílem projektu je návrh a implementace objednávacího systému pro menzu
s využitím technologií **.NET Aspire, Minimal WebAPI, Entity Framework a Blazor**.

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

