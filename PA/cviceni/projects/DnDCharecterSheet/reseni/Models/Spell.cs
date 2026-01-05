using CharacterSheet.Helpers;

namespace CharacterSheet.Models;

public struct Spell
{
    public string title { get; }
    public int level { get; }
    public MagicSchool school { get; }
    public string description { get; }
    public bool IsCantrip => level==0;
    public Spell(string title, int level, MagicSchool school, string description = "")
    {
        if(level<=9 && level>=0)
        { 
        this.title = title;
        this.level = level;
        this.school = school;
        this.description = description;
        }
        else
        {
            throw new ArgumentException("Invalid level");
        }
    }

    public string GetFormattedInfo() =>
        IsCantrip ? $"{title} (Cantrip, {school})" : $"{title} (Level: {level}, {school})";
}