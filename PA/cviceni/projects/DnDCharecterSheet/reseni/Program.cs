using CharacterSheet.Helpers;
using CharacterSheet.Models;

namespace CharSheet;

class Program
{
    static void Main()
    {
        
        Console.WriteLine("Welcome to D&D Character Creator!");
        Character character = new Character();
        Character c2 = new Character(character);
        

        // --- Output ---

        bool isRunning = true;
        while (isRunning)
        {
            Console.WriteLine(" ... Press any key to continue to the main menu");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine(" ===*| Character Sheet - Main menu |*===");
            Console.WriteLine("--| Character actions |--");
            Console.WriteLine("Create New Character: 1");
            Console.WriteLine("View Character info: 2");
            Console.WriteLine("--| Dice rolling |--");
            Console.WriteLine("Roll check: 3");
            Console.WriteLine("Roll until you pass: 4");
            Console.WriteLine("--| Inventory management |--");
            Console.WriteLine("Add item: 5");
            Console.WriteLine("Remove item: 6");
            Console.WriteLine("Check inventory for item: 7");
            Console.WriteLine("--| Health management |--");
            Console.WriteLine("Take/Heal damage: 8");
            Console.WriteLine("Roll for attack: 9");
            Console.WriteLine(" -**- EXIT: Press any other key -**-");

            int option = int.TryParse(Console.ReadLine(), out option) ? option : 99;
            var rand = new Random();
            int roll = 0;
            switch (option)
            {
                default:
                    isRunning = false;
                    break;
                case 1:
                    Console.Clear();
                    character.InitializeCharacter();
                    break;
                case 2:
                    character.DisplayCharacter();
                    break;
                case 3:
                    Console.WriteLine("Choose which mod to use");
                    Console.WriteLine("str=1, dex=2, con=3, intel=4, wis=5, cha=6 ");
                    int modifier = int.TryParse(Console.ReadLine(), out modifier) ? modifier : 1;
                    roll = character.RollCheck(modifier);
                    Console.WriteLine($"You have rolled: {roll}");
                    break;
                case 4:
                    dc:
                    Console.WriteLine("Set the DC");
                    int DC = int.TryParse(Console.ReadLine(), out DC) ? DC : 15;
                    Console.WriteLine("Choose which mod to use");
                    Console.WriteLine("str=1, dex=2, con=3, intel=4, wis=5, cha=6 ");
                    int mod = int.TryParse(Console.ReadLine(), out mod) ? mod : 1;
                    int bonus = character.GetAbilityModifier((AbilityScore)Math.Clamp(mod, 0, 5));

                    if (bonus + 20 < DC)
                    {
                        Console.WriteLine("This is impossible to pass");
                        goto dc;
                    }

                    int totalRolls = 0;
                    do
                    {
                        roll = rand.Next(1, 21);
                        Console.WriteLine(roll + bonus);
                        totalRolls++;
                    } while (roll + bonus < DC);

                    Console.WriteLine($"It took this many rolls {totalRolls}");
                    break;
                case 5:
                    Console.WriteLine("Enter name of item ");
                    string itemToAdd = Console.ReadLine();
                    character.AddItemToEquipment(itemToAdd);
                    break;
                case 6:
                    Console.WriteLine("Enter name of item ");
                    string itemToRemove = Console.ReadLine();
                    bool success = character.RemoveItemFromEquipment(itemToRemove);
                    if (success)
                    {
                        Console.WriteLine("Item removed");
                    }
                    else
                    {
                        Console.WriteLine("Failed to remove item, please try again");
                    }

                    break;
                case 7:
                    Console.WriteLine("Which item are you looking for?:");
                    string checkItem = Console.ReadLine() ?? "";
                    bool itemExists = character.GetEquipment().Contains(checkItem);
                    if (itemExists)
                    {
                        Console.WriteLine("The item that you are looking for does indeed exist");
                    }
                    else
                    {
                        Console.WriteLine("The item does not exist");
                    }

                    break;
                case 8:
                    Console.WriteLine("Enter amount of damage/healing(negative number to heal)");
                    int damHel = int.TryParse(Console.ReadLine(), out damHel) ? damHel : 5;
                    character.TakeDamage(damHel);
                    break;
                case 9:
                    Console.WriteLine("Enter target AC");
                    int AC = int.TryParse(Console.ReadLine(), out AC) ? AC : 10;

                    if (character.RollAttack(AC, out int dmg))
                    {
                        Console.WriteLine("Character hit successfully and did: " + dmg);
                    }
                    else
                    {
                        Console.WriteLine("Character hit failed");
                    }

                    break;
            }
        }
    }


    static public string Fmt(int mod) => mod >= 0 ? "+" + mod : mod.ToString();
    
}