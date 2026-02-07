# ✅ Kompaktní checklist pro semestrální projekt

---

## 🔧 Obecné

- [ ] Projekt se přeloží bez chyb
- [ ] Spustí se přes **Aspire** bez ruční konfigurace
- [ ] Databáze se vytváří a seeduje automaticky (`Http Command`)
- [ ] DTO jsou oddělené od entit a definovány jen v `UTB.Minute.Contracts`
- [ ] Kód není duplikovaný

---

## 📤 Půlsemestrální odevzdání (20 bodů)

- [ ] Projekty: `Db`, `DbManager`, `Contracts`, `WebAPI`
- [ ] Entity a vztahy odpovídají zadání
- [ ] CRUD API funguje
- [ ] Aspire: Service Discovery, Http Command

---

## 🏁 Semestrální odevzdání (40 bodů)

- [ ] Projekty: `AdminClient`, `CanteenClient`, plně funkční backend
- [ ] Student: vidí menu, objednává, vyprodané jídlo přeškrtnuté
- [ ] Kuchařka: vidí objednávky, mění stavy
- [ ] Vedení menzy: spravuje jídla a menu
- [ ] Stav objednávek: Připravuje se / Hotová / Zrušená / Dokončená

### 🔔 SSE notifikace

- [ ] SSE endpoint funguje
- [ ] Notifikace pro studenta i kuchařku
- [ ] UI aktualizuje stav objednávek v reálném čase

### 🔐 Autentizace a autorizace

- [ ] Keycloak spuštěn přes Aspire
- [ ] Backend chráněn, role správně přiřazeny
- [ ] Klienti chrání routy a UI prvky podle role

### 🖥️ Klienti (Blazor)

- [ ] Komunikace přes WebAPI
- [ ] Stav aplikace korektně aktualizován
- [ ] Funkční a přehledné UI na dotykovém panelu

### 🧱 Kvalita

- [ ] Architektura odpovídá zadání
- [ ] Kód čitelný a logicky strukturovaný
- [ ] Bez mrtvého kódu nebo citlivých dat

---

## ✅ Finální kontrola

- [ ] Projekt se spustí na čistém stroji přes Aspire
- [ ] Kompletní odevzdání, všechny funkce dostupné
- [ ] Checklist kompletně prošel/a
