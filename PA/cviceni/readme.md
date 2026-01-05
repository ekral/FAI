
# D&D 5.5e Character Sheet Application - Course Task List

## Project Overview

Throughout this course, students will build a functional D&D 5e/5.5e character sheet application in C#. Each lesson introduces new programming concepts that will be used to improve and expand the application.

----------

## Lesson 3: Code Branching (If, Else-If, Switch)

### Mandatory Tasks

1.  **Welcome Message**
    
    -   Display a welcome message: "Welcome to D&D Character Creator!"
    -   Ask the user for their character's name and store it in a variable
2.  **Background Selection**
    
    -   Create a variable to store character background
    -   Use `Console.WriteLine()` to display background options (Choose 4 backgrounds from [wiki](https://5point5.fandom.com/wiki/Background))
    -   Use `Console.ReadLine()` to get user input
    -   Use `if-else` statements to validate the input and set the background
3.  **Background Ability Bonuses**
    
    -   Create variables for the six ability scores (Strength, Dexterity, Constitution, Intelligence, Wisdom, Charisma)
    -   Initialize all abilities to default 10
    -   Use `switch` statement on background to apply background bonuses
4.  **Class and Species Selection**
    -   Display class options (Cleric, Fighter, Rogue, Wizard)
    -   Use `if-else` chain to validate and store the class choice
    -  Do the same for Species
5.  **Starting Hit Points**
    
    -   Create an HP variable
    -   Use `switch` statement on class to set starting HP
        -   Fighter: 10 + Constitution modifier
        -   Rogue + Cleric: 8 + Constitution modifier
        -   Wizard: 6 + Constitution modifier
    -   Calculate Constitution modifier using formula: (Constitution - 10) / 2 (integer division)
6.  **Ability Check Simulation**
    
    -   Ask user which ability they want to check (Strength, Dexterity, etc.)
    -   Use nested `if` statements to determine the ability modifier
    -   Display: "Your [Ability] modifier is: [modifier]"
   
8.  **Character Summary**
    
    -   Display all character information collected so far
    -   Format: Name, Race, Class, HP, AC, and all six ability scores

### Optional Tasks

1.  **Lineage Selection**
    
    -   Add lineage options for Elves (High Elf, Wood Elf) and Dwarves (Mountain Dwarf, Hill Dwarf)
    -   Use nested `switch` or `if` statements to apply additional bonuses
2.  **Alignment Choice**

    - Add alignment selection (Lawful Good, Neutral, Chaotic Evil, etc.)
    - Use `switch` expression [(C# 8.0+)](https://learn.microsoft.com/cs-cz/dotnet/csharp/language-reference/operators/switch-expression) to validate input
3.  **Level Input with Validation**
    
    -   Ask for character level (1-20)
    -   Use `if` statements to validate the range
    -   Calculate proficiency bonus based on level: +2 (levels 1-4), +3 (5-8), +4 (9-12), +5 (13-16), +6 (17-20)

----------

## Lesson 4: Code Cycling (While, For, Do-While Loops)

### Mandatory Tasks

1.  **Main Menu Loop**
    
    -   Refactor the program to use a `while(true)` loop
    -   Create a menu with options:
        1.  Create New Character
        2.  View Character
        3.  Exit
    -   Use `switch` to handle menu choices
    -   Add "Exit" option that uses `break` to exit the loop
2.  **Dice Roller**
    
    -   Create a dice rolling feature using `Random` class
    -   Ask user how many dice to roll (1-10)
    -   Use a `for` loop to roll that many d20s
    -   Display each roll: "Roll 1: 15, Roll 2: 8, ..."
    -   Display the total
3.  **Ability Score Manual Entry**
    
    -   Replace the fixed ability scores with manual entry
    -   Use a `for` loop to iterate through all 6 abilities
    -   For each ability, ask user to enter a value (3-18)
    -   Use a `while` loop to validate each input (repeat until valid)
4.  **Repeated Ability Checks**
    
    -   In the "View Character" menu option, add a submenu loop
    -   Options: Roll Ability Check, Roll Attack, Back to Main Menu
    -   Use `do-while` loop for the submenu
5.  **Attack Roll Loop**
    
    -   Create an attack roll feature
    -   Ask how many attacks to make
    -   Use `for` loop to roll attacks
    -   For each attack: roll d20, add appropriate modifier, compare to target AC
    -   Display hit or miss for each attack
6.  **HP Modification Loop**
    
    -   Create a submenu for HP management
    -   Use `while` loop with options: Take Damage, Heal, View Current HP, Back
    -   Update HP based on user input
    -   Prevent HP from going below 0 or above maximum
7.  **Initiative Roller**
    
    -   Ask for number of combatants (2-8)
    -   Use `for` loop to roll initiative for each (d20 + Dexterity modifier)
    -   Display each roll
8.  **Character Validation**
    
    -   After character creation, use a `while` loop to ask: "Is this correct? (yes/no)"
    -   If "no", restart character creation
    -   Continue until user confirms

### Optional Tasks

1.  **Advantage/Disadvantage System**
    
    -   When rolling ability checks or attacks, ask if rolling with advantage or disadvantage
    -   Use a `for` loop to roll 2d20
    -   Display both rolls and use appropriate one (higher for advantage, lower for disadvantage)
2.  **Multiple d20 Rolls with Statistics**
    
    -   Roll d20 multiple times (10-100 times)
    -   Use `for` loop to count how many times each number (1-20) appears
    -   Display distribution

3.  **Death Saving Throws**
    
    -   If HP reaches 0, enter death save mode
    -   Use `do-while` loop to roll death saves until 3 successes or 3 failures
    -   Track successes and failures using counter variables

----------

## Lesson 5: Arrays and Lists

### Mandatory Tasks

1.  **Create Ability Score Enum**
    
	 -   Create an enum: `enum Ability { Strength = 0, Dexterity = 1, Constitution = 2, Intelligence = 3, Wisdom = 4, Charisma = 5 }`
	-   This gives meaningful names to array indices!

2.  **Ability Names Array**
    
	-   Replace individual ability score variables with `int[] abilityScores = new int[6]`
	-   Use enum for indexing: `abilityScores[(int)Ability.Strength] = 15`
	-   Update all code that uses ability scores to use enum-based indexing
	
3.  **Equipment List**
    
    -   Create `List<string> equipment = new List<string>()`
    -   Add starting equipment based on class using `equipment.Add()`
    -   Fighter: "Longsword", "Shield", "Chain Mail"
    -   Wizard: "Spellbook", "Quarterstaff", "Component Pouch"
    -   Display all equipment using `foreach` loop
4.  **Inventory Management System**
    
    -   Create menu options: Add Item, Remove Item, View Inventory
    -   Use `List` methods: `Add()`, `Remove()`, `Contains()`
    -   Display all items with `for` loop showing index numbers
5.  **Skill Proficiencies Array**
    
	-   Create an array of tuples: `(string skillName, bool isProficient)[]`
	-   Access tuple elements with dot notation: `skills[i].skillName` and `skills[i].isProficient`
    -   Based on background, set some skills to true
    -   Display all skills showing which are proficient
6.  **Known Spells List**
    
    -   For Wizard and Cleric, create `List<string> knownSpells = new List<string>()`
    -   Add starting spells based on class
    -   Create Add Spell and Remove Spell options
    -   Use `foreach` to display all known spells
7.  **String Manipulation - Character Description**
    
    -   Create a `List<string> characterTraits = new List<string>()`
    -   Ask user to enter personality traits, ideals, bonds, flaws (one at a time)
    -   Use string methods: `ToUpper()`, `Trim()`, `Split()`
    -   Display character backstory by concatenating all traits
8.  **Multi-dimensional Array - Spell Slots**
    
    -   Create `int[,] spellSlots = new int[9, 2]`
    -   First dimension: spell level (0-8), Second dimension: [0]=max slots, [1]=current slots
    -   Initialize based on class and level
    -   Create methods to display and use spell slots

### Optional Tasks

1.  **Generic List Exercise**
    
    -   Create a `List<(string itemName, int quantity)>` for inventory with quantities
    -   Update inventory system to track quantities
    -   Allow stacking identical items
2.  **Conditions List**
    
    -   Create `List<string> activeConditions = new List<string>()`
    -   Add conditions: "Poisoned", "Blinded", "Charmed", etc.
    -   Create Add/Remove condition options
    -   Display all active conditions
3.  **String Search - Spell Lookup**
    
    -   Allow user to search for a spell by partial name
    -   Use `Contains()` or `StartsWith()` on the spells list
    -   Display all matching spells

----------

## Lesson 6: Classes and Structures

### Mandatory Tasks

1.  **Create Program.cs with Main Method**
    
    -   Create a proper `Program.cs` file with `class Program` and `static void Main(string[] args)`
    -   Move all top-level statement code into Main method

2.  **Create Character Class**
    
    -   Create `Character.cs` file in a `Models` folder
    -   Define `Character` class with public fields:
        -   `string Name`
        -   `string Race`
        -   `string Class`
        -   `int[] AbilityScores` (6 elements)
        -   `int CurrentHP`
        -   `int MaxHP`
        -   `int ArmorClass`
        -   `List<string> Equipment`
    -   Move from loose variables to a `Character character = new Character()`
3.  **Refactor Character Creation**
    
    -   Update character creation code to set properties on the Character object
    -   Instead of: `string name = "Bob"`
    -   Use: `character.Name = "Bob"`

4.  **Create Dice Structure**
    
    -   Create `Dice.cs` with a `struct Dice`
    -   Fields: `int Sides`, `int NumberOfDice`
    -   This represents dice notation (2d6 = NumberOfDice:2, Sides:6)
    -   Create a Dice instance for common rolls: d20, d6, d8, d10, d12
5.  **Create AbilityScores Struct**
    
    -   Create `AbilityScores.cs` with `struct AbilityScores`
    -   Fields: `int Strength, Dexterity, Constitution, Intelligence, Wisdom, Charisma`
    -   Update Character class to use `AbilityScores` instead of array
    -   This makes the code more readable!
6.  **Create Spell Class**
    
    -   Create `Spell.cs` in Models folder
    -   Fields: `string Name`, `int Level`, `string School`, `string Description`
    -   Update known spells to use `List<Spell>` instead of `List<string>`
7.  **Create ClassInfo Class**
    
    -   Create `ClassInfo.cs` with information about each D&D class
    -   Fields: `string ClassName`, `int HitDie`, `string PrimaryAbility`, `List<string> SavingThrows`
    -   Create static instances for each class: `ClassInfo.Fighter`, `ClassInfo.Wizard`, etc.
8.  **Organize Files**
    
    -   Create folder structure:
        -   `/Models` - Character, Spell, ClassInfo
        -   `/Structs` - Dice, AbilityScores
    -   Move all classes and structs to appropriate folders
    -   Update using statements in Program.cs

### Optional Tasks

1.  **Create Species Class**
    
    -   Similar to ClassInfo, create `Species` class
    -   Fields: `string SpeciesName`, `AbilityScores BonusScores`, `int Speed`, `List<string> Traits`
2.  **Create Equipment Class**
    
    -   Create `Equipment` class with fields: `string Name`, `EquipmentType Type`, `int Weight`
    -   Create enum `EquipmentType` with values: Weapon, Armor, Tool, Item
    -   Update inventory to use `List<Equipment>`
3.  **Create Skill Struct**
    
    -   Create struct with: `string SkillName`, `string LinkedAbility`, `bool IsProficient`
    -   Replace skill arrays with `Skill[] skills = new Skill[18]`
4.  **Create SpellSlots Class**
    
    -   Create class to manage spell slots: `int[] MaxSlots`, `int[] CurrentSlots`
    -   Add method to use slot: `public bool UseSlot(int level)`

----------

## Lesson 7: Methods and Functions

### Mandatory Tasks

1.  **Create DisplayCharacter Method**
    
    -   Create: `public void DisplayCharacter() ` method
    -   Include: Name, Race, Class, HP, AC, all ability scores with modifiers

2.  **Create CheckHealth Method**
    
    -   Create: `public void CheckHealth() ` method
    -   Check for characters wellbeing, show either:
	    - Pristine = If the character is at full health
	    - Healthy = If the character is damaged but has more than half hp left
	    - Bloodied = If the character is damaged and has less that half hp left
	    - Critical = If the character has less than tenth of their hp
	    - Dead = If the character has 0 hp

3.  **Character Class - TakeDamage Method**
    
    -   Add: `public void TakeDamage(int damage)`
    -   Reduce CurrentHP by damage amount
    -   Ensure HP doesn't go below 0

4.  **Create RollAbilityCheck Method**
    
    -   Add to Character: `public int RollAbilityCheck(string ability, string skill = null)`
    -   Roll d20 + ability modifier
    -   If skill provided and proficient, add proficiency bonus

5.  **Character Class - RollAttack Method**
    
    -   Add method to Character class: `public bool RollAttack(int AC, out int damage)`
    -   Roll d20 and add appropriate modifiers based on class
    -   Return the total attack roll

6.  **Create RollDice Method**
    
    -   In Program class, create: `static int RollDice(Dice dice)`
    -   Move dice rolling logic into this method
    -   Return the total of all dice rolled
    -   Update all dice rolls in code to use this method

7.  **Create Static Dice Helper Class**
    
    -   Create `DiceHelper` class with static methods:
    -   Move the `RollDice` method into the `DiceHelper`class
    -   `static int D20()` - rolls 1d20
    -   `static int D6(int count = 1)` - rolls count d6
    -   Similar for d4, d8, d10, d12
    -   Update code to use these convenient methods

8.  **Create Validation Helper Methods**
    
    -   Create: `static bool IsValidAbilityScore(int score)` - checks 3-20 range
    -   Create: `static bool IsValidLevel(int level)` - checks 1-20 range
    -   Use throughout code for validation

### Optional Tasks


1.  **Install Spectre.Console**
    
    -   Add Spectre.Console NuGet package to project
    -   Replace basic `Console.WriteLine` with `AnsiConsole.MarkupLine()` for colored output
    -   Add welcome banner using `AnsiConsole.Write(new FigletText("D&D Character Sheet"))`

2.  **Character Class - RollSavingThrow Method**
    
    -   Add: `public int RollSavingThrow(string ability)`
    -   Roll d20 + ability modifier
    -   If proficient in that save, add proficiency bonus
    -   Return total
5.  **Spell Class - Cast Method**
    
    -   Add: `public string Cast()` - returns a formatted casting description
    -   Use Spectre.Console markup for spell school colors
6.  **Create Progress Bar for Long Rest**
    
    -   When character takes a long rest, show Spectre.Console progress bar
    -   Restore HP and spell slots
    -   Animate the "resting" process

----------

## Lesson 8: Constructors

### Mandatory Tasks

1.  **Create a constructor for Skill struct**
    
    -   Create `public Skill(string name, bool isProficient, AbilityScore atribute)` constructor
    -   Fill `character.Skills` using the new constructor

2.  **Create Initialization function for character creation**
    
    -   Move existing character creation to a dedicated function
    -  Create subfunctions for huge blocks of code (Background bonuses, HP, Starting equipment...)
    -   Remove manual initialization from Program.cs

3.  **Character Default Constructor**
    
    -   Create: `public Character()` constructor
    -   Use initialization function to fill values 

4.  **Copy Constructor**
    
    -   Create: `public Character(Character other)`
    -   Creates a deep copy of another character
    -   Useful for "Clone Character" feature

5.  **Character Parameterized Constructor**
    
    -   Create: `public Character(string name, string race, string characterClass...)`
    -   Set basic properties
    -   Initialize collections
    -   Call helper methods to set race and class bonuses

6.  **Item class**
    - Add EquipmentType enum
    -   Add Item class
    - Add Item constructor
    -   `public Equipment(string name, EquipmentType type, int weight, int count)`
    
7.  **Random Character Creation**
    
    -   Create: `public void CreateRandom()`
    -   Randomly select background, race and class (`Enum.GetValues<Background>().Length`)
    -   Ask user to specify Name and Level, other parameters should be generated randomly
    -   Roll 4d6 drop lowest for each AbilityScore
8.  **Add character creation menu**
    -   Add menu options:
        -   Create Character (Manual)
        -   Create Character (Random)
    -   Each option calls appropriate factory method

### Optional Tasks

1.  **Standard Array Constructor**
    
    -   Create: `public static Character CreateWithStandardArray(...)`
    -   Uses standard array (15, 14, 13, 12, 10, 8)
    -   Player assigns which ability gets which score
2.  **Constructor Chaining**
    
    -   Demonstrate constructor chaining with `: this(...)`
    -   Create constructor that calls another constructor with default values
3.  **Level-Appropriate Constructor**
    
    -   Create: `public Character(string name, string race, string characterClass, int level)`
    -   Calculate HP based on level
    -   Set appropriate spell slots
    -   Add level-appropriate equipment
4.  **Wizard with Spellbook Constructor**
    
    -   Create specialized constructor for Wizard
    -   Automatically add starting spells to spellbook
    -   Initialize spell slots

----------

## Lesson 9: Access Modifiers & Encapsulation

### Mandatory Tasks

1.  **Private Fields in Character**
    
    -   Convert all public fields in Character to private
    -   Rename with underscore prefix: `private string _name;`
    -   Students will see the code break - this shows why we need public access!
2.  **Public Properties for Basic Fields**
    
    -   Create public properties for Name, Race, Class
    -   Example: `public string Name { get => _name; set => _name = value; }`
    -   Fix all compilation errors
3.  **HP with Validation**
    
    -   Create private `_currentHP` and `_maxHP`
    -   Create public property:
    
    ```csharp
    public int CurrentHP 
    { 
        get => _currentHP;
        set => _currentHP = Math.Clamp(value, 0, _maxHP);
    }
    
    ```
    
    -   This prevents HP from going out of range automatically!
4.  **Ability Score Properties**
    
    -   Create private `_abilityScores` array
    -   Create individual properties:
    >!  `public int Strength { get => _abilityScores[0]; set => _abilityScores[0] = Math.Clamp(value, 1, 30); }`
    -   Similar for all six abilities
    -   This makes code more readable than array indexing!
5.  **Read-Only Modifier Properties**
    
    -   Create calculated properties without setters:
    -   `public int StrengthModifier => CalculateModifier(Strength);`
    -   Similar for all ability modifiers
    -   These can't be set, only calculated from ability scores
6.  **Property with Logic - Level**
    
    -   Create `_level` private field
    -   Create Level property that updates dependent stats when changed:
    
    ```csharp
    public int Level 
    { 
        get => _level;
        set 
        {
            _level = Math.Clamp(value, 1, 20);
            UpdateProficiencyBonus();
            UpdateMaxHP();
        }
    }
    
    ```
    
7.  **Proficiency Bonus as Read-Only Property**
    
    -   Create: `public int ProficiencyBonus => 2 + (Level - 1) / 4;`
    -   Remove any manual proficiency tracking
    -   Always calculated from level
8.  **Equipment as Read-Only List**
    
    -   Make equipment list private: `private List<Equipment> _equipment`
    -   Provide public property: `public IReadOnlyList<Equipment> Equipment => _equipment.AsReadOnly();`
    -   Provide methods: `public void AddEquipment(Equipment item)`, `public void RemoveEquipment(Equipment item)`
    -   This prevents external code from modifying the list directly

### Optional Tasks

1.  **Initiative Property**
    
    -   Create: `public int Initiative => 10 + DexterityModifier;`
    -   Read-only property that calculates initiative bonus
2.  **Passive Perception Property**
    

-   Create: `public int PassivePerception => 10 + WisdomModifier + (IsSkillProficient("Perception") ? ProficiencyBonus : 0);`

3.  **Spell Save DC Property**

-   For spellcasters: `public int SpellSaveDC => 8 + ProficiencyBonus + GetSpellcastingModifier();`

4.  **Private Helper Methods**
    
    -   Create private methods that are only used internally:
    -   `private void UpdateProficiencyBonus()`
    -   `private void UpdateMaxHP()`
    -   `private int GetSpellcastingModifier()`
    -   These should not be accessible outside the class
5.  **Spell Slots with Encapsulation**
    
    -   Create private spell slots array
    -   Public methods: `public bool HasSpellSlot(int level)`, `public void UseSpellSlot(int level)`, `public void RestoreSpellSlots()`
    -   Property: `public int[] AvailableSpellSlots => _currentSlots.ToArray();` (returns copy)
6.  **Auto-Properties where Appropriate**
    
    -   Use auto-properties for simple fields: `public string Background { get; set; }`
    -   Discuss when to use auto-properties vs full properties with backing fields

----------

## Lesson 10: Subject summary - Spell system

1. ** Create the MagicSchool Enumeration**

- Create an enum called `MagicSchool` that represents the 8 schools of magic in D&D 5.5e plus one additional "Universal" category.
-   The enum should contain these values: Abjuration, Conjuration, Divination, Enchantment, Evocation, Illusion, Necromancy, Transmutation, Universal

2. **Create the Spell Struct with Private Fields**
-   Private fields: `title` (string), `level` (int), `school` (MagicSchool), `description` (string)
-   Constructor: `Spell(string title, int level, MagicSchool school, string description = "")`
-   Constructor validation: Throw an `ArgumentException` if level is not between 0-9
-   Public read-only properties: `Title`, `Level`, `School`, `Description`
-   Add a computed property `IsCantrip` that returns true if level equals 0

3. **Add GetFormattedInfo Method to Spell Struct**
Add a public method to the `Spell` struct that returns a formatted string with spell information.
-   Method name: `GetFormattedInfo()`
-   Return type: `string`
-   Format: "{Title} ({Level}, {School})" where Level should display as "Cantrip" if level is 0, otherwise "Level X"
-   Example output: "Fireball (Level 3, Evocation)" or "Fire Bolt (Cantrip, Evocation)"

4. **Add Multidimensional Array for Spell Slots**
Add a multidimensional array to your `Character` class to track spell slots.
-   Add a private field: `int[,] spellSlots = new int[2, 9];`
-   First dimension [0, x] stores maximum spell slots
-   First dimension [1, x] stores current (remaining) spell slots
-   Array indices 0-8 represent spell levels 1-9 (cantrips don't use slots)
-   Add public methods:
    -   `GetMaxSpellSlots(int spellLevel)` - returns max slots for given level
    -   `GetCurrentSpellSlots(int spellLevel)` - returns current slots for given level
    -   Both methods should return 0 if spellLevel is invalid (<1 or >9)
5. **Initialize Spell Slots Based on Character Level**
Create a method that initializes spell slots based on character level using full caster progression.
-   Method name: `InitializeSpellSlots()`
-   Reset all spell slots to 0 first
-   Set spell slots according to full caster progression, use following function:
`public int[] GetSpellSlot() => this.Level switch  
{  
  1 => [2, 0, 0, 0, 0, 0, 0, 0, 0],  
  2 => [3, 0, 0, 0, 0, 0, 0, 0, 0],  
  3 => [4, 2, 0, 0, 0, 0, 0, 0, 0],  
  4 => [4, 3, 0, 0, 0, 0, 0, 0, 0],  
  5 => [4, 3, 2, 0, 0, 0, 0, 0, 0],  
  6 => [4, 3, 3, 0, 0, 0, 0, 0, 0],  
  7 => [4, 3, 3, 1, 0, 0, 0, 0, 0],  
  8 => [4, 3, 3, 2, 0, 0, 0, 0, 0],  
  9 => [4, 3, 3, 3, 1, 0, 0, 0, 0],  
  10 => [4, 3, 3, 3, 2, 0, 0, 0, 0],  
  11 => [4, 3, 3, 3, 2, 1, 0, 0, 0],  
  12 => [4, 3, 3, 3, 2, 1, 0, 0, 0],  
  13 => [4, 3, 3, 3, 2, 1, 1, 0, 0],  
  14 => [4, 3, 3, 3, 2, 1, 1, 0, 0],  
  15 => [4, 3, 3, 3, 2, 1, 1, 1, 0],  
  16 => [4, 3, 3, 3, 2, 1, 1, 1, 0],  
  17 => [4, 3, 3, 3, 2, 1, 1, 1, 1],  
  18 => [4, 3, 3, 3, 3, 1, 1, 1, 1],  
  19 => [4, 3, 3, 3, 3, 2, 1, 1, 1],  
  20 => [4, 3, 3, 3, 3, 2, 2, 1, 1],  
  _ => [0, 0, 0, 0, 0, 0, 0, 0, 0],  
};`

-   After setting max slots, copy them to current slots (full rest state)

6. **Add List of Known Spells to Character**
Add a list to store spells that the character knows.
-   Add a private field: `List<Spell> knownSpells = new List<Spell>();`
-   Add a method `LearnSpell(Spell newSpell)` that:
    -   Adds the spell to the list
    -   Prints a confirmation message: "Learned spell: {spell.GetFormattedInfo()}"
-   Add a public property to get the count: `public int SpellCount => knownSpells.Count;`

7. **Prevent Learning Duplicate Spells**
Modify the `LearnSpell` method to prevent learning the same spell twice.
-   Before adding the spell, check if a spell with the same title already exists in `knownSpells`
-   Use LINQ's `Any()` method: `knownSpells.Any(s => s.Title == newSpell.Title)`
-   If spell already exists, print "Already know {spell.Title}!" and return without adding
-   Otherwise, add the spell normally

8. **Implement Basic Spell Casting**
Create a method that allows casting spells and manages spell slots.
-   Method signature: `public bool CastSpell(string spellTitle)`
-   Find the spell in `knownSpells` using LINQ: `knownSpells.FirstOrDefault(s => s.Title == spellTitle)`
-   If spell not found, print error message and return false
-   If spell is a cantrip, print "Casting {title} (Cantrip)" and return true (no slot needed)
-   If spell requires slots:
    -   Check if current slots for that level > 0
    -   If no slots available, print "No level {X} spell slots remaining!" and return false
    -   If slots available, reduce current slots by 1 and return true
    -   Print: "Casting {title}! Remaining level {X} slots: {Y}"

9. **Implement Spell Upcasting**
 Extend the `CastSpell` method to allow casting spells using higher-level spell slots.
-   Change method signature: `public bool CastSpell(string spellTitle, int usingSlotLevel = -1)`
-   If `usingSlotLevel` is -1, use the spell's normal level
-   If `usingSlotLevel` is specified:
    -   Check that it's not lower than the spell's level (can't downcast)
    -   If trying to downcast, print error and return false
-   Use the specified slot level when checking and consuming slots
-   When casting with higher slot, print: "Casting {title} (upcast at level {X})!"

10. **LINQ Query - Get Spells by School**
Create a method that returns all spells of a specific magic school.
-   Method signature: `public List<Spell> GetSpellsBySchool(MagicSchool school)`
-   Use LINQ `Where()` to filter spells
-   Return the filtered list

11. **LINQ Query - Get Spells by Level**
Create a method that returns all spells of a specific level.
-   Method signature: `public List<Spell> GetSpellsByLevel(int level)`
-   Use LINQ `Where()` to filter spells by level
-   Return the filtered list

12. **LINQ Query - Sort Spells by Level**
Create a method that returns spells sorted by level, then alphabetically by title.
-   Method signature: `public List<Spell> GetSpellsSortedByLevel()`
-   Use LINQ `OrderBy()` to sort by level first
-   Use `ThenBy()` to sort alphabetically by title second
-   Return the sorted list

13. **LINQ Query - Get Castable Spells**
Create a method that returns only spells that can currently be cast (have slots or are cantrips).
-   Method signature: `public List<Spell> GetCastableSpells()`
-   Use LINQ `Where()` with a condition that checks:
    -   Either the spell is a cantrip (IsCantrip is true)
    -   OR the character has available spell slots for that level (GetCurrentSpellSlots(spell.Level) > 0)
-   Return the filtered list

14. **LINQ Query - Search Spells by Name**
Create a method that searches for spells by partial name match (case-insensitive).
-   Method signature: `public List<Spell> SearchSpellsByName(string searchTerm)`
-   Use LINQ `Where()` with `Contains()` method
-   Use `StringComparison.OrdinalIgnoreCase` for case-insensitive search
-   Return the filtered list

15. **LINQ Query - Get Highest Known Spell Level**
Create a method that returns the highest level spell the character knows.
-   Method signature: `public int GetHighestKnownSpellLevel()`
-   If `knownSpells.Count == 0`, return 0
-   Use LINQ `Max()` to find the highest spell level
-   Return the result

16. **LINQ Query - Count Spells by School**
Create a method that returns a dictionary showing how many spells of each school the character knows.
-   Method signature: `public Dictionary<MagicSchool, int> GetSpellCountBySchool()`
-   Use LINQ `GroupBy()` to group spells by school
-   Use `ToDictionary()` to convert to dictionary
-   Dictionary key should be MagicSchool, value should be count

17. **Display Spell Slots**
Create a method that displays spell slots in a visual format.
-   Method signature: `public void DisplaySpellSlots()`
-   Show only spell levels that have slots (max > 0)
-   Use filled circles (●) for available slots
-   Use empty circles (○) for used slots
-   Display in format: "Level X: ●●●○○ (3/5)"

**Expected Output Format:**
=== SPELL SLOTS ===
Level 1: ●●○○ (2/4)
Level 2: ●●● (3/3)
Level 3: ○○ (0/2)

18. **Display Spellbook**
Create a method that displays all known spells in an organized format.
-   Method signature: `public void DisplaySpellbook()`
-   Display cantrips in a separate section
-   Display leveled spells grouped by level (1-9)
-   For each spell level, show current/max slots
-   Sort spells alphabetically within each section
-   Display total spell count at the end

**Expected Output Format:**
=== SPELLBOOK ===

CANTRIPS:
  • Fire Bolt (Evocation)
  • Mage Hand (Conjuration)

LEVEL 1 SPELLS (Slots: ●●○○ (2/4)):
  • Magic Missile (Evocation)
  • Shield (Abjuration)

LEVEL 3 SPELLS (Slots: ○○ (0/2)):
  • Fireball (Evocation)

Total Spells Known: 5

19. **Implement Long Rest**
Create a method that restores all spell slots and hit points.
-   Method signature: `public void LongRest()`
-   Copy all max spell slots to current spell slots (restore all slots)
-   Set CurrentHitPoints to MaxHitPoints
-   Print confirmation message: "Completed long rest. HP and spell slots fully restored!"

## Bonus: Finishing & Serialization

### Mandatory Tasks

1.  **Install System.Text.Json**
    
    -   Add System.Text.Json NuGet package (or use built-in if .NET 9)
    -   Add using statement: `using System.Text.Json;`
2.  **Make Character Serializable**
    
    -   Add `[Serializable]` attribute to Character class
    -   Ensure all properties can be serialized
    -   Test with: `string json = JsonSerializer.Serialize(character);`
3.  **Save Character to File**
    
    -   Create method: `public static void SaveCharacter(Character character, string filename)`
    -   Serialize character to JSON
    -   Write to file using `File.WriteAllText()`
    -   Add error handling with try-catch
    -   Save to "characters" folder
4.  **Load Character from File**
    
    -   Create: `public static Character LoadCharacter(string filename)`
    -   Read JSON from file
    -   Deserialize using `JsonSerializer.Deserialize<Character>()`
    -   Add error handling for file not found, invalid JSON
5.  **Character Selection Menu**
    
    -   Create method to list all saved characters in folder
    -   Use `Directory.GetFiles("characters", "*.json")`
    -   Display list using Spectre.Console selection prompt
    -   Allow user to load a character
6.  **Delete Character Option**
    
    -   Add menu option to delete a saved character
    -   Show list of characters
    -   Confirm deletion with user
    -   Delete file using `File.Delete()`
7.  **Polish Main Menu**
    
    -   Final menu structure:
        -   New Character (submenu: Manual, Point Buy, Random, Standard Array)
        -   Load Character
        -   Character Actions (submenu: View, Roll Check, Combat, Level Up, Delete)
        -   Exit
    -   Use Spectre.Console panels and colors throughout
8.  **Application Testing**
    
    -   Create a test character
    -   Save it
    -   Close program
    -   Restart program
    -   Load character
    -   Verify all data is preserved

### Optional Tasks

1.  **Character Leveling System**
    
    -   Add "Level Up" option
    -   Increase level
    -   Roll for HP increase (class hit die + CON modifier)
    -   Update spell slots if spellcaster
    -   Use Spectre.Console to show level-up effects
2.  **Multiple Character Management**
    
    -   Save multiple characters
    -   Create a "party" that can have up to 4 characters
    -   Switch between characters
    -   Display party summary
3.  **Export Character to Text**
    
    -   Create formatted text export (not JSON)
    -   Create a readable character sheet in .txt format
    -   Include all character information in D&D character sheet format
4.  **Combat Tracker**
    
    -   Create a full combat system
    -   Roll initiative for party and enemies
    -   Track HP for all combatants
    -   Roll attacks and damage
    -   Track spell slots used in combat
    -   Use Spectre.Console to display combat beautifully
5.  **Spell Database**
    
    -   Create a JSON file with many D&D spells
    -   Load spell database on startup
    -   Allow Wizards to learn spells from the database
    -   Search and filter spells
6.  **Character Backup System**
    
    -   Auto-save character periodically
    -   Create backup folder with timestamped saves
    -   Allow restoring from backup if something goes wrong
7.  **Statistics and Achievements**
    
    -   Track character statistics: total damage dealt, checks made, nat 20s rolled
    -   Save statistics with character
    -   Display statistics in character summary
8.  **Settings File**
    
    -   Create a settings.json file
    -   Save user preferences: theme, auto-save interval, dice animation speed
    -   Load settings on startup
9.  **Class-Specific Features**
    
    -   Add class-specific resources:
        -   Fighter: Second Wind, Action Surge
        -   Wizard: Arcane Recovery
        -   Rogue: Sneak Attack dice
        -   Cleric: Channel Divinity
    -   Track uses per rest
----------
