# Adventure Game Decision Maker

## 🔢 Popis úkolu

Představte si, že vytváříte konzolovou "hru", ve které vytvoříte postavu pomocí odpovídání na vstupní otázky a program vypočítá,
co všechno vaše dokáže a v čem naopak selže.
Pro práci na úkolu použijte přiložený soubor Program.cs, který obsahuje základ aplikace, do kterého následně přidáte další kód, 
podle zadání.

Odevzdejte jak soubor Program.cs, tak .pdf soubor s vypracovaným protokolem k zadání.
(Úvod, Postup práce včetně zdrojů ze kterých jste čerpal(a) inspiraci a Závěr, kde shrnete své pocity a trable z vypracovávání úkolu).

⌛ Celková náročnost úkolu = cca 2 hodiny

## 🚀 Výchozí kód v C# se zadáním úkolů

```csharp
/*
    ============== Úkol cvičení #2 ==============
    Vytváříte jednoduchou hru v konzoli, která podle nastavení vybavení vaší postavy dokáže určit co všechno postava
    dokáže a na čem selže. Vyplňte všechny následující úkoly.
*/

// INICIALIZACE PROMĚNNÝCH
Console.WriteLine("=== ADVENTURE GAME DECISION MAKER ===\n");

Console.WriteLine("\n--- CHARACTER STATS ---\n");

// Použij desetinnou čárku, ne desetinou tečku!
Console.Write("Enter player current health percentage (0,0-1,0): ");
double health = double.Clamp(double.Parse(Console.ReadLine()), 0.0, 1.0);

Console.Write("Enter player level (1-10): ");
int level = int.Clamp(int.Parse(Console.ReadLine()),1,10 );

Console.WriteLine("\nChoose your class:");
Console.WriteLine("0 - Warrior, 1 - Wizard, 2 - Rogue, 3 - Cleric, 4 - Ranger");
Console.Write("Enter class number: ");
CharacterClass playerClass = (CharacterClass) int.Clamp(int.Parse(Console.ReadLine()), 0,Enum.GetNames(typeof(CharacterClass)).Length-1);

Console.WriteLine("\n--- CHARACTER EQUIPMENT ---\n");

Console.Write("Does player have a key? (true/false): ");
bool hasKey = bool.TryParse(Console.ReadLine(), out hasKey) ? hasKey : false;

Console.Write("Does player have a sword? (true/false): ");
bool hasSword = bool.TryParse(Console.ReadLine(), out hasSword) ? hasSword : false;

Console.Write("Does player have magic to spare? (true/false): ");
bool hasMagic = bool.TryParse(Console.ReadLine(), out hasMagic) ? hasMagic : false;

Console.WriteLine("\n--- CHARACTER WEALTH ---\n");

Console.Write("Enter amount of gold pieces (GP): ");
int goldPieces = int.Parse(Console.ReadLine());

Console.Write("Enter amount of silver pieces (SP): ");
int silverPieces = int.Parse(Console.ReadLine());

Console.Write("Enter amount of copper pieces (CP): ");
int copperPieces = int.Parse(Console.ReadLine());

Console.WriteLine("\n--- OTHER CIRCUMSTANCES ---\n");

Console.Write("Is it nighttime? (true/false): ");
bool isNight = bool.TryParse(Console.ReadLine(), out isNight) ? isNight : false;

Console.Write("What month is it? (1-12): ");
int month = int.Clamp(int.Parse(Console.ReadLine()), 1,12);

// ÚKOLY KE ZPRACOVÁNÍ
Console.WriteLine("\n--- HERO ACHIEVEMENTS ---\n");

// Task 1: Is player alive?
// (health is more than 0)
Console.Write("Player is alive and well: " + (health > 0));

// BASIC TASKS

// Task 2: Should player rest? 
// (health less than 30 OR it's nighttime)

// Task 3: Is player healthy? 
// (health greater than or equal to 70)

// Task 4: Is player beginner? 
// (level less than or equal to 3)

// Task 5: Is player unarmed? 
// (does NOT have sword)

// Task 6: Is player rich? 
// (gold pieces greater than or equal to 100)

// Task 7: Is it daytime? 
// (is NOT nighttime)

// Task 8: Is player a wizard? 
// (player class equals Wizard)


// INTERMEDIATE TASKS

// Task 9: Can obtain health potion from merchant? 
// (gold pieces greater than or equal to 50 OR is a Rogue of at least level 2)

// Task 10: Can obtain magic scroll? 
// (gold pieces greater than or equal to 20 OR (is a Wizard of at least level 3 AND has magic to spare))

// Task 11: Can get basic supplies? 
// (silver pieces greater than or equal to 10 OR (is a Ranger of at least level 3 AND is NOT nighttime))

// Task 12: Is summer? 
// (month is greater than or equal 6 AND month is lower than 9)

// Task 13: Can cast fireball? 
// (can use magic AND (player class equals Wizard OR player class equals Cleric) AND level greater than or equal to 3)

// Task 14: Can stealth attack? 
// (player class equals Rogue AND (is nighttime OR level greater than 6) AND has sword)

// Task 15: Can use heavy armor? 
// (player class equals Warrior OR player class equals Cleric) AND level greater than or equal to 2

// Task 16: Should meditate to regain magical powers? 
// (can NOT use magic AND health greater than 50 AND (is nighttime OR player class equals Wizard))

// Task 17: Can dual wield weapons? 
// ((is NOT Wizard AND is NOT Cleric) AND level is NOT lower than 3)

// Task 18: Eligible for guild membership? 
// (level greater than or equal to 5 AND (gold pieces greater than 100 OR player class equals Wizard))


// ADVANCED TASKS

// Task 19: Can buy legendary sword from merchant? 
// (gold pieces greater than or equal to 500 AND level greater than or equal to 7 AND (player class equals Warrior OR player class equals Ranger))

// Task 20: Can contact Druids? 
// (is NOT winter AND player class equals Ranger OR Wizard)

// Task 21: Is it warm outside? 
// ((is NOT nighttime AND is NOT winter) OR (is summer))

// Task 22: Can learn arcane spells? 
// (can use magic AND (player class equals Wizard OR player class equals Cleric) AND (level greater than or equal to 4 OR gold pieces greater than 200))

// Task 23: Can access VIP merchant area? 
// (gold pieces greater than 150 OR (silver pieces greater than 800 AND has key) AND level greater than 5 AND (player class is NOT Rogue) AND has no sword)

// Task 24: Is battle-ready spellcaster? 
// (can use magic AND (player class equals Wizard OR player class equals Cleric) AND health greater than 0.6 AND (has sword OR gold pieces greater than 100))

// Task 25: Can gain audience with archbishop? 
// (has NOT a sword AND (player class equals Cleric AND level greater than 6) OR (level greater or equal than 8 AND has a key AND gold pieces greater than or equal 500)

// Task 26: Should visit healer? 
// (health less than 0.4 OR (health less than 0.6 AND is nighttime)) AND (gold pieces greater than 10 OR player class equals Cleric)

// Task 27: Can start merchant business? 
// (gold pieces greater than 1000 AND level greater than or equal to 8 AND has key AND player class is NOT Ranger)

// Task 28: Is a versatile warrior? 
// (has sword AND can use magic) OR (player class equals Ranger OR player class equals Cleric) AND level greater than 5 AND health greater than 0.5)

// Task 29: Can afford magic training? 
// (gold pieces greater than 100 AND can use magic) AND (player class equals Wizard OR player class equals Cleric OR level greater than 7)

// Task 30: Is legendary adventurer? 
// (level equal to 10 AND health greater than or equal to 0.8 AND can use magic AND has sword AND has key AND gold pieces greater than 500)



enum CharacterClass
{
    Fighter,
    Wizard,
    Rogue,
    Cleric,
    Ranger
}
```