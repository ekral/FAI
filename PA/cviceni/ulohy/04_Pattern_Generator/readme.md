# 04 Pattern Generator

## 🔢 Popis úkolu

Vytváříte aplikaci pro generování obrazců na konzoli.
Pro práci na úkolu použijte přiložený soubor Program.cs, který obsahuje základ aplikace, do kterého následně přidáte další kód, 
podle zadání.


Odevzdejte jak soubor Program.cs, tak .pdf soubor s vypracovaným protokolem k zadání.
(Úvod, Postup práce včetně zdrojů ze kterých jste čerpal(a) inspiraci a Závěr, kde shrnete své pocity a trable z vypracovávání úkolu).


⌛ Celková náročnost úkolu = cca 2 hodiny

## 🚀 Výchozí kód v C# se zadáním úkolů

```csharp
/*
 * ====================================================================
 *                    PATTERN GENERATOR ASSIGNMENT
 * ====================================================================
 * 
 * DESCRIPTION:
 * Create a console application that generates various text-based patterns
 * based on user input. This assignment focuses on practicing While, For,
 * and Do-While loops.
 * 
 * ESTIMATED TIME: 2 hours
 * 
 * ====================================================================
 * REQUIRED TASKS:
 * ====================================================================
 * 
 * TASK 1: Implement the Main Menu (Do-While Loop)
 * -------
 * Create a menu system that displays pattern options and keeps running
 * until the user chooses to exit. The menu should:
 * - Display all available pattern options
 * - Use a do-while loop to ensure the menu shows at least once
 * - Handle invalid menu selections gracefully
 * 
 * Menu options should include:
 * 1. Right Triangle
 * 2. Inverted Right Triangle
 * 3. Pyramid
 * 4. Diamond
 * 5. Exit
 * 
 * 
 * TASK 2: Get Valid User Input (While Loop)
 * -------
 * After the user selects a pattern, ask for the pattern size (number of rows).
 * - Use a while loop to validate input
 * - Only accept positive integers between 1 and 20
 * - Keep asking until valid input is provided
 * 
 * 
 * TASK 3: Generate Right Triangle Pattern (For Loop)
 * -------
 * When option 1 is selected, use nested for loops to generate a right triangle.
 * Example output for size = 5:
 * *
 * **
 * ***
 * ****
 * *****
 * 
 * 
 * TASK 4: Generate Inverted Right Triangle (For Loop)
 * -------
 * When option 2 is selected, use nested for loops to generate an inverted triangle.
 * Example output for size = 5:
 * *****
 * ****
 * ***
 * **
 * *
 * 
 * 
 * TASK 5: Generate Pyramid Pattern (For Loop)
 * -------
 * When option 3 is selected, use nested for loops to generate a pyramid.
 * This requires printing spaces before stars to center the pattern.
 * Example output for size = 5:
 *     *
 *    ***
 *   *****
 *  *******
 * *********
 * 
 * 
 * TASK 6: Generate Diamond Pattern (For Loop)
 * -------
 * When option 4 is selected, use nested for loops to generate a diamond.
 * Combine pyramid logic for top half and inverted for bottom half.
 * Example output for size = 5:
 *     *
 *    ***
 *   *****
 *  *******
 * *********
 *  *******
 *   *****
 *    ***
 *     *
 * 
 * ====================================================================
 * BONUS TASKS (Optional - for extra credit):
 * ====================================================================
 * 
 * BONUS 1: Number Patterns
 * --------
 * Add menu options for number-based patterns instead of stars.
 * Example right triangle with numbers for size = 5:
 * 1
 * 12
 * 123
 * 1234
 * 12345
 * 
 * 
 * BONUS 2: Hollow Patterns
 * --------
 * Create hollow versions of the patterns where only the outline is shown.
 * Example hollow pyramid for size = 5:
 *     *
 *    * *
 *   *   *
 *  *     *
 * *********
 * 
 * 
 * BONUS 3: Custom Character
 * --------
 * Allow users to choose which character to use for the pattern
 * (instead of always using '*')
 * 
 * 
 * BONUS 4: Color Patterns
 * --------
 * Use Console.ForegroundColor to make patterns display in different colors.
 * Alternate colors for each row or create a rainbow effect.
 * 
 * 
 * ====================================================================
 * HINTS:
 * ====================================================================
 * - For pyramids, you need to print (size - currentRow) spaces before stars
 * - For diamonds, the top half is a pyramid, bottom half is inverted
 * - Use nested loops: outer loop for rows, inner loop(s) for characters
 * - Test with small sizes first (like 3 or 4) before trying larger patterns
 * - Use Console.WriteLine() for new lines, Console.Write() to stay on same line
 * - Remember to declare variables before the do-while loop if you need them outside
 * 
 * ====================================================================
 */

using System;

namespace PatternGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Pattern Generator!");
            Console.WriteLine("==================================\n");
            
            // TODO: TASK 1 - Declare variables you'll need for the menu
            // Example: int choice; int size;
            
            
            // TODO: TASK 1 - Implement the main menu using a do-while loop
            // The menu should keep running until the user chooses option 5 (Exit)
            
            // do
            // {
            //     Display menu options here
            //     Get user's menu choice
            //     
            //     Use if-else or switch to handle different choices
            //     
            //     For choices 1-4:
            //         - TASK 2: Get valid size using while loop
            //         - TASK 3-6: Generate the appropriate pattern using for loops
            //     
            //     For choice 5: Exit the program
            //     
            // } while (user hasn't chosen to exit);
            
            
            Console.WriteLine("\nThank you for using Pattern Generator!");
        }
    }
}
```