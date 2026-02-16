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
- Zobrazuje seznam jídel (popis, cena).
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
- Zobrazuje menu pro aktuální den (vyprodaná jídla jsou vizuálně odlišena).
- Objednává jídlo z aktuálního menu (sníží se počet dostupných porcí).

### Stavy objednávky
- Připravuje se (sníží počet porcí)
- Hotová (připraveno k vyzvednutí)
- Zrušená (zrušená objednávka nevrací porci zpět)
- Dokončená

---

## Nefunkční požadavky

Díky použití nástrojů [Aspire](https://aspire.dev/get-started/what-is-aspire/)
musí být vyučující schopen spustit celý projekt lokálně včetně databáze a Keycloak.

### Požadavky na řešení

- .NET 10
- Jazyk použitý ve zdrojovém kódu bude **angličtina**.
- Projekt využívá [**Aspire**](https://aspire.dev/get-started/what-is-aspire/):
  - Vytváří databázi (např. [**SQL Server**](https://aspire.dev/integrations/databases/efcore/sql-server/sql-server-get-started/)).
  - Používá Identity nástroj [**Keycloak**](https://aspire.dev/integrations/security/keycloak/) k zabezpečení aplikace.
  - Využívá **Service Discovery**, bez pevně zadaných IP adres.
  - Obsahuje **Http Command** pro reset databáze (smazání, vytvoření, seed testovacích dat).
- Projekt používá **Entity framework pro práci s databází**.
- Projekt používá **Minimal Web API** s TypedResults.
- Projekt používá **DTO (Data Transfer Objects)** nezávislé na entitách.
- Kód se neopakuje (DTO jsou definována pouze na jednom místě).
- Projekt využívá **Server-Sent Events (SSE)** pro serverem iniciované notifikace
  o změnách v objednávkách studentů a pro kuchařku. SSE o změnách objednávek se broadcastují všem bez zabezpečení.
- Klientské aplikace volají Minimal Web API pomocí HTTP protokolu a nepřistupují přímo k databázi a entitám.
- Testy budou využívat "produkční" databázi, například SQL Server a ne InMemory EF. Testy musí běžet automaticky bez manuálního zásahu (pomocí databáze spuštěné přes Aspire).

---

## 📂 Struktura řešení

V solution budou následující projekty:

- `UTB.Minute.AppHost` - Aspire Integrace.
- `UTB.Minute.Db` – entity a `DbContext`.
- `UTB.Minute.DbManager` – WebAPI pro Http Command, reset a seed databáze (reference na `UTB.Minute.Db`).
- `UTB.Minute.Contracts` – DTO (Data Transfer Objects).
- `UTB.Minute.WebAPI` – společné WebAPI pro všechny klienty včetně Server-Sent Events (SSE) notifikací (reference na `UTB.Minute.Db` a `UTB.Minute.Contracts`).
- `UTB.Minute.WebAPI.Tests` - test WebAPI využívající použitou databázi, například SQL Server (reference na `UTB.Minute.WebAPI`).     
- `UTB.Minute.AdminClient` – Blazor Server aplikace pro vedení menzy (reference na `UTB.Minute.Contracts`). Volá WebAPI pomocí protokolu HTTP.
- `UTB.Minute.CanteenClient` – Blazor Server aplikace pro zjednodušení pro studenty a kuchařky (nutno zabezpečit přístup). Pro kuchařky a studenty je možné i vytvořit nezávislé projekty (reference na `UTB.Minute.Contracts`). Volá WebAPI pomocí protokolu HTTP.

---

# 📝 Checklist a hodnocení

Tento checklist slouží:
- **studentům** jako kontrolní seznam před odevzdáním
- **vyučujícím** jako jednotná hodnoticí kritéria

> [!WARNING]
> **Důležité pravidlo** 
> Pokud se odevzdaný projekt **nedá sestavit nebo spustit**,
> **není v angličtině** nebo **není vytvořen v .NET 10**  
> bude hodnocen **0 body**
> (a to bez ohledu na míru implementované funkcionality).

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

### Funkčnost WebAPI a jeho testy (0–6 bodů)

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
- [ ] Bugy, warningy (-1 bod za každý)
- [ ] Nedodržení jmenných konvencí (-2 body)

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

### Student – funkcionalita klienta (0–10 bodů)
- [ ] Zobrazení menu pro aktuální den (2 body)
- [ ] Zobrazení seznamu objednávek (2 bod)
- [ ] Objednání jídla + snížení počtu porcí (2 body)
- [ ] Vyprodaná jídla vizuálně odlišena (2 body)
- [ ] Řešena souběžnost při objednávání poslední porce na úrovni databáze nebo transakce (např. optimistic concurrency, RowVersion) (2 body)

---

### Kuchařka – funkcionalita klienta (0–6 bodů)
- [ ] Zobrazení nedokončených objednávek (2 body)
- [ ] Změna stavu objednávky (hotová / zrušená / dokončená) (2 body)
- [ ] Neplatné přechody objednávek jsou blokovány (např. nelze přejít ze 'Zrušeno' na 'Hotovo') (2 body)

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

### SSE notifikace (0–5 bodů)
- [ ] Funkční SSE endpoint (2 body)
- [ ] Notifikace pro studenta i kuchařku (2 body)
- [ ] Automatická aktualizace UI (1 bod)

---

### Autentizace a autorizace (0–6 bodů)
- [ ] Keycloak spuštěn přes Aspire (2 body)
- [ ] Backend zabezpečen podle rolí (2 body)
- [ ] UI reaguje na roli uživatele (2 body)

---

### Dokumentace (0–2 body)
- [ ] Aktualizovaná dokumentace k finálnímu řešení (2 body)

---

### Srážkové body (záporné)
- [ ] Bugy, warningy (-1 bod za každý)
- [ ] Nedodržení jmenných konvencí (-2 body)

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


## Řešení a odevzdávání projektu

Projekt vypracovávají studenti ve **tří členném týmu** (výjimečně jiný počet pokud se studenti nemohou rozdělit přesně na tříčlenné týmy).

⚠️ **Varování:** Všechny projekty budou testovány na plagiátorství. Jakékoliv zjištěné kopírování nebo předání cizí práce bude považováno za plagiát a bude mít za následek **0 bodů z projektu** a případné další akademické sankce podle pravidel univerzity.

### Dokumentace a odevzdání

- Student odevzdá pouze **zdrojové kódy** bez binárních a dočasných souborů  
  (např. tak, jak je projekt uložen na GitHubu).  
- Součástí odevzdaného projektu bude **stručná dokumentace ve formátu Markdown (readme.md)**,  
  která vysvětlí použitá architektonická rozhodnutí a případné problémy při řešení. Zde je šablona: [readme-template.md](readme-template.md).  
- Dokumentace musí také obsahovat **poměr práce jednotlivých členů týmu**,  
  aby bylo možné rozdělit body podle skutečného přínosu. Například `1:1:1` znamená rovnoměrný přínos, `1:1:2` znamená, že student 3 pracoval tolik, co studenti 1 a 2 dohromady.  
- Odevzdává se **zazipovaný soubor** se zdrojovými soubory a dokumentací.  

> Vedoucí teamu odevzdá zazipovaný soubor se zdrojovým kódem a dokumentací. Ostatní studenti odevzdají jen dokumentaci.

## Obhajoba projektu

Cílem obhajoby je ověřit, že student rozumí odevzdanému projektu. 
Skládá se z **písemné části** pro všechny studenty a **dobrovolné ústní obhajoby** 🗣️ pro ty, kteří chtějí své znalosti prokázat více.

Pro řešení projektu můžete používat AI, ale odevzdanému kódu musíte rozumět.

Příklady otázek:

- Co vše by bylo potřeba v projektu udělat pro přidání nového stavu objednávky?  
- Jakým způsobem byste do projektu přidali nového Webového Clienta pracujícího s databází pomocí EF? 

> Student si vybere jednu otázku odpovídající jeho části projektu a zodpoví ji textově v Moodle, bez použití vývojového nástroje.

Procenta projektu podle písemné obhajoby:

| Úroveň porozumění | Procento projektu | Poznámka |
|------------------|-----------------|-----------|
| Plně prokázal    | 100 %           | Student rozumí všem klíčovým částem projektu. |
| Slabé porozumění | 70 %            | Drobná mezera (např. SSE, DTO, WebAPI). |
| Výrazné mezery   | 40 %            | Neznalost klíčového toku objednávky nebo integrace. |
| Neprokázal       | 30 %            | Projekt nebyl pochopen nebo byl použitý cizí kód. |

Výpočet bodů projektu po obhajobě:

Projektové body po obhajobě = Projektové body × (procento splnění z písemné/ústní obhajoby).

> ⚠️ Body z testů se tímto neovlivňují. Procenta se vztahují **pouze k projektové části (60 b)**.

### 🗣️ Ústní obhajoba

- Každý student, který chce, může přijít **obhájit ústně**.  
- Celkem je možné navýšit procento projektu až na **100 %**.  
- Doporučené studentům s velmi nízkým procentem (≤30 %) z písemné části, aby si mohli hodnocení zlepšit.  
- Ti, kdo nepřijdou, zůstávají u procenta projektu z písemné části.

---

## Bonusový úkol

Bonusovým úkolem je vypracování desktopového klienta pro kuchařky ve frameworku [**Avalonia**](https://avaloniaui.net/).