# Tasklist

Vytváříte aplikaci spravování úkolů (Tasklist).
Pro práci na úkolu použijte přiložený soubor Program.cs, který obsahuje základ aplikace, do kterého následně přidáte další kód, 
podle zadání.


Odevzdejte jak soubor Program.cs, tak .pdf soubor s vypracovaným protokolem k zadání.
(Úvod, Postup práce včetně zdrojů ze kterých jste čerpal(a) inspiraci a Závěr, kde shrnete své pocity a trable z vypracovávání úkolu).


Celková náročnost úkolu = cca 2 hodiny

```csharp
/*
 * ====================================================================
 *                    TASK LIST MANAGER ASSIGNMENT
 * ====================================================================
 * 
 * DESCRIPTION:
 * Create a console application that manages a to-do list with tasks.
 * Users can add tasks, mark them as complete, view tasks by priority,
 * and delete tasks. This assignment focuses on practicing Arrays, Lists,
 * and Foreach loops.
 * 
 * ====================================================================
 * REQUIRED TASKS:
 * ====================================================================
 * 
 * TASK 1: Set Up Data Structures
 * -------
 * Create the following collections to store your task data:
 * 
 * a) Create a string array called 'priorities' with 3 fixed values:
 *    "High", "Medium", "Low"
 *    (Use an array because priorities are fixed and won't change)
 * 
 * b) Create THREE List<string> collections:
 *    - taskNames: to store the names/descriptions of tasks
 *    - taskPriorities: to store the priority of each task
 *    - taskStatuses: to store whether task is "Pending" or "Completed"
 *    
 *    Note: All three lists should have the same count - use the same index
 *    to access related information (e.g., taskNames[0], taskPriorities[0],
 *    and taskStatuses[0] all refer to the same task)
 * 
 * 
 * TASK 2: Implement the Main Menu
 * -------
 * Create a menu system with the following options:
 * 1. Add New Task
 * 2. View All Tasks
 * 3. View Tasks by Priority
 * 4. Mark Task as Completed
 * 5. Delete Task
 * 6. View Statistics
 * 7. Exit
 * 
 * Use a do-while loop to keep the menu running until user chooses to exit.
 * 
 * 
 * TASK 3: Add New Task
 * -------
 * When user selects option 1:
 * - Ask for task name/description
 * - Display the priorities array using a for or foreach loop
 * - Ask user to select a priority (1-3)
 * - Use a while loop to validate that they enter 1, 2, or 3
 * - Add the task name to taskNames list
 * - Add the selected priority to taskPriorities list
 * - Add "Pending" status to taskStatuses list
 * - Confirm task was added successfully
 * 
 * 
 * TASK 4: View All Tasks
 * -------
 * When user selects option 2:
 * - Check if there are any tasks
 * - If no tasks, display "No tasks in the list"
 * - If tasks exist, display them in a formatted way:
 *   - Use a for loop to iterate through the lists
 *   - Display each task with: number, name, priority, status
 *   
 * Example output:
 * 1. [High] Buy groceries - Pending
 * 2. [Low] Read book - Completed
 * 3. [Medium] Call dentist - Pending
 * 
 * 
 * TASK 5: View Tasks by Priority
 * -------
 * When user selects option 3:
 * - Display the priorities array using foreach
 * - Ask user to select a priority
 * - Use a for loop to go through all tasks
 * - Display only tasks that match the selected priority
 * - Count how many tasks were found
 * - If none found, display appropriate message
 * 
 * 
 * TASK 6: Mark Task as Completed
 * -------
 * When user selects option 4:
 * - First, display all PENDING tasks only
 * - If no pending tasks, display message and return to menu
 * - Ask user to enter the task number to mark as complete
 * - Use while loop to validate the input
 * - Update the taskStatuses list at that index to "Completed"
 * - Display confirmation message
 * 
 * 
 * TASK 7: Delete Task
 * -------
 * When user selects option 5:
 * - Display all tasks with their numbers
 * - Ask user which task number to delete
 * - Use while loop to validate the input
 * - Remove the item at that index from ALL THREE lists:
 *   taskNames.RemoveAt(index);
 *   taskPriorities.RemoveAt(index);
 *   taskStatuses.RemoveAt(index);
 * - Display confirmation message
 * 
 * 
 * TASK 8: View Statistics
 * -------
 * When user selects option 6, display:
 * - Total number of tasks
 * - Number of completed tasks
 * - Number of pending tasks
 * - Number of high priority tasks
 * - Number of medium priority tasks
 * - Number of low priority tasks
 * 
 * Use for or foreach loops to count each category.
 * 
 * ====================================================================
 * BONUS TASKS (Optional):
 * ====================================================================
 * 
 * BONUS 1: Search Tasks
 * --------
 * Add a menu option to search for tasks by keyword.
 * - Ask user for a search term
 * - Use foreach to find all tasks containing that keyword (case-insensitive)
 * - Display matching tasks
 * Hint: Use .ToLower() and .Contains() methods
 * 
 * 
 * 
 * 
 * BONUS 2: Edit Task
 * --------
 * Add a menu option to edit an existing task:
 * - Let user select which task to edit
 * - Allow them to change the name and/or priority
 * - Keep the same status
 * 
 * 
 * BONUS 3: Save/Load Tasks
 * --------
 * Add options to:
 * - Save all tasks to a text file when exiting
 * - Load tasks from a text file when starting
 * Hint: Use System.IO.File.WriteAllLines() and File.ReadAllLines()
 * 
 * 
 * BONUS 4: Due Dates
 * --------
 * Add a fourth list for due dates:
 * - Let users optionally add a due date when creating a task
 * - Display tasks with their due dates
 * - Show overdue tasks in red (Console.ForegroundColor)

 * ====================================================================
 * HINTS:
 * ====================================================================
 * - Declare all your lists BEFORE the do-while loop so they persist
 * - Remember: taskNames[i], taskPriorities[i], and taskStatuses[i] all
 *   refer to the same task
 * - When deleting, subtract 1 from user's input (they see 1-based numbers,
 *   but lists use 0-based indexing)
 * - Use taskNames.Count to get the number of tasks
 * - For displaying priorities array: use foreach or for loop
 * - For working with task indices: use for loop (not foreach)
 * - Test with a few tasks first before implementing all features
 * 
 * ====================================================================
 */

using System;
using System.Collections.Generic;

namespace TaskListManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===================================");
            Console.WriteLine("   TASK LIST MANAGER");
            Console.WriteLine("===================================\n");
            
            // TODO: TASK 1 - Create your data structures here
            
            // Create the priorities array with 3 values: "High", "Medium", "Low"
            
            
            // Create three lists to store task information
            // List<string> taskNames = new List<string>();
            // List<string> taskPriorities = new List<string>();
            // List<string> taskStatuses = new List<string>();
            
            
            // TODO: TASK 2 - Create a variable for menu choice
            
            
            // TODO: TASK 2 - Implement the main menu using do-while loop
            // do
            // {
            //     Display menu options (1-7)
            //     Get user's choice
            //     
            //     Use if-else or switch to handle each menu option:
            //     
            //     Case 1: TASK 3 - Add New Task
            //     Case 2: TASK 4 - View All Tasks
            //     Case 3: TASK 5 - View Tasks by Priority
            //     Case 4: TASK 6 - Mark Task as Completed
            //     Case 5: TASK 7 - Delete Task
            //     Case 6: TASK 8 - View Statistics
            //     Case 7: Exit
            //     
            // } while (user hasn't chosen to exit);
            
            
            Console.WriteLine("\nThank you for using Task List Manager!");
            Console.WriteLine("Goodbye!");
        }
    }
}
```