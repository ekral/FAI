#define ASSIGNMENT_REPORT

using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Json.Nodes;
using System.Text;

var stdout = Console.Out;
var dummyWritter = new StringWriter();
Console.SetOut(dummyWritter);

TestCase t1 = TestRun(0);
TestCase t2 = TestRun(1);
TestCase t3 = TestRun(7);
TestCase t4 = TestRun(Random.Shared.Next(8, 100));

TestCase[][] testCases = [[t1],[t2], [t3,t4]];

Console.SetOut(stdout);

var header = new JsonArray("Test", "Očekávaná hodnota", "Výsledek", "IsCorrect");

JsonArray array = [header];

int points = 0;
int maxPoints = 0;

foreach(var group in testCases)
{
    var missing = group.Where(tc => !tc.Result.IsCorrect);
    bool groupAllCorrect = !missing.Any();

    int groupMaxPoints = 0;

    foreach(TestCase tc in group)
    {
        groupMaxPoints += tc.MaxPoints;

        var jsonArray = groupAllCorrect switch
        {
            true => new JsonArray(tc.Result.Description, tc.Result.Expected, tc.Result.Actual, true),
            false => tc.Result.IsCorrect switch
            {
                true => new JsonArray(tc.Result.Description, tc.Result.Expected, $"{tc.Result.Actual} správně, ale chybí související testy: [{string.Join(", ", missing.Select(m => m.Result.Description))}]", true),
                false => new JsonArray(tc.Result.Description, tc.Result.Expected, tc.Result.Actual, false)
            }
        };

        array.Add(jsonArray);
    }

    maxPoints += groupMaxPoints;

    points += groupAllCorrect ? groupMaxPoints : 0;    
}

double fraction = (double)points / (double)maxPoints;


var json = new JsonObject()
{
  ["fraction"] = fraction,
  ["testresults"] = array
};

Console.WriteLine(json.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));

#if ASSIGNMENT_REPORT

Console.WriteLine($"Výsledné body se spočítají jako: (počet bodů za splněné kritéria / {maxPoints}) x celkový počet bodů za úlohu.");

int i = 1;

foreach(var group in testCases)
{
    string indentation = string.Empty;

    if(group.Count() > 1)
    {
        int groupMaxPoints = group.Sum(g => g.MaxPoints);
        Console.WriteLine($"Skupina úkolů, které musí být splněny současně (body: {groupMaxPoints}):");

        indentation = "  - ";
    }

    foreach(var testCase in group)
    {
        Console.Write($"{indentation}{i}. {testCase.Result.Description} (body: {testCase.MaxPoints}).");

        if(testCase.Dependencies.Length > 0)
        {
            Console.Write(" (závislosti: ");
            Console.Write(string.Join(", ", testCase.Dependencies.Select(d => d.Description)));
            Console.Write(")");
        }

        Console.Write(Console.Out.NewLine);

        i++;
    }
}

#endif

TestCase TestRun(int n)
{
    TestCase testCase = new TestCase(1, [], (deps) => 
    {
        string description = $"Test({n})";

        TestConsole.Clear();
        Reference.Test(n);
        string expected = TestConsole.Message;

        TestConsole.Clear();
        Submission.Test(n);
        string actual = TestConsole.Message;

        bool isCorrect = actual == expected;

        return new TestResult(description, expected, actual, isCorrect, 1);
    });

    return testCase;
}

class Reference
{
   public static void Test(int cislo)
    {
        int pocetDeleni = 0;

        while (cislo > 0)
        {
            pocetDeleni++;
            cislo /= 2;
        }

        TestConsole.WriteLine($"pocet deleni: {pocetDeleni}");
    }
}

record TestResult(string Description, string Expected, string Actual, bool IsCorrect, int Points);

class TestCase(int maxPoints, TestResult[] dependencies, Func<TestResult[], TestResult> testFunction)
{
    private TestResult? result = null;
    private readonly Func<TestResult[], TestResult> testFunction = testFunction;
    public int MaxPoints {get; } = maxPoints;
    public TestResult[] Dependencies {get; } = dependencies;

    public TestResult Result
    {
        get 
        {
            result ??= testFunction(Dependencies);
            
            return result;
        }
    }
}

static class TestConsole
{
    private readonly static StringWriter writer = new();

    public static string Message {get => writer.ToString();}

    public static void Clear()
    {
        writer.GetStringBuilder().Clear();
    }

    public static void WriteLine(string message)
    {
        writer.Write(message);
    }
}

