using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Json.Nodes;

var stdout = Console.Out;
var dummyWritter = new StringWriter();
Console.SetOut(dummyWritter);

var t1 = TestCase.Test(2);
var t2 = TestCase.Test(20);
var t3 = TestCase.Test(60);
var t4 = TestCase.Test(Random.Shared.Next(100, 624));

string code = File.ReadAllText("../../../solution.cs");
bool withoutMathPow = !Regex.IsMatch( code, @"\bMath\s*\.\s*Pow\b");

var t5 = new TestResult("Math.Pow", "bez pouziti Math.Pow", withoutMathPow ? "ano" : "ne", withoutMathPow);

TestResult[] testResults = [t1, t2, t3, t4, t5];
double fraction = testResults.All(x => x.IsCorrect) ? 1.0 : 0.0;

Console.SetOut(stdout);

var header = new JsonArray("Test","Ocekavana hodnota","Vysledek","IsCorrect");
var data = testResults.Select(x => new JsonArray(x.Description, x.Expected, x.ActualSafe, x.IsCorrect));

var json = new JsonObject()
{
  ["fraction"] = fraction,
  ["testresults"] = new JsonArray([header, ..data])
};

Console.WriteLine(json.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));

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

record TestResult(string Description, string Expected, string ActualSafe, bool IsCorrect);

static class TestCase
{
    public static TestResult Test(int cislo)
    {
        string description = $"{nameof(Run)}({cislo})";

        TestConsole.Clear();

        Run(cislo);

        string expected = TestConsole.Message;

        TestConsole.Clear();

        Submission.Run(cislo);

        string actual = TestConsole.Message;

        const int limit = 48;
        string actualSafe = System.Net.WebUtility.HtmlEncode(actual.Length > limit ? actual[..limit] + "…" : actual);
        
        bool isCorrect = expected == actual;

        return new TestResult(description, expected, actualSafe, isCorrect);
    }

    public static void Run(int cislo)
    {
        int pocetNasobeni = 0;
        int mocnina = 1;

        while (mocnina <= cislo)
        {
            pocetNasobeni++;
            mocnina *= 2;
        }

        TestConsole.WriteLine($"m: {mocnina}, pn: {pocetNasobeni}");
    }
}
