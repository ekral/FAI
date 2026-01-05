using CharacterSheet.Helpers;
using CharSheet;

namespace CharacterSheet.Models;

public class Character
{
    private string _name;
    private Species _race;
    private CharacterClass _characterClass;
    private Background _background;
    private int _level;
    private int[] _abilityScores;
    private int _currentHp;
    private int _maxHp;
    private int _armorClass;
    private List<string> _equipment;
    private List<Skill> _skills;
    private int[,] spellSlots = new int[2, 9];
    private List<Spell> knownSpells = new List<Spell>();

    public bool LearnSpell(Spell newSpell)
    {
        if (knownSpells.Any(s => s.title == newSpell.title))
        {
            Console.WriteLine("You already know this spell");
            return false;
        }
        
        knownSpells.Add(newSpell);
        Console.WriteLine("Learned spell: {spell.GetFormattedInfo()}");
        return true;
        
    }
    
    public int SpellCount => knownSpells.Count;

    public int GetMaxSpellSlots(int spellLevel)
    {
        return spellLevel >= 1 && spellLevel <= 9 ? spellSlots[0, spellLevel - 1] : 0;
    }

    public int GetCurrentSpellSlots(int spellLevel)
    {
        return spellLevel >= 1 && spellLevel <= 9 ? spellSlots[1, spellLevel - 1] : 0;
    }

    public int CurrentHp
    {
        get => _currentHp;
        set => _currentHp = Math.Clamp(value, 0, _maxHp);
    }

    public int Strength => _abilityScores[(int)AbilityScore.Strength];
    public int StrengthModifier => Mod(Strength);
    public int Dexterity => _abilityScores[(int)AbilityScore.Dexterity];
    public int DexterityModifier => Mod(Dexterity);
    public int Constitution => _abilityScores[(int)AbilityScore.Constitution];
    public int ConstitutionModifier => Mod(Constitution);
    public int Intelligence => _abilityScores[(int)AbilityScore.Intelligence];
    public int IntelligenceModifier => Mod(Intelligence);
    public int Wisdom => _abilityScores[(int)AbilityScore.Wisdom];
    public int WisdomModifier => Mod(Wisdom);
    public int Charisma => _abilityScores[(int)AbilityScore.Charisma];
    public int CharismaModifier => Mod(Charisma);
    public int Proficiency => ProficiencyBonus(_level);

    public void InitializeSpellSlots()
    {
        int[] Template = GetSpellSlots();
            
        for (int i = 0; i < Template.Length; i++)
        {
          spellSlots[0,i]= Template[i];
          spellSlots[1, i] = Template[i];
        }
    }



    public List<string> GetEquipment() => new(_equipment);
    public void AddItemToEquipment(string item) => _equipment.Add(item);
    public bool RemoveItemFromEquipment(string item) => _equipment.Remove(item);


    public Character()
    {
        Console.WriteLine("1 - Create character manually, 2 - Create random character");
        switch (Console.ReadLine())
        {
            default:
                InitializeCharacter();
                break;
            case "2":
                RandomizeCharacter();
                break;
        }
    }

    public Character(Character other)
    {
        this._abilityScores = new int [6];
        this._name = other._name;
        this._race = other._race;
        this._characterClass = other._characterClass;
        this._background = other._background;
        this._level = other._level;
        other._abilityScores.CopyTo(this._abilityScores, 0);
        this._currentHp = other._currentHp;
        this._maxHp = other._maxHp;
        this._armorClass = other._armorClass;
        this._equipment = new List<string>(other._equipment);
        this._skills = new List<Skill>(other._skills);
    }

    public void InitializeCharacter()
    {
        this._abilityScores = new int [6];
        this._equipment = new List<string>();
        this._skills = new List<Skill>();

        Console.Write("Name: ");
        this._name = Console.ReadLine();

        Console.Write("Background(Criminal,Farmer,Soldier,Sage...): ");
        string? backgroundInput = Console.ReadLine();
        // Switch expression - Mapping string to Background enum
        this._background = backgroundInput?.ToLower() switch
        {
            "acolyte" => Background.Acolyte,
            "artisan" => Background.Artisan,
            "charlatan" => Background.Charlatan,
            "criminal" => Background.Criminal,
            "entertainer" => Background.Entertainer,
            "farmer" => Background.Farmer,
            "guard" => Background.Guard,
            "guide" => Background.Guide,
            "hermit" => Background.Hermit,
            "merchant" => Background.Merchant,
            "noble" => Background.Noble,
            "pilgrim" => Background.Pilgrim,
            "sage" => Background.Sage,
            "scribe" => Background.Scribe,
            "sailor" => Background.Sailor,
            "soldier" => Background.Soldier,
            "wayfarer" => Background.Wayfarer,
            _ => Background.Other,
        };

        Console.Write("Race (Human, Elf, Dwarf, Halfling, Orc...): ");
        this._race = Enum.TryParse(Console.ReadLine(), true, out Species r) ? r : Species.Human;

        Console.Write("Class (Barbarian, Fighter, Paladin, Ranger...): ");
        this._characterClass = Enum.TryParse(Console.ReadLine(), true, out CharacterClass c)
            ? c
            : CharacterClass.Fighter;

        Console.Write("Level: ");
        this._level = int.TryParse(Console.ReadLine(), out int l) && l > 0 ? l : 1;


        // Proficiency calculation
        if (this._level > 20)
        {
            Console.WriteLine("You are frankly too powerful for this world... Begone!");
            return;
        }


        this._skills.Add(new Skill("Survival", true, AbilityScore.Wisdom));
        this._skills.Add(new Skill("Persuasion", false, AbilityScore.Charisma));
        this._skills.Add(new Skill(
            isProficient: true,
            name: "Perception",
            atribute: AbilityScore.Wisdom));
        foreach (var skill in this._skills)
        {
            if (skill.isProficient) Console.WriteLine(skill.name);
        }

        // Ability Scores
        var abilityNames = Enum.GetNames(typeof(AbilityScore));
        for (int i = 0; i < 6; i++)
        {
            this._abilityScores[i] = ReadInt(abilityNames[i] + ": ");
        }

        // Basic Armor Class calculation
        this._armorClass = 10 + DexterityModifier;

        // Background bonuses

        SetBgBonuses();
        SetStarterHp();
        SetStarterEquipment();
    }

    public void RandomizeCharacter()
    {
        this._abilityScores = new int [6];
        this._equipment = new List<string>();
        this._skills = new List<Skill>();

        Console.Write("Name: ");
        this._name = Console.ReadLine();

        this._background = (Background)DiceHelpers.SelectEnumOption(Enum.GetValues<Background>().Length);
        this._race = (Species)DiceHelpers.SelectEnumOption(Enum.GetValues<Species>().Length);
        this._characterClass = (CharacterClass)DiceHelpers.SelectEnumOption(Enum.GetValues<CharacterClass>().Length);

        Console.Write("Level: ");
        this._level = int.TryParse(Console.ReadLine(), out int l) && l > 0 ? l : 1;


        // Proficiency calculation
        if (this._level > 20)
        {
            Console.WriteLine("You are frankly too powerful for this world... Begone!");
            return;
        }


        this._skills.Add(new Skill("Survival", true, AbilityScore.Wisdom));
        this._skills.Add(new Skill("Persuasion", false, AbilityScore.Charisma));
        this._skills.Add(new Skill(
            isProficient: true,
            name: "Perception",
            atribute: AbilityScore.Wisdom));
        foreach (var skill in this._skills)
        {
            if (skill.isProficient) Console.WriteLine(skill.name);
        }

        // Ability Scores
        var abilityNames = Enum.GetNames(typeof(AbilityScore));
        for (int i = 0; i < 6; i++)
        {
            this._abilityScores[i] = DiceHelpers.RollAbilityScore();
        }

        // Basic Armor Class calculation
        this._armorClass = 10 + DexterityModifier;

        // Background bonuses

        SetBgBonuses();
        SetStarterHp();
        SetStarterEquipment();
    }

    public void SetStarterEquipment()
    {
        switch (this._characterClass)
        {
            default:
                Console.WriteLine("You get nothing :(");
                break;
            case CharacterClass.Fighter:
                this._equipment.Add("Longsword");
                this._equipment.Add("Shield");
                this._equipment.Add("Chain Mail");
                break;
            case CharacterClass.Wizard:
                this._equipment.AddRange("Spellbook", "Quarterstaff", "Component Pouch");
                break;
        }
    }

    public void SetStarterHp()
    {
        if (this._characterClass == CharacterClass.Barbarian)
            this._maxHp = (12 + ConstitutionModifier) * this._level;
        else if (this._characterClass is CharacterClass.Fighter or CharacterClass.Paladin or CharacterClass.Ranger)
            this._maxHp = (10 + ConstitutionModifier) * this._level;
        else if (this._characterClass is CharacterClass.Sorcerer or CharacterClass.Wizard)
            this._maxHp = (6 + ConstitutionModifier) * this._level;
        else
            this._maxHp = (8 + ConstitutionModifier) * this._level;

        this._currentHp = this._maxHp;
    }


    public void SetBgBonuses()
    {
        switch (this._background)
        {
            case Background.Acolyte:
                this._abilityScores[(int)AbilityScore.Intelligence]++;
                this._abilityScores[(int)AbilityScore.Wisdom]++;
                this._abilityScores[(int)AbilityScore.Charisma]++;
                break;
            case Background.Artisan:
                this._abilityScores[(int)AbilityScore.Strength]++;
                this._abilityScores[(int)AbilityScore.Dexterity]++;
                this._abilityScores[(int)AbilityScore.Intelligence]++;
                break;
            case Background.Charlatan:
                this._abilityScores[(int)AbilityScore.Dexterity]++;
                this._abilityScores[(int)AbilityScore.Constitution]++;
                this._abilityScores[(int)AbilityScore.Charisma]++;
                break;
            case Background.Criminal:
                this._abilityScores[(int)AbilityScore.Dexterity]++;
                this._abilityScores[(int)AbilityScore.Constitution]++;
                this._abilityScores[(int)AbilityScore.Intelligence]++;
                break;
            case Background.Entertainer:
                this._abilityScores[(int)AbilityScore.Strength]++;
                this._abilityScores[(int)AbilityScore.Dexterity]++;
                this._abilityScores[(int)AbilityScore.Charisma]++;
                break;
            case Background.Farmer:
                ++this._abilityScores[(int)AbilityScore.Strength];
                this._abilityScores[(int)AbilityScore.Constitution]++;
                this._abilityScores[(int)AbilityScore.Wisdom]++;
                break;
            case Background.Guard:
                this._abilityScores[(int)AbilityScore.Strength]++;
                this._abilityScores[(int)AbilityScore.Intelligence]++;
                this._abilityScores[(int)AbilityScore.Wisdom]++;
                break;
            case Background.Guide:
                this._abilityScores[(int)AbilityScore.Dexterity]++;
                this._abilityScores[(int)AbilityScore.Constitution]++;
                this._abilityScores[(int)AbilityScore.Wisdom]++;
                break;
            case Background.Hermit:
                this._abilityScores[(int)AbilityScore.Constitution]++;
                this._abilityScores[(int)AbilityScore.Wisdom]++;
                this._abilityScores[(int)AbilityScore.Charisma]++;
                break;
            case Background.Merchant:
                this._abilityScores[(int)AbilityScore.Constitution]++;
                this._abilityScores[(int)AbilityScore.Intelligence]++;
                this._abilityScores[(int)AbilityScore.Charisma]++;
                break;
            case Background.Noble:
                ++this._abilityScores[(int)AbilityScore.Strength];
                this._abilityScores[(int)AbilityScore.Intelligence]++;
                this._abilityScores[(int)AbilityScore.Charisma]++;
                break;
            case Background.Pilgrim:
                this._abilityScores[(int)AbilityScore.Wisdom]++;
                this._abilityScores[(int)AbilityScore.Wisdom]++;
                this._abilityScores[(int)AbilityScore.Constitution]++;
                break;
            case Background.Sage:
                this._abilityScores[(int)AbilityScore.Constitution]++;
                this._abilityScores[(int)AbilityScore.Intelligence]++;
                this._abilityScores[(int)AbilityScore.Wisdom]++;
                break;
            case Background.Sailor:
                this._abilityScores[(int)AbilityScore.Strength]++;
                this._abilityScores[(int)AbilityScore.Dexterity]++;
                this._abilityScores[(int)AbilityScore.Wisdom]++;
                break;
            case Background.Scribe:
                this._abilityScores[(int)AbilityScore.Intelligence]++;
                this._abilityScores[(int)AbilityScore.Dexterity]++;
                this._abilityScores[(int)AbilityScore.Wisdom]++;
                break;
            case Background.Soldier:
                ++this._abilityScores[(int)AbilityScore.Strength];
                this._abilityScores[(int)AbilityScore.Constitution]++;
                this._abilityScores[(int)AbilityScore.Dexterity]++;
                break;
            case Background.Wayfarer:
                this._abilityScores[(int)AbilityScore.Dexterity]++;
                this._abilityScores[(int)AbilityScore.Wisdom]++;
                this._abilityScores[(int)AbilityScore.Charisma]++;
                break;
            default:
                Console.WriteLine("Error processing background!");
                break;
        }
    }

    public void DisplayCharacter()
    {
        Console.WriteLine(
            $"{this._name}, {this._race}, {this._characterClass}, {this._background}, Level {this._level}");
        Console.WriteLine(
            $"STR {this.Strength} ({Program.Fmt(StrengthModifier)}), DEX {Dexterity} ({Program.Fmt(DexterityModifier)}), CON {Constitution} ({Program.Fmt(ConstitutionModifier)})");
        Console.WriteLine(
            $"INT {Intelligence} ({Program.Fmt(IntelligenceModifier)}), WIS {Wisdom} ({Program.Fmt(WisdomModifier)}), CHA {Charisma} ({Program.Fmt(CharismaModifier)})");
        Console.WriteLine($"Armor Class: {this._armorClass}");
        Console.WriteLine($"Max Hit Points: {this._maxHp}");
        Console.WriteLine($"Current Hit Points: {this._currentHp}");
        Console.WriteLine("Proficiency: " + this.Proficiency);
        Console.WriteLine("Equipment:");
        foreach (var item in this._equipment)
        {
            Console.Write(item + ", ");
        }

        Console.WriteLine();
    }

    public void CheckHealth()
    {
        if (this._currentHp == this._maxHp)
        {
            Console.WriteLine("Character is pristine.");
        }
        else if (this._currentHp > this._maxHp / 2)
        {
            Console.WriteLine("Character is healthy.");
        }
        else if (this._currentHp == 0)
        {
            Console.WriteLine("Character is dead.");
        }
        else if (this._currentHp < this._maxHp / 10)
        {
            Console.WriteLine("Character is critical.");
        }
        else
        {
            Console.WriteLine("Character is bloodied.");
        }
    }

    public void TakeDamage(int damage)
    {
        if ((this._currentHp - damage) <= 0)
        {
            this._currentHp = 0;
        }

        else if ((this._currentHp - damage) >= this._maxHp)
        {
            this._currentHp = this._maxHp;
        }
        else
        {
            this._currentHp = this._currentHp - damage;
        }

        this.CheckHealth();
    }

    public int GetAbilityModifier(AbilityScore score) => score switch
    {
        AbilityScore.Strength => StrengthModifier,
        AbilityScore.Charisma => CharismaModifier,
        AbilityScore.Constitution => ConstitutionModifier,
        AbilityScore.Dexterity => DexterityModifier,
        AbilityScore.Intelligence => IntelligenceModifier,
        AbilityScore.Wisdom => WisdomModifier
    };

    public int RollCheck(int abilityModifier) =>
        (DiceHelpers.RollD20() + GetAbilityModifier((AbilityScore)abilityModifier));

    public bool RollAttack(int AC, out int damage)
    {
        var rand = new Random();
        int roll = 0;
        damage = 0;

        roll = DiceHelpers.RollD20();

        if ((roll + this.Proficiency + StrengthModifier) >= AC)
        {
            damage = DiceHelpers.RollDice(dicePool: 3, sides: 6);
            return true;
        }
        else
        {
            return false;
        }
    }

    public int[] GetSpellSlots() => this._level switch
    {
        1 => [2, 0, 0, 0, 0, 0, 0, 0, 0], 2 => [3, 0, 0, 0, 0, 0, 0, 0, 0], 3 => [4, 2, 0, 0, 0, 0, 0, 0, 0],
        4 => [4, 3, 0, 0, 0, 0, 0, 0, 0], 5 => [4, 3, 2, 0, 0, 0, 0, 0, 0], 6 => [4, 3, 3, 0, 0, 0, 0, 0, 0],
        7 => [4, 3, 3, 1, 0, 0, 0, 0, 0], 8 => [4, 3, 3, 2, 0, 0, 0, 0, 0], 9 => [4, 3, 3, 3, 1, 0, 0, 0, 0],
        10 => [4, 3, 3, 3, 2, 0, 0, 0, 0], 11 => [4, 3, 3, 3, 2, 1, 0, 0, 0], 12 => [4, 3, 3, 3, 2, 1, 0, 0, 0],
        13 => [4, 3, 3, 3, 2, 1, 1, 0, 0], 14 => [4, 3, 3, 3, 2, 1, 1, 0, 0], 15 => [4, 3, 3, 3, 2, 1, 1, 1, 0],
        16 => [4, 3, 3, 3, 2, 1, 1, 1, 0], 17 => [4, 3, 3, 3, 2, 1, 1, 1, 1], 18 => [4, 3, 3, 3, 3, 1, 1, 1, 1],
        19 => [4, 3, 3, 3, 3, 2, 1, 1, 1], 20 => [4, 3, 3, 3, 3, 2, 2, 1, 1], _ => [0, 0, 0, 0, 0, 0, 0, 0, 0],
    };

    static int ReadInt(string label)
    {
        Console.Write(label);
        return int.TryParse(Console.ReadLine(), out int v) ? v : 10;
    }

    static int Mod(int score) => (score - 10) / 2;
    static int ProficiencyBonus(int level) => 2 + (level - 1) / 4;
}