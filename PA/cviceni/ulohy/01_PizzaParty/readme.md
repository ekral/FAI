# 01 pizza party

Představte si, že organizujete pizza party pro své kamarády a potřebujete spočítat různé náklady a množství, aby bylo všeho dostatek
a nikomu nepochybělo, ale aby po akci nezůstalo nespotřebované jídlo a pití.
Pro práci na úkolu použijte přiložený soubor Program.cs, který obsahuje základ aplikace, do kterého následně přidáte další kód, 
podle zadání.


Odevzdejte jak soubor Program.cs, tak .pdf soubor s vypracovaným protokolem k zadání.
(Úvod, Postup práce včetně zdrojů ze kterých jste čerpal(a) inspiraci a Závěr, kde shrnete své pocity a trable z vypracovávání úkolu).


Celková náročnost úkolu = cca 2 hodiny

```csharp
/*
    ============== Úkol cvičení #1 ==============
    Představte si, že organizujete pizza party pro své kamarády a potřebujete spočítat různé náklady a množství,
    aby bylo všeho dostatek a nikomu nepochybělo, ale aby po akci nezůstalo nespotřebované jídlo a pití.

    Aplikace musí umět následující:
    1. Spočítat celkový počet klínků (slices), který je potřeba, aby nebyl nikdo o hladu.
    2. Spočítat celkový počet potřebných pizz (Nezapomeňte zaokrouhlit nahoru, protože nelze zakoupit 1/2 pizzy!)
    3. Spočítat kolik klínků pizzy zbyde
    4. Vypočítat náklady na pizzy a nápoje
    5. Vypočítat výši spropitného, kterou bude potřeba zaplatit poslíčkovi
    6. Sečíst veškeré náklady za celou akci
    7. Spravedlivě rozpočítat náklady mezi všechny účastníky
    
    Do aplikace potom přidejte alespoň 5 faktů ohledně pizzy, nápojů a ceny. Vyberte některé z následujícího seznamu
    nebo vymyslete vlastní fakta.
    1. Kolik metrů krychlových nápoje bude potřeba pro napojení všech účastníků?
    2. Kolik metrů čtverečních pizzy bude potřeba pro nakrmení všech účastníků?
    3. Pokud má pizza 285 kalorií a člověk v průměru spálí 50kal/km, kolik musí účastník ujít, aby spálil to co na párty sní?
    4. Pokud každá pizza váží 0.65kg, a nápoj 2.25kg, jakou musí mít stůl na párty minimální nosnost, aby mohl pojmout veškeré občerstvení?
    5. Kolik stupňů (úhel ne teplota) má každý krajíček a kolik je to radianů?
    6. Jak dlouhý je okraj jednotlivých klínků pizzy v cm? (Trigonometrie)
    7. Pokud nakládáte všechny pizza klínky za sebe do řady (z jakéhokoliv nepochopitelného důvodu), jak bude řada dlouhá? (V cm, km, ve fábiích...)
    atd.
    
    Odevzdejte jak soubor Program.cs, tak .pdf soubor s vypracovaným protokolem k zadání.
    (Úvod, Postup práce včetně zdrojů ze kterých jste čerpal(a) inspiraci a Závěr, kde shrnete své pocity a trable z vypracovávání úkolu).

    Celková náročnost úkolu = cca 2 hodiny
*/

Console.WriteLine("=== KALKULAČKA PIZZA PARTY ===");
Console.WriteLine();

// ČÁST 1: Deklarujte všechny proměnné zde
int guestCount = 12;
int slicesPerGuest = 3;
int slicesPerPizza = 8;

double pizzaCost = 15.99; // V Eurech, 40cm průměr
double drinksCost = 2.50; // V Eurech, každému bude stačit jeden nápoj, řekněme, že je to 2.25l Kofoly
double tip = 0.1; // 10% Dýško pro poslíčka s pizzou

// Proveďte základní výpočty
int slicesTotal = guestCount * slicesPerGuest;
double pizzasTotal = Math.Ceiling(slicesTotal / (double)slicesPerPizza);

// ... pokračujte s dalšími výpočty

// Zobrazte výsledky
Console.WriteLine("Detaily akce:");
Console.WriteLine("- Počet hostů: " + guestCount + " lidí");
Console.WriteLine("- Klínků na osobu: " + slicesPerGuest);

// ... pokračujte s dalším výstupem

// ČÁST 2: Výstup podivných faktů
Console.WriteLine();
Console.WriteLine("=== VĚDĚL JSI, ŽE...? ===");

Console.WriteLine();
Console.WriteLine("Stiskni libovolnou klávesu pro ukončení...");
Console.ReadKey();
```