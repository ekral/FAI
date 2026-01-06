# Blackjack

## 🔢 Popis úkolu

Vytváříte aplikaci simulující karetní hru Blackjack.
Pro práci na úkolu použijte přiložený soubor Program.cs, který obsahuje základ aplikace, do kterého následně přidáte další kód, 
podle zadání.

Odevzdejte jak soubor Program.cs, tak .pdf soubor s vypracovaným protokolem k zadání.
(Úvod, Postup práce včetně zdrojů ze kterých jste čerpal(a) inspiraci a Závěr, kde shrnete své pocity a trable z vypracovávání úkolu).

⌛ Celková náročnost úkolu = cca 2 hodiny

## 🚀 Výchozí kód v C# se zadáním úkolů

```csharp
/*
 * ====================================================================
 *                   CARD GAME TOOLKIT ASSIGNMENT
 * ====================================================================
 * 
 * DESCRIPTION:
 * Create a console application that implements a simple card game
 * (similar to Blackjack) using various methods and functions.
 * This assignment focuses on practicing Methods with different return
 * types, parameters, and understanding when to use void vs returning values.
 * 
 * 
 * ====================================================================
 * GAME RULES (Simplified Blackjack):
 * ====================================================================
 * 
 * - Goal: Get as close to 21 as possible without going over
 * - Card values: 2-10 = face value, J/Q/K = 10, A = 11 (simplified)
 * - Player and dealer each start with 2 cards
 * - Player can "hit" (draw card) or "stand" (stop drawing)
 * - If player goes over 21, they lose (bust)
 * - If player stands, dealer draws until reaching 17 or higher
 * - Whoever is closest to 21 without going over wins
 * 
 * ====================================================================
 * CARD REPRESENTATION:
 * ====================================================================
 * 
 * Cards will be stored as strings in format: "Rank-Suit"
 * Examples: "A-Hearts", "10-Diamonds", "K-Spades", "7-Clubs"
 * 
 * Ranks: A, 2, 3, 4, 5, 6, 7, 8, 9, 10, J, Q, K
 * Suits: Hearts, Diamonds, Clubs, Spades
 * 
 * ====================================================================
 * REQUIRED TASKS:
 * ====================================================================
 * 
 * TASK 1: Create the CreateDeck() Method
 * -------
 * Method signature: static List<string> CreateDeck()
 * 
 * This method should:
 * - Create and return a List<string> representing a full deck of 52 cards
 * - Use nested for loops to create all combinations of ranks and suits
 * - Ranks array: {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"}
 * - Suits array: {"Hearts", "Diamonds", "Clubs", "Spades"}
 * - Format each card as "Rank-Suit" (e.g., "A-Hearts")
 * - Return the complete deck
 * 
 * Hint: You'll need two arrays and nested loops to create all 52 combinations
 * 
 * 
 * TASK 2: Create the ShuffleDeck() Method
 * -------
 * Method signature: static void ShuffleDeck(List<string> deck)
 * 
 * This method should:
 * - Take a deck as a parameter (passed by reference - lists are reference types)
 * - Shuffle the deck randomly using the Fisher-Yates algorithm:
 *   - Loop from the last index down to 0
 *   - For each position, generate a random index from 0 to current position
 *   - Swap the cards at these two positions
 * - This is a void method because it modifies the existing list
 * 
 * You'll need: Random random = new Random(); (create in Main, pass if needed,
 * or create inside the method)
 * 
 * 
 * TASK 3: Create the DrawCard() Method
 * -------
 * Method signature: static string DrawCard(List<string> deck)
 * 
 * This method should:
 * - Take the deck as a parameter
 * - Remove and return the first card from the deck (index 0)
 * - Use deck.RemoveAt(0) after storing the card
 * - Return the card that was drawn
 * 
 * This demonstrates a method that both modifies a parameter AND returns a value.
 * 
 * 
 * TASK 4: Create the GetCardValue() Method
 * -------
 * Method signature: static int GetCardValue(string card)
 * 
 * This method should:
 * - Take a card string as parameter (e.g., "K-Hearts")
 * - Extract the rank (the part before the "-")
 * - Return the numeric value:
 *   - "A" = 11
 *   - "2" to "10" = face value (convert string to int)
 *   - "J", "Q", "K" = 10
 * 
 * Hint: Use card.Split('-')[0] to get the rank
 * Hint: Use int.TryParse() or if-else chains to handle different ranks
 * 
 * 
 * TASK 5: Create the CalculateHandValue() Method
 * -------
 * Method signature: static int CalculateHandValue(List<string> hand)
 * 
 * This method should:
 * - Take a list of cards (a hand) as parameter
 * - Calculate the total value of all cards in the hand
 * - Use a loop to go through each card
 * - Call GetCardValue() for each card
 * - Return the sum
 * 
 * This demonstrates a method calling another method.
 * 
 * 
 * TASK 6: Create the DisplayHand() Method
 * -------
 * Method signature: static void DisplayHand(List<string> hand, string playerName)
 * 
 * This method should:
 * - Take a hand and a player name as parameters
 * - Display the player's name
 * - Display all cards in the hand on one line
 * - Display the total value of the hand
 * - This is a void method because it only displays information
 * 
 * Example output:
 * Player's Hand: A-Hearts, 7-Diamonds
 * Total Value: 18
 * 
 * 
 * TASK 7: Create the DetermineWinner() Method
 * -------
 * Method signature: static string DetermineWinner(int playerValue, int dealerValue)
 * 
 * This method should:
 * - Take two integers: player's hand value and dealer's hand value
 * - Return a string with the result:
 *   - If player > 21: return "Player busts! Dealer wins!"
 *   - If dealer > 21: return "Dealer busts! Player wins!"
 *   - If player > dealer: return "Player wins!"
 *   - If dealer > player: return "Dealer wins!"
 *   - If equal: return "It's a tie!"
 * 
 * This demonstrates a method with multiple parameters and conditional logic.
 * 
 * 
 * TASK 8: Create the GetPlayerAction() Method
 * -------
 * Method signature: static string GetPlayerAction()
 * 
 * This method should:
 * - Display options: "Do you want to (H)it or (S)tand?"
 * - Read user input
 * - Use a while loop to validate input is "H" or "S" (case-insensitive)
 * - Return the validated choice ("H" or "S" in uppercase)
 * 
 * Hint: Use input.ToUpper() for case-insensitive comparison
 * This demonstrates input validation within a method.
 * 
 * 
 * TASK 9: Create the PlayAgain() Method
 * -------
 * Method signature: static bool PlayAgain()
 * 
 * This method should:
 * - Ask the user "Do you want to play again? (Y/N)"
 * - Read and validate input (must be Y or N)
 * - Return true if "Y", false if "N"
 * 
 * This demonstrates a method returning a boolean value.
 * 
 * 
 * TASK 10: Implement the Game Loop in Main
 * -------
 * In the Main method, use all the functions you created to build the game:
 * 
 * 1. Create a deck using CreateDeck()
 * 2. Shuffle it using ShuffleDeck()
 * 3. Create two hands (List<string>) for player and dealer
 * 4. Draw 2 cards for each player using DrawCard()
 * 5. Display both hands (hide one dealer card initially - optional)
 * 6. Player's turn:
 *    - Display player's hand using DisplayHand()
 *    - Ask for action using GetPlayerAction()
 *    - If Hit: draw a card, check if bust (value > 21)
 *    - If Stand or Bust: end player's turn
 * 7. Dealer's turn (if player didn't bust):
 *    - While dealer's value < 17: draw cards
 * 8. Determine winner using DetermineWinner()
 * 9. Ask to play again using PlayAgain()
 * 
 * Use a do-while loop for the main game loop.
 * 
 * ====================================================================
 * BONUS TASKS (Optional)
 * ====================================================================
 * 
 * BONUS 1: Track Statistics
 * --------
 * Create methods:
 * - void UpdateStatistics(string result) - updates win/loss counters
 * - void DisplayStatistics(int wins, int losses, int ties) - shows stats
 * Track player's record across multiple games.
 * 
 * 
 * BONUS 2: Betting System
 * --------
 * Create methods:
 * - int PlaceBet(int currentMoney) - handles betting with validation
 * - int UpdateMoney(int currentMoney, int bet, bool won) - updates player's money
 * Give player starting money ($100) and let them bet each round.
 * 
 * 
 * BONUS 3: Advanced Ace Handling
 * --------
 * Modify GetCardValue() and CalculateHandValue():
 * - Make Ace count as 11 or 1 (whichever is better)
 * - If hand > 21 and has an Ace counting as 11, change it to 1
 * This is how real Blackjack works.
 * 
 * 
 * BONUS 4: Card Counting Display
 * --------
 * Create method:
 * - int GetRemainingCards(List<string> deck) - returns deck.Count
 * - void DisplayDeckInfo(List<string> deck) - shows cards remaining
 * Display how many cards are left in the deck after each round.
 * Reshuffle if deck gets too low (< 15 cards).
 * 
 * 
 * BONUS 5: Multiple Players
 * --------
 * Modify the game to support 2-4 players:
 * - Create method: int GetNumberOfPlayers() - validates input 2-4
 * - Use List<List<string>> to store multiple hands
 * - Each player plays against the dealer
 * 
 * ====================================================================
 * HINTS:
 * ====================================================================
 * - Place all methods OUTSIDE of Main but INSIDE the Program class
 * - Methods should be declared as "static" since Main is static
 * - Use meaningful parameter names: DrawCard(List<string> deck) not DrawCard(List<string> x)
 * - Test each method individually before building the full game
 * - Methods with return values should always return something
 * - Void methods perform actions but don't return values
 * - Methods can call other methods you've created
 * - Use Console.WriteLine() in void methods for display
 * - Remember: lists are reference types (changes inside methods affect the original)
 * 
 * METHOD SIGNATURE QUICK REFERENCE:
 * - static returnType MethodName(parameters) { }
 * - returnType can be: void, int, string, bool, List<string>, etc.
 * - If void, no return statement needed (or use "return;" to exit early)
 * - If non-void, must return a value of that type
 * 
 * ====================================================================
 */

using System;
using System.Collections.Generic;

namespace CardGameToolkit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("    BLACKJACK CARD GAME");
            Console.WriteLine("========================================\n");
            
            // TODO: TASK 10 - Implement the game loop here
            // Use all the methods you create below
            
            // Example structure:
            // List<string> deck = CreateDeck();
            // ShuffleDeck(deck);
            // 
            // bool playingAgain = true;
            // do
            // {
            //     Game logic here...
            //     playingAgain = PlayAgain();
            // } while (playingAgain);
            
            
            Console.WriteLine("\n========================================");
            Console.WriteLine("   Thanks for playing Blackjack!");
            Console.WriteLine("========================================");
        }
        
        // ====================================================================
        // TODO: TASK 1 - Create the CreateDeck() method here
        // ====================================================================
        // static List<string> CreateDeck()
        // {
        //     
        // }
        
        
        // ====================================================================
        // TODO: TASK 2 - Create the ShuffleDeck() method here
        // ====================================================================
        // static void ShuffleDeck(List<string> deck)
        // {
        //     
        // }
        
        
        // ====================================================================
        // TODO: TASK 3 - Create the DrawCard() method here
        // ====================================================================
        // static string DrawCard(List<string> deck)
        // {
        //     
        // }
        
        
        // ====================================================================
        // TODO: TASK 4 - Create the GetCardValue() method here
        // ====================================================================
        // static int GetCardValue(string card)
        // {
        //     
        // }
        
        
        // ====================================================================
        // TODO: TASK 5 - Create the CalculateHandValue() method here
        // ====================================================================
        // static int CalculateHandValue(List<string> hand)
        // {
        //     
        // }
        
        
        // ====================================================================
        // TODO: TASK 6 - Create the DisplayHand() method here
        // ====================================================================
        // static void DisplayHand(List<string> hand, string playerName)
        // {
        //     
        // }
        
        
        // ====================================================================
        // TODO: TASK 7 - Create the DetermineWinner() method here
        // ====================================================================
        // static string DetermineWinner(int playerValue, int dealerValue)
        // {
        //     
        // }
        
        
        // ====================================================================
        // TODO: TASK 8 - Create the GetPlayerAction() method here
        // ====================================================================
        // static string GetPlayerAction()
        // {
        //     
        // }
        
        
        // ====================================================================
        // TODO: TASK 9 - Create the PlayAgain() method here
        // ====================================================================
        // static bool PlayAgain()
        // {
        //     
        // }
        
    }
}
```