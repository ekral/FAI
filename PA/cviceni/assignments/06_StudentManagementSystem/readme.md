# Student Management system

## 🔢 Popis úkolu

Vytváříte aplikaci pro správu studentů a předmětů.
Pro práci na úkolu použijte přiložený soubor Program.cs, který obsahuje základ aplikace, do kterého následně přidáte další kód, 
podle zadání.

Odevzdejte jak soubory zkomprimované do formátu .zip nebo .7z, tak .pdf soubor s vypracovaným protokolem k zadání.
(Úvod, Postup práce včetně zdrojů ze kterých jste čerpal(a) inspiraci a Závěr, kde shrnete své pocity a trable z vypracovávání úkolu).

⌛ Celková náročnost úkolu = cca 2 hodiny


## 🚀 Výchozí kód v C# se zadáním úkolů

```csharp
/*
 * ====================================================================
 *               STUDENT MANAGEMENT SYSTEM ASSIGNMENT
 * ====================================================================
 * 
 * DESCRIPTION:
 * Create a console application that manages students, courses, and
 * enrollments. This assignment focuses on practicing Classes, Structs,
 * and file organization. You will create separate files for each class
 * and struct, and interact with objects using the dot operator.
 * 
 * ====================================================================
 * FILE STRUCTURE REQUIREMENTS:
 * ====================================================================
 * Start with creating a proper folder structure
 * YourApp
 * ├─ Classes
 * ├─ Structs
 * ├─ Enums
 * ├─ Program.cs 
 * 
 * Then create the following separate files in your project:
 * 
 * 1. Student.cs - Contains the Student class
 * 2. Course.cs - Contains the Course class
 * 3. Enrollment.cs - Contains the Enrollment struct
 * 
 * Your Folder structure should look like this:
 * YourApp
 * ├─ Classes (Namespace = YourApp.Classes)
 * │    ├─ Student.cs
 * │    ├─ Course.cs
 * │ 
 * ├─ Enums (Namespace = YourApp.Enums)
 * │    ├─ Year.cs
 * │    ├─ Grade.cs
 * │ 
 * ├─ Structs (Namespace = YourApp.Structs)
 * │    ├─ Enrollment.cs
 * │ 
 * ├─ Program.cs (Namespace = YourApp)
 * 
 * ====================================================================
 * REQUIRED TASKS:
 * ====================================================================
 * 
 * TASK 1: Create the Student Class (in Student.cs)
 * -------
 * Create a new file called Student.cs and define a class with these fields:
 * - public string Name;
 * - public string Email;
 * - public Year Year;  // 1, 2, 3, or 4 (Enum)
 * 
 * Remember:
 * - Use "public class Student"
 * - All fields should be public
 * - Make sure it's in the proper namespace
 * - Don't forget to implement the Year enum in proper file and import it via "using"
 * 
 * 
 * TASK 2: Create the Course Class (in Course.cs)
 * -------
 * Create a new file called Course.cs and define a class with these fields:
 * - public string CourseName;
 * - public int Credits;
 * - public string InstructorName;
 * 
 * 
 * TASK 3: Create the Enrollment Struct (in Enrollment.cs)
 * -------
 * Create a new file called Enrollment.cs and define a struct with these fields:
 * - public string StudentEmail;
 * - public string CourseName;
 * - public string EnrollmentDate;  // Store as string for simplicity
 * - public string Grade;  // "A", "B", "C", "D", "F", or "Not Graded"
 * 
 * Use "public struct Enrollment" instead of class.
 * Structs are good for small, simple data structures like this.
 * 
 * 
 * TASK 4: Set Up Data Storage in Program.cs
 * -------
 * In the Main method, create three lists to store your data:
 * - List<Student> students = new List<Student>();
 * - List<Course> courses = new List<Course>();
 * - List<Enrollment> enrollments = new List<Enrollment>();
 * 
 * These should be declared BEFORE your do-while menu loop.
 * 
 * 
 * TASK 5: Implement the Main Menu
 * -------
 * Create a menu system with these options:
 * 1. Add New Student
 * 2. Add New Course
 * 3. Enroll Student in Course
 * 4. Assign Grade to Student
 * 5. View All Students
 * 6. View All Courses
 * 7. View Student Enrollments
 * 8. View Course Enrollments
 * 9. View Statistics
 * 10. Exit
 * 
 * Use a do-while loop to keep the menu running.
 * 
 * 
 * TASK 6: Add New Student (Menu Option 1)
 * -------
 * When user selects option 1:
 * - Create a new Student object: Student newStudent = new Student();
 * - Ask user for Name and assign: newStudent.Name = Console.ReadLine();
 * - Ask for Email and assign: newStudent.Email = ...
 * - Ask for Year (1-4) and assign: newStudent.Year = ...
 * - Use a while loop to validate Year is between 1 and 4
 * - Add to list: students.Add(newStudent);
 * - Display confirmation message
 * 
 * 
 * TASK 7: Add New Course (Menu Option 2)
 * -------
 * When user selects option 2:
 * - Create a new Course object
 * - Ask for and assign: CourseName, Credits, Instructor
 * - Use while loop to validate Credits is between 1 and 6
 * - Add to courses list
 * - Display confirmation
 * 
 * 
 * TASK 8: Enroll Student in Course (Menu Option 3)
 * -------
 * When user selects option 3:
 * - First, display all students in following pattern: "[Index] Email - Student name" (use for loop)
 * - Ask user to enter student index and validate if it has been entered properly with while loop
 * - Display all courses in following patter: "[Index] CourseName (use for loop)
 * - Ask user to choose course
 * - Create new Enrollment struct
 * - Assign Email, CourseName, today's date, and "Not Graded" for Grade
 * - Add to enrollments list
 * - Display confirmation
 * 
 * 
 * TASK 9: Assign Grade (Menu Option 4)
 * -------
 * When user selects option 4:
 * - Display all enrollments that have "Not Graded" status in following patter: "[Index] Email - CourseName"
 * - Ask user which enrollment to grade (by index)
 * - Ask for grade (A, B, C, D, or F)
 * - Find the enrollment in the list and update its Grade field
 * - Note: With structs, you need to update the list item, not just the struct
 *   Example: enrollments[index] = updatedEnrollment;
 * - Display confirmation
 * 
 * 
 * TASK 10: View All Students (Menu Option 5)
 * -------
 * When user selects option 5:
 * - Use a foreach loop to display all students
 * - For each student, use the dot operator to access fields
 * - Display in format: "Name: [Name], Email: [Email], Year: [Year]"
 * - If no students, display "No students registered"
 * 
 * 
 * TASK 11: View All Courses (Menu Option 6)
 * -------
 * When user selects option 6:
 * - Use foreach loop to display all courses
 * - Display: CourseName,  Credits, Instructor
 * - Format nicely for readability
 * 
 * 
 * TASK 12: View Student Enrollments (Menu Option 7)
 * -------
 * When user selects option 7:
 * - Ask for Email
 * - Use for loop to go through enrollments list
 * - Display all enrollments for that student
 * - For each enrollment, also display the instructor name (search courses list)
 * - Show:  CourseName, InstructorName, Grade
 * 
 * 
 * TASK 13: View Course Enrollments (Menu Option 8)
 * -------
 * When user selects option 8:
 * - Ask for CourseName
 * - Use for loop to find all enrollments for that course
 * - For each enrollment, display student name (search students list)
 * - Show: Email, StudentName, Grade
 * 
 * 
 * TASK 14: View Statistics (Menu Option 9)
 * -------
 * Display the following statistics:
 * - Total number of students
 * - Total number of courses
 * - Total number of enrollments
 * - Number of students per year (1st year, 2nd year, etc.)
 * - Number of graded vs ungraded enrollments
 * Use for or foreach loops to count each category.
 * 
 * ====================================================================
 * BONUS TASKS (Optional - for extra credit):
 * ====================================================================
 * 
 * BONUS 1: Search Functionality
 * --------
 * Add menu options to search:
 * - Search student by name (partial match)
 * - Search course by name or code (partial match)
 * Display matching results.
 * 
 * 
 * BONUS 2: Grade Point Average (GPA)
 * --------
 * Calculate and display GPA for a student:
 * - A = 4.0, B = 3.0, C = 2.0, D = 1.0, F = 0.0
 * - GPA = (sum of grade points × credits) / total credits
 * - You'll need to look up course credits for each enrollment
 * 
 * 
 * 
 * 
 * BONUS 3: Remove/Unenroll
 * --------
 * Add options to:
 * - Remove a student (and all their enrollments)
 * - Remove a course (and all enrollments in it)
 * - Unenroll a student from a specific course
 * 
 * 
 * BONUS 4: Use Tuples for Return Data
 * --------
 * Practice using tuples:
 * - When displaying enrollment details, return a tuple with
 *   (StudentName, CourseName, Grade)
 * - Example: var info = (student.Name, course.CourseName, enrollment.Grade);
 * 
 * 
 * ====================================================================
 * HINTS:
 * ====================================================================
 * - To access object fields: student.Name, course.Credits, etc.
 * - To create objects: Student s = new Student();
 * - To create structs: Enrollment e = new Enrollment();
 * - When updating structs in a list, assign back: list[i] = updatedStruct;
 * - Use foreach when you just need to read all items
 * - Use for when you need the index or need to modify items
 * - Remember to validate user input with while loops
 * - Test with a few items first before implementing all features
 * - Make sure all your classes/structs are in the correct namespaces and you have set up correct usings
 * 
 * COMMON MISTAKES TO AVOID:
 * - Forgetting to add "using System.Collections.Generic;" for List
 * - Trying to modify struct fields directly in foreach (won't work!)
 * - Not validating that StudentID/CourseCode exists before enrolling
 * 
 * ====================================================================
 */

using System;
using System.Collections.Generic;

namespace StudentManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("================================================");
            Console.WriteLine("       STUDENT MANAGEMENT SYSTEM");
            Console.WriteLine("================================================\n");
            
            // TODO: TASK 4 - Create your three lists here
            // List<Student> students = new List<Student>();
            // List<Course> courses = new List<Course>();
            // List<Enrollment> enrollments = new List<Enrollment>();
            
            
            // TODO: TASK 5 - Create variable for menu choice
            
            
            // TODO: TASK 5 - Implement the main menu using do-while loop
            // do
            // {
            //     Display menu options (1-10)
            //     Get user's choice
            //     
            //     Use if-else or switch to handle each menu option:
            //     
            //     Case 1: TASK 6 - Add New Student
            //     Case 2: TASK 7 - Add New Course
            //     Case 3: TASK 8 - Enroll Student in Course
            //     Case 4: TASK 9 - Assign Grade
            //     Case 5: TASK 10 - View All Students
            //     Case 6: TASK 11 - View All Courses
            //     Case 7: TASK 12 - View Student Enrollments
            //     Case 8: TASK 13 - View Course Enrollments
            //     Case 9: TASK 14 - View Statistics
            //     Case 10: Exit
            //     
            // } while (user hasn't chosen to exit);
            
            
            Console.WriteLine("\n================================================");
            Console.WriteLine("   Thank you for using Student Management System!");
            Console.WriteLine("================================================");
        }
    }
}
```