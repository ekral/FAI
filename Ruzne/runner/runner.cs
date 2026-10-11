using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

var stdout = Console.Out;
var dummyWritter = new StringWriter();
Console.SetOut(dummyWritter);

var t1 = new TestCase(2, 3);
var t2 = new TestCase(2, 4);
var t3 = new TestCase(2, 8);
var t4 = new TestCase(Random.Shared.Next(3, 10), 2);
string code = File.ReadAllText("../../../solution.cs");

bool withoutMathPow = !Regex.IsMatch( code, @"\bMath\s*\.\s*Pow\b");

double fraction = (t1.IsCorrect && t2.IsCorrect && t3.IsCorrect && t4.IsCorrect && withoutMathPow) ? 1.0 : 0.0;

Console.SetOut(stdout);

var result = new
{
    fraction,
    testresults = new object[]
    {
        new[] { "Test", "Ocekavana hodnota", "Vysledek", "iscorrect" },
        new object[] { t1.Name, t1.Expected, t1.ActualDisplay, t1.IsCorrect },
        new object[] { t2.Name, t2.Expected, t2.ActualDisplay, t2.IsCorrect },
        new object[] { t3.Name, t3.Expected, t3.ActualDisplay, t3.IsCorrect },
        new object[] { t4.Name, t4.Expected, t4.ActualDisplay, t4.IsCorrect },
        new object[] { "Math.Pow", "bez pouziti Math.Pow", withoutMathPow ? "ano" : "ne", withoutMathPow }
    }
};

Console.WriteLine(JsonSerializer.Serialize(result));

public static class TestConsole
{
    public static string Message {get; set;} = string.Empty;

    public static void WriteLine(string message)
    {
        Message += message;
    }
}

class TestCase
{
    private const int DisplayLimit = 32;

    public string Name { get; }
    public string Expected { get; }
    public string ActualDisplay { get; }
    public bool IsCorrect { get; }
    public string Message { get => IsCorrect ? "prosel" : "chyba"; } 

    public TestCase(int x, int y)
    {
        Name = $"{nameof(Test)}({x}, {y})";

        TestConsole.Message = string.Empty;

        Test(x, y   );

        Expected = TestConsole.Message;

        TestConsole.Message = string.Empty;

        Submission.Test(x, y);

        string actual = TestConsole.Message;
 
        IsCorrect = Expected == actual;

        ActualDisplay = actual.Length > DisplayLimit ? actual[..DisplayLimit] + "…" : actual;
    }

    public static void Test(int x, int y)
    {
        int mocnina = x;

        for (int i = 0; i < y; i++)
        {
            mocnina *= x;
        }

        TestConsole.WriteLine($"mocnina: {mocnina}");
    }
}
