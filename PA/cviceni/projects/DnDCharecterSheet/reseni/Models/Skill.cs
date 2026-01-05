using CharacterSheet.Helpers;

namespace CharacterSheet.Models;

public struct Skill
{
    public string name;
    public bool isProficient;
    public AbilityScore atribute;

    public Skill(string name, bool isProficient, AbilityScore atribute)
    {
        this.name = name;
        this.isProficient = isProficient;
        this.atribute = atribute;
    }
}