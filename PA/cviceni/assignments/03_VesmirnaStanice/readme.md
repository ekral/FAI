# Vesmírná stanice

## 🔢 Popis úkolu

Představte si, že vytváříte konzolovou "hru", ve které provádíte kosmonauta vesmírnou stanicí, která byla napadena neznámým útočníkem.
Pro práci na úkolu použijte přiložený soubor Program.cs, který obsahuje základ aplikace, do kterého následně přidáte další kód, 
podle zadání.

Odevzdejte jak soubor Program.cs, tak .pdf soubor s vypracovaným protokolem k zadání.
(Úvod, Postup práce včetně zdrojů ze kterých jste čerpal(a) inspiraci a Závěr, kde shrnete své pocity a trable z vypracovávání úkolu).

⌛ Celková náročnost úkolu = cca 2 hodiny

## 🚀 Výchozí kód v C# se zadáním úkolů

```csharp
/*
 * SPACE STATION ESCAPE - Text Adventure Game
 * 
 * SCENARIO:
 * You are a crew member aboard the research station "Artemis-7" orbiting a distant planet.
 * An alien outbreak has infected the station, and most of your crew is dead or missing.
 * Your goal is to navigate through the dangerous station to reach the hangar bay and escape.
 * You must make careful choices - some will help you survive, others will lead to your doom.
 * 
 * TASKS TO COMPLETE:
 * 
 * 1. GAME SETUP (Already finished)
 *    - Initialize player stats (health, ammo, hasKeycard)
 *    - Display welcome message and initial scenario
 * 
 * 2. IMPLEMENT LINEAR SCENE PROGRESSION
 *    - Each scene leads directly to the next based on player choices, use if-else statements and booleans to determine the correct path
 *
 * 
 * 3. IMPLEMENT SCENE 1: CREW QUARTERS (Starting Location)
 *    - Use a SWITCH statement to handle player choices (at least 3 options)
 *    - Options might include: search the room, check the corridor, hide, etc.
 *    - Update player stats based on choices (find ammo, take damage, etc.)
 *    - Dont forget to inform the player via Console commands
 * 
 * 4. IMPLEMENT SCENE 2: CORRIDOR
 *    - Use IF-ELSE statements to create branching paths
 *    - Example: If player has low health, certain options are unavailable
 *    - Include at least one alien encounter
 * 
 * 5. IMPLEMENT SCENE 3: MEDICAL BAY OR ARMORY (Player's choice from corridor)
 *    - Medical Bay: Restore health (use IF to check if health is already full)
 *    - Armory: Get ammo and weapons (use IF-ELSE to check inventory)
 * 
 * 6. IMPLEMENT SCENE 4: ENGINEERING OR SECURITY (Two paths)
 *    - Use NESTED IF-ELSE statements for complex decision trees
 *    - One path requires a keycard (check hasKeycard boolean)
 * 
 * 7. IMPLEMENT SCENE 5: HANGAR BAY (Final Scene)
 *    - Use IF-ELSE to determine the ending based on player stats
 *    - Possible endings:
 *      * SUCCESS: Reach hangar with health > 0
 *      * PARTIAL SUCCESS: Escape but badly wounded (health < 30)
 *      * FAILURE: Death (health <= 0)
 *      * HEROIC ENDING: Escape with high health and saved survivors (bonus)
 * 
 * 8. INPUT VALIDATION
 *    - Use IF-ELSE or SWITCH with a default case to handle invalid inputs
 *    - Display error messages and ask for input again
 * 
 * 9. ADD VISUAL POLISH (BONUS)
 *    - Use Console.Clear() between scenes for better readability
 *    - Display current stats (health, ammo) at key moments
 *    - Add ASCII art or decorative text
 * 
 * 10. TESTING
 *     - Test all possible paths through the game
 *     - Ensure all endings are reachable
 *     - Check that conditionals work correctly (keycard locks, health checks, etc.)
 * 
 * BONUS CHALLENGES:
 * - Add a random encounter system using Random class with IF-ELSE
 * - Create a combat system where player can fight or flee
 * - Track multiple items in an inventory
 * - Add a stealth mechanic (noise level that attracts aliens)
 * - Include a moral choice that affects the ending
 */

using System;

namespace SpaceStationEscape
{
    class Program
    {
        static void Main(string[] args)
        {
            // ===== GAME VARIABLES =====
            // Player stats
            int health = 100;
            int ammo = 10;
            bool hasKeycard = false;
            bool gameOver = false;
            string currentLocation = "crewQuarters";

            // ===== WELCOME MESSAGE =====
            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║          SPACE STATION ESCAPE                          ║");
            Console.WriteLine("║          Survive. Navigate. Escape.                    ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("You wake up in your crew quarters aboard Artemis-7.");
            Console.WriteLine("The emergency lights are flashing red. Sirens wail in the distance.");
            Console.WriteLine("Your last memory: screams in the corridor, and something... alien.");
            Console.WriteLine();
            Console.WriteLine($"Current Status - Health: {health} | Ammo: {ammo}");
            Console.WriteLine();
            Console.WriteLine("Press any key to begin...");
            Console.ReadKey();
            Console.Clear();

            // ===== MAIN GAME =====
            // TODO: Use switch or if-else to handle different locations
            // TODO: Each location should present choices and update game state

            // ===== EXAMPLE SCENE STRUCTURE (You need to expand this) =====
            Console.WriteLine("=== CREW QUARTERS ===");
            Console.WriteLine($"Health: {health} | Ammo: {ammo}");
            Console.WriteLine();
            Console.WriteLine("You're in your small crew quarters. The door is slightly ajar.");
            Console.WriteLine("You can hear something moving in the corridor outside.");
            Console.WriteLine();
            Console.WriteLine("What do you do?");
            Console.WriteLine("1. Search the room for supplies");
            Console.WriteLine("2. Peek into the corridor");
            Console.WriteLine("3. Barricade the door and hide");
            Console.Write("\nEnter your choice (1-3): ");

            string choice = Console.ReadLine();

            // TODO: Implement switch statement here to handle the choice
            // TODO: Update variables based on player choice
            // TODO: Move to next scene

            // ===== ENDING PLACEHOLDER =====
            // TODO: Implement different endings based on player stats and choices

            Console.WriteLine("\n=== GAME OVER ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
```