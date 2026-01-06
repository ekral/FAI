# Settlers of Catan

## 🔢 Popis úkolu

Vytváříte aplikaci simulující stolní hru Settlers of Catan (Osadníci z Catanu)
Pro práci na úkolu použijte přiložený soubor Program.cs, který obsahuje základ aplikace, do kterého následně přidáte další kód, 
podle zadání.

Odevzdejte jak soubor Program.cs, tak .pdf soubor s vypracovaným protokolem k zadání.
(Úvod, Postup práce včetně zdrojů ze kterých jste čerpal(a) inspiraci a Závěr, kde shrnete své pocity a trable z vypracovávání úkolu).

⌛ Celková náročnost úkolu = cca 2 hodiny

## 🚀 Výchozí kód v C# se zadáním úkolů

```csharp
/*
 * ====================================================================
 *            SETTLERS OF CATAN CONSOLE GAME ASSIGNMENT
 * ====================================================================
 * 
 * DESCRIPTION:
 * Create a simplified console version of Settlers of Catan focusing on
 * resource management and building. This assignment emphasizes creating
 * classes with constructors and methods, proper encapsulation, and
 * object-oriented design without advanced OOP techniques.
 * 
 * ESTIMATED TIME: 2 hours
 * 
 * ====================================================================
 * FILE STRUCTURE REQUIREMENTS:
 * ====================================================================
 * 
 * You MUST create the following separate files in your project:
 * 
 * 1. ResourceType.cs - Contains the ResourceType enum
 * 2. SettlementTier.cs - Contains the SettlementTier enum
 * 3. Player.cs - Contains the Player class
 * 4. Hex.cs - Contains the Hex class
 * 5. Settlement.cs - Contains the Settlement class
 * 6. GameBoard.cs - Contains the GameBoard class
 * 7. Program.cs - Contains the Main method (this file)
 * 
 * All files should be in the namespace: CatanGame
 * 
 * ====================================================================
 * GAME RULES (Simplified):
 * ====================================================================
 * 
 * OBJECTIVE: First player to reach 10 victory points wins!
 * 
 * VICTORY POINTS:
 * - Settlement = 1 point
 * - City = 2 points
 * 
 * RESOURCES (using enum):
 * - Wood, Brick, Sheep, Wheat, Ore
 * 
 * BUILDING COSTS:
 * - Settlement: 1 Wood, 1 Brick, 1 Sheep, 1 Wheat
 * - City (upgrade from Settlement): 3 Ore, 2 Wheat
 * 
 * TRADING:
 * - Universal 3:1 trade ratio
 * - Trade any 3 of the same resource for 1 of any other resource
 * - No limit on trading (as long as you have resources)
 * 
 * GAME FLOW:
 * 1. Setup: 2-4 players start with initial resources
 * 2. Each turn:
 *    a) Roll two dice (2-12)
 *    b) All hexes with matching number produce resources
 *    c) Players with settlements on those hexes get resources
 *    d) Current player can: Build Settlement, Upgrade to City, Trade Resources, or End Turn
 * 3. First to 10 points wins
 * 
 * SIMPLIFICATIONS:
 * - Fixed board layout (19 hexes)
 * - Settlement positions are numbered 1-54
 * - No player-to-player trading (only 3:1 with bank)
 * - No robber or development cards
 * - Resources distributed to ALL players (not just current player)
 * 
 * ====================================================================
 * REQUIRED TASKS:
 * ====================================================================
 * 
 * TASK 1: Create the ResourceType Enum (in ResourceType.cs)
 * -------
 * Create an enum with the following values:
 * - Wood
 * - Brick
 * - Sheep
 * - Wheat
 * - Ore
 * 
 * TASK 2: Create the SettlementTier Enum (in SettlementTier.cs)
 * -------
 * Create an enum with the following values:
 * - Empty (position has no building)
 * - Settlement (basic building, 1 victory point)
 * - City (upgraded building, 2 victory points)
 * 
 * TASK 3: Create the Player Class (in Player.cs)
 * -------
 * Fields:
 * - public string Name;
 * - public int Wood;
 * - public int Brick;
 * - public int Sheep;
 * - public int Wheat;
 * - public int Ore;
 * - public List<Settlement> Settlements;
 * 
 * Constructors:
 * - Default constructor: Initialize all resources to 0, create empty Settlements list
 * - Constructor with name parameter: Set Name, initialize resources to 3 each,
 *   create empty Settlements list (starting resources for trading)
 * 
 * Methods to implement:
 * - void AddResource(ResourceType resourceType, int amount)
 *   Takes resource type as enum and adds amount to the appropriate resource
 *   Use switch statement on resourceType enum
 *   Example cases:
 *   case ResourceType.Wood: Wood += amount; break;
 *   case ResourceType.Brick: Brick += amount; break;
 *   etc.
 * 
 * - void RemoveResource(ResourceType resourceType, int amount)
 *   Similar to AddResource but subtracts instead
 *   Use switch statement on resourceType enum
 * 
 * - int GetResourceAmount(ResourceType resourceType)
 *   Returns the amount of the specified resource
 *   Use switch statement to return correct resource count
 * 
 * - bool CanAffordSettlement()
 *   Returns true if player has at least 1 Wood, 1 Brick, 1 Sheep, 1 Wheat
 * 
 * - bool CanAffordCity()
 *   Returns true if player has at least 3 Ore and 2 Wheat
 * 
 * - void BuildSettlement(int position)
 *   Deducts resources (1 Wood, 1 Brick, 1 Sheep, 1 Wheat)
 *   Creates new Settlement object with this player's name, position, and tier = Settlement
 *   Adds it to Settlements list
 * 
 * - void UpgradeToCity(int settlementIndex)
 *   Deducts resources (3 Ore, 2 Wheat)
 *   Calls UpgradeToCity() method on the settlement at given index
 * 
 * - bool CanTrade(ResourceType fromResource)
 *   Returns true if player has at least 3 of the specified resource
 * 
 * - void Trade(ResourceType fromResource, ResourceType toResource)
 *   Remove 3 of fromResource
 *   Add 1 of toResource
 *   Display trade message
 * 
 * - int GetVictoryPoints()
 *   Loop through all settlements, sum up their victory points
 *   Return total
 * 
 * - void DisplayResources()
 *   Display all resources in a nice format
 *   Example: "Wood: 3, Brick: 2, Sheep: 4, Wheat: 1, Ore: 0"
 * 
 * - void DisplaySettlements()
 *   Loop through settlements list and display each one
 * 
 * 
 * TASK 4: Create the Hex Class (in Hex.cs)
 * -------
 * Fields:
 * - public ResourceType Resource;
 * - public int NumberToken;  // 2-12
 * - public int HexID;  // 1-19 for identification
 * 
 * Constructors:
 * - Constructor with resource and numberToken parameters
 *   Set Resource and NumberToken, HexID = 0 (will be set later)
 * 
 * - Constructor with all three parameters (resource, numberToken, hexID)
 *   Set all three fields
 * 
 * Methods to implement:
 * - ResourceType GetResource()
 *   Return the Resource field
 * 
 * - int GetNumberToken()
 *   Return the NumberToken field
 * 
 * - void Display()
 *   Display hex info in format: "Hex #[ID]: [Resource] ([NumberToken])"
 *   Example: "Hex #5: Wood (6)"
 *   Note: Enum will automatically convert to string when printed
 * 
 * 
 * TASK 5: Create the Settlement Class (in Settlement.cs)
 * -------
 * Fields:
 * - public string OwnerName;
 * - public int Position;  // 1-54 (position on board)
 * - public SettlementTier Tier;  // Using enum: Empty, Settlement, or City
 * 
 * Constructors:
 * - Constructor with ownerName and position parameters
 *   Set OwnerName and Position
 *   Set Tier = SettlementTier.Settlement by default
 * 
 * - Constructor with all three parameters (ownerName, position, tier)
 *   Set all three fields
 * 
 * Methods to implement:
 * - void UpgradeToCity()
 *   Set Tier = SettlementTier.City
 * 
 * - int GetVictoryPoints()
 *   Use switch on Tier:
 *   - SettlementTier.Empty: return 0
 *   - SettlementTier.Settlement: return 1
 *   - SettlementTier.City: return 2
 * 
 * - string GetOwnerName()
 *   Return OwnerName
 * 
 * - SettlementTier GetTier()
 *   Return Tier
 * 
 * - void Display()
 *   Display settlement info based on tier
 *   Example: "Settlement at position 12 (Owner: Alice)" or
 *            "City at position 12 (Owner: Alice)"
 *   Use if statement or switch on Tier
 * 
 * 
 * TASK 6: Create the GameBoard Class (in GameBoard.cs)
 * -------
 * Fields:
 * - public List<Hex> Hexes;
 * - public List<Settlement> AllSettlements;  // All settlements on the board
 * 
 * Constructor:
 * - Default constructor:
 *   Initialize Hexes list and AllSettlements list
 *   Call InitializeBoard() method to populate hexes
 * 
 * Methods to implement:
 * - void InitializeBoard()
 *   Create 19 Hex objects with various resources and numbers
 *   Use the ResourceType enum when creating hexes
 *   Example setup (you can customize):
 *   - 4 Wood hexes (numbers: 3, 4, 8, 11)
 *   - 4 Brick hexes (numbers: 5, 6, 9, 10)
 *   - 4 Sheep hexes (numbers: 2, 4, 9, 11)
 *   - 4 Wheat hexes (numbers: 3, 5, 10, 12)
 *   - 3 Ore hexes (numbers: 6, 8, 10)
 *   Example: new Hex(ResourceType.Wood, 3, 1)
 *   Add all to Hexes list
 * 
 * - void DisplayBoard()
 *   Loop through all hexes and call their Display() method
 * 
 * - void PlaceSettlement(Settlement settlement)
 *   Add the settlement to AllSettlements list
 * 
 * - void DistributeResources(int diceRoll, List<Player> players)
 *   Loop through all hexes
 *   If hex's NumberToken matches diceRoll:
 *     Get the hex's resource type and ID
 *     Loop through AllSettlements
 *     For each settlement, call GetAdjacentHexes(settlement.Position) from Program class
 *     If the returned list contains the current hex's ID:
 *       Find the player who owns this settlement
 *       Give the owner 1 of the hex's resource (use AddResource with enum)
 *       If settlement is a City (tier), give 2 resources instead of 1
 *   
 *   Hint: You'll need to pass the dice roll and players list, and use the
 *   GetAdjacentHexes helper method provided in Program.cs to determine
 *   which settlements should receive resources from each hex
 * 
 * - List<Settlement> GetSettlementsForPlayer(string playerName)
 *   Loop through AllSettlements
 *   Return list of settlements owned by playerName
 * 
 * 
 * TASK 7: Implement Helper Methods in Program.cs
 * -------
 * Create these static methods in the Program class (outside Main):
 * 
 * - static int RollDice()
 *   Create Random object (or pass as parameter)
 *   Roll two dice (1-6 each)
 *   Return the sum (2-12)
 * 
 * - static void DisplayPlayerInfo(Player player)
 *   Display player's name
 *   Call player.DisplayResources()
 *   Display victory points using player.GetVictoryPoints()
 *   Call player.DisplaySettlements()
 * 
 * - static int GetPlayerChoice(int minOption, int maxOption)
 *   Display menu options
 *   Get user input
 *   Use while loop to validate input is between min and max
 *   Return validated choice
 * 
 * - static Player GetCurrentPlayer(List<Player> players, int turnIndex)
 *   Use modulo operator to cycle through players
 *   Return players[turnIndex % players.Count]
 * 
 * - static void DisplayTradeMenu()
 *   Display all resource types for trading
 *   Example:
 *   "Trade Menu (3:1 ratio):"
 *   "1. Wood"
 *   "2. Brick"
 *   "3. Sheep"
 *   "4. Wheat"
 *   "5. Ore"
 * 
 * - static ResourceType GetResourceFromChoice(int choice)
 *   Convert menu choice (1-5) to ResourceType enum
 *   Use switch statement:
 *   case 1: return ResourceType.Wood;
 *   case 2: return ResourceType.Brick;
 *   etc.
 * 
 * 
 * TASK 8: Implement the Game Loop in Main
 * -------
 * In the Main method:
 * 
 * 1. Setup Phase:
 *    - Ask how many players (2-4)
 *    - Create List<Player> and add players with names
 *    - Create GameBoard object
 *    - Display the board
 *    - Each player places 1 initial settlement (ask for position 1-54)
 * 
 * 2. Main Game Loop (use while loop - continue until someone reaches 10 points):
 *    - Determine current player
 *    - Display whose turn it is
 *    - Roll dice using RollDice()
 *    - Display dice result
 *    - Call board.DistributeResources(diceRoll, players)
 *    - Display current player's info
 *    - Show action menu:
 *      1. Build Settlement
 *      2. Upgrade to City
 *      3. Trade Resources (3:1)
 *      4. End Turn
 *    - Handle player's choice:
 *      - If build settlement: check CanAffordSettlement, ask position, build it, place on board
 *      - If upgrade city: show settlements, check CanAffordCity, ask which one, upgrade
 *      - If trade: 
 *        * Display trade menu for "from" resource
 *        * Get choice and convert to ResourceType enum
 *        * Check CanTrade
 *        * Display trade menu for "to" resource
 *        * Get choice and convert to ResourceType enum
 *        * Call player.Trade(fromResource, toResource)
 *      - If end turn: continue to next player
 *    - Check if any player has 10+ victory points
 *    - If yes, display winner and end game
 *    - Increment turn counter
 * 
 * 3. End Phase:
 *    - Display final scores for all players
 *    - Display winner
 * 
 * ====================================================================
 * BONUS TASKS (Optional - for extra credit):
 * ====================================================================
 * 
 * BONUS 1: Starting Placement Round
 * --------
 * - Each player places 2 settlements at start
 * - Player then gets initial resources from hexes adjacent to their settlements
 * - Implement PlaceStartingSettlements() method
 * 
 * BONUS 2: Resource Cards Limit (Robber Rule)
 * --------
 * - If dice roll is 7, players with 8+ resource cards must discard half
 * - Add GetTotalResources() method to Player
 * - Add DiscardResources() method where player chooses which to discard
 * 
 * 
 * BONUS 3: Better Board Visualization
 * --------
 * - Display board as ASCII art showing hex arrangement
 * - Show which positions have settlements/cities
 * - Use colors (Console.ForegroundColor) for different resources:
 *   * Wood = Green
 *   * Brick = Red
 *   * Sheep = White/Gray
 *   * Wheat = Yellow
 *   * Ore = DarkGray
 * 
 * 
 * BONUS 4: Multiple Trades Per Turn
 * --------
 * - Allow player to trade multiple times in one turn
 * - After each trade, show updated resources and ask if they want to trade again
 * 
 * 
 * BONUS 5: Trade Validation and Better UI
 * --------
 * - Prevent trading same resource (from = to)
 * - Show preview of trade before confirming
 * - Add "Cancel" option in trade menu
 * - Display what player can afford to build after each action
 * 
 * ====================================================================
 * HINTS:
 * ====================================================================
 * 
 * USING GetAdjacentHexes:
 * - Call it with: List<int> adjacentHexes = GetAdjacentHexes(settlementPosition);
 * - Check if a hex ID is in the list: adjacentHexes.Contains(hexID)
 * - Use this in DistributeResources to give resources only to adjacent settlements
 * - Example: if (GetAdjacentHexes(settlement.Position).Contains(hex.HexID))
 *   then give resources to the settlement owner
 * 
 * ENUM TIPS:
 * - Enums are defined outside of classes (at namespace level)
 * - Use enums in switch statements for clean code
 * - Enums automatically convert to strings when printed
 * - Compare enums with ==: if (resource == ResourceType.Wood)
 * - Can't use enums as array indices directly, cast to int if needed
 * 
 * CONSTRUCTOR TIPS:
 * - Initialize lists in constructor: Settlements = new List<Settlement>();
 * - You can have multiple constructors (constructor overloading)
 * - Use meaningful default values
 * 
 * METHOD TIPS:
 * - Use switch statements with enums for cleaner code than if-else chains
 * - Methods that change object state should be void (BuildSettlement, Trade)
 * - Methods that calculate/retrieve should return values (GetVictoryPoints, GetResourceAmount)
 * 
 * COMMON MISTAKES:
 * - Forgetting to initialize lists in constructor (causes null reference)
 * - Not checking if player can afford before building
 * - Forgetting to deduct resources after building/trading
 * - Not handling all enum cases in switch statements
 * - Comparing enums with strings instead of enum values
 * 
 * TESTING TIPS:
 * - Test enums by printing them (they show as strings)
 * - Test each class separately before building full game
 * - Give players extra starting resources to test trading
 * - Try trading multiple times to verify resource tracking
 * 
 * ====================================================================
 */

using System;
using System.Collections.Generic;

namespace CatanGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("   SETTLERS OF CATAN - CONSOLE EDITION");
            Console.WriteLine("========================================\n");
            
            // TODO: TASK 8 - Implement the game here
            // 
            // Setup Phase:
            // - Create players
            // - Create game board
            // - Place initial settlements
            //
            // Game Loop:
            // - Roll dice
            // - Distribute resources
            // - Let player: Build, Upgrade, Trade, or End Turn
            // - Check for winner
            //
            // End Phase:
            // - Display winner and final scores
            
            
            Console.WriteLine("\n========================================");
            Console.WriteLine("      Thanks for playing Catan!");
            Console.WriteLine("========================================");
        }
        
        // ====================================================================
        // TODO: TASK 7 - Create helper methods here
        // ====================================================================
        
        // static int RollDice()
        // {
        //     
        // }
        
        // static void DisplayPlayerInfo(Player player)
        // {
        //     
        // }
        
        // static int GetPlayerChoice(int minOption, int maxOption)
        // {
        //     
        // }
        
        // static Player GetCurrentPlayer(List<Player> players, int turnIndex)
        // {
        //     
        // }
        
        // static void DisplayTradeMenu()
        // {
        //     
        // }
        
        // static ResourceType GetResourceFromChoice(int choice)
        // {
        //     
        // }
        
        
        // ====================================================================
        // PRE-BUILT HELPER METHOD - DO NOT MODIFY
        // This method is provided to help you map settlements to hexes
        // ====================================================================
        
        /// <summary>
        /// Returns a list of hex IDs that are adjacent to the given settlement position.
        /// Settlement positions are numbered 1-54, hex IDs are numbered 1-19.
        /// This represents a standard Catan board layout.
        /// </summary>
        static List<int> GetAdjacentHexes(int settlementPosition)
        {
            // This dictionary maps each settlement position to its neighboring hexes
            // You can use this in DistributeResources to give resources only to
            // players whose settlements are actually next to producing hexes
            
            Dictionary<int, List<int>> settlementToHexes = new Dictionary<int, List<int>>()
            {
                // Top row settlements
                {1, new List<int> {1}}, {2, new List<int> {1, 2}}, {3, new List<int> {2}},
                {4, new List<int> {2, 3}}, {5, new List<int> {3}}, {6, new List<int> {3, 4}},
                {7, new List<int> {4}},
                
                // Second row settlements
                {8, new List<int> {1}}, {9, new List<int> {1, 5}}, {10, new List<int> {1, 2, 5}},
                {11, new List<int> {2, 6}}, {12, new List<int> {2, 3, 6}}, {13, new List<int> {3, 7}},
                {14, new List<int> {3, 4, 7}}, {15, new List<int> {4, 8}}, {16, new List<int> {4, 8}},
                
                // Third row settlements
                {17, new List<int> {5}}, {18, new List<int> {5, 9}}, {19, new List<int> {5, 6, 9}},
                {20, new List<int> {6, 10}}, {21, new List<int> {6, 7, 10}}, {22, new List<int> {7, 11}},
                {23, new List<int> {7, 8, 11}}, {24, new List<int> {8, 12}}, {25, new List<int> {8, 12}},
                
                // Fourth row settlements
                {26, new List<int> {9}}, {27, new List<int> {9, 13}}, {28, new List<int> {9, 10, 13}},
                {29, new List<int> {10, 14}}, {30, new List<int> {10, 11, 14}}, {31, new List<int> {11, 15}},
                {32, new List<int> {11, 12, 15}}, {33, new List<int> {12, 16}}, {34, new List<int> {12, 16}},
                
                // Fifth row settlements
                {35, new List<int> {13}}, {36, new List<int> {13, 17}}, {37, new List<int> {13, 14, 17}},
                {38, new List<int> {14, 18}}, {39, new List<int> {14, 15, 18}}, {40, new List<int> {15, 19}},
                {41, new List<int> {15, 16, 19}}, {42, new List<int> {16}}, {43, new List<int> {16}},
                
                // Bottom row settlements
                {44, new List<int> {17}}, {45, new List<int> {17}}, {46, new List<int> {17, 18}},
                {47, new List<int> {18}}, {48, new List<int> {18, 19}}, {49, new List<int> {19}},
                {50, new List<int> {19}},
                
                // Additional edge positions
                {51, new List<int> {1}}, {52, new List<int> {4}}, {53, new List<int> {13}}, {54, new List<int> {16}}
            };
            
            if (settlementToHexes.ContainsKey(settlementPosition))
            {
                return settlementToHexes[settlementPosition];
            }
            
            // If position not found, return empty list
            return new List<int>();
        }
    }
}
```