using CharacterSheet.Models;

namespace CharacterSheet.Helpers;

public static class DiceHelpers
{
    public static int RollDice(Dice dice)
    {
        var rand = new Random();
        int roll = 0;
        int total = 0;
        
        for (int i = 1; i <= dice.DicePool; i++)
        {
            roll = rand.Next(1, dice.Sides + 1);
            total += roll;
        }
        return total;
    }

    public static int RollDice(int dicePool,int sides)
    {
        Dice dice;
        dice.DicePool = dicePool;
        dice.Sides = sides;
        return RollDice(dice);
    }

    public static int RollD20()
    {
        Dice dice;
        dice.DicePool = 1;
        dice.Sides = 20;
        return RollDice(dice);
    }

    public static int SelectEnumOption(int upperLimit) =>
        new Random().Next(0, upperLimit);

    public static int RollAbilityScore()
    {
        List<int> rolls = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            rolls.Add(RollDice(1, 6));
        }

        rolls.Remove(rolls.Min());
        return rolls.Sum();
    }
}