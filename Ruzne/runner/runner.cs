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

var t1 = new TestResult("Zamestnanec definovan", true.ToString(), false.ToString(), false);
var t2 = new TestResult("Jmeno pristupne vsude", true.ToString(), false.ToString(), false);
var t3 = new TestResult("plat pristupny u rodice a potomka", true.ToString(), false.ToString(), false);
var t4 = new TestResult("heslo pristupne pouze uvnitr rodice", true.ToString(), false.ToString(), false);
var t5 = new TestResult("Manazer je definovan", true.ToString(), false.ToString(), false);
var t6 = new TestResult("Manazer je Zamestnanec", true.ToString(), false.ToString(), false);
var t7 = new TestResult("zamestnanec.VypisHeslo()", TestRun(typeof(Reference.Zamestnanec), "VypisHeslo", "Petr"), "run error", false);
var t8 = new TestResult("manazer.VypisUdaje()", TestRun(typeof(Reference.Manazer), "VypisUdaje", "Petr"), "run error", false);

if(Type.GetType("Zamestnanec") is Type zamestnanecType)
{
    t1.SetCorrectAndActual(true);
 
    if(zamestnanecType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).FirstOrDefault(p => p.Name == "Jmeno") is PropertyInfo jmenoPrope)
    {
        t2.SetCorrectAndActual(true);
    }

    if(zamestnanecType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).FirstOrDefault(f => f.Name == "plat" && f.IsFamily) is FieldInfo platField)
    {
        t3.SetCorrectAndActual(true);
    }

    if(zamestnanecType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).FirstOrDefault(f => f.Name == "heslo" && f.IsPrivate) is FieldInfo hesloField)
    {
        t4.SetCorrectAndActual(true);
    }

    t7.Actual = TestRun(zamestnanecType, "VypisHeslo", "Petr");
    t7.IsCorrect = t7.Actual == t7.Expected;

    if(Type.GetType("Manazer") is Type manazerType)
    {
        t5.SetCorrectAndActual(true);

        if(manazerType.IsSubclassOf(zamestnanecType))
        {
            t6.SetCorrectAndActual(true);
        }

        t8.Actual = TestRun(manazerType, "VypisUdaje", "Petr");
        t8.IsCorrect = t8.Actual == t8.Expected;
    }
}


TestResult[] testResults = [t1, t2, t3, t4, t5, t6, t7, t8];

int body = 0;

if(t1.IsCorrect) body += 1;
if(t2.IsCorrect) body += 1;
if(t3.IsCorrect) body += 1;
if(t4.IsCorrect) body += 1;
if(t5.IsCorrect) body += 1;
if(t6.IsCorrect) body += 1;
if(t7.IsCorrect) body += 1;
if(t8.IsCorrect) body += 1;

double fraction = body / 8.0;

Console.SetOut(stdout);

var header = new JsonArray("Test","Ocekavana hodnota","Vysledek","IsCorrect");
var data = testResults.Select(x => new JsonArray(x.Description, x.Expected, x.Actual, x.IsCorrect));

var json = new JsonObject()
{
  ["fraction"] = fraction,
  ["testresults"] = new JsonArray([header, ..data])
};

Console.WriteLine(json.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));

TestResult TestTypes(Type expectedType, Type actualType)
{
    var expected = expectedType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(p => p.Name);
    var actual = actualType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(p => p.Name);

    return new TestResult(
        $"{expectedType.Name} properties",
        string.Join(", ", expected),
        string.Join(", ", actual),
        new HashSet<string>(expected).SetEquals(actual)
    );
}

TestResult TestConstructors(Type expectedType, Type actualType)
{
    var expected = expectedType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(c => string.Join(", ", c.GetParameters().Select(p => p.ParameterType.Name)));
    var actual = actualType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(c => string.Join(", ", c.GetParameters().Select(p => p.ParameterType.Name)));

    return new TestResult(
        $"Typy konstruktoru tridy {expectedType.Name}",
        string.Join(", ", expected),
        string.Join(", ", actual),
        expected.SequenceEqual(actual)
    );
}

string TestRun(Type type, string method, string jmeno)
{
    TestConsole.Clear();

    string result = "run exception";

    try
    {
        object? expectedObject = Activator.CreateInstance(type);

        PropertyInfo? property = type.GetProperty("Jmeno");
        property?.SetValue(expectedObject, jmeno);

        MethodInfo? metodaExpected = type.GetMethod(method);
        metodaExpected?.Invoke(expectedObject, null);

        result = TestConsole.Message;
    }
    catch(Exception ex)
    {
        result = $"run exception: {ex.Message}";
    }

    TestConsole.Clear();

    return System.Net.WebUtility.HtmlEncode(result.Length > 128 ? result[..128] + ".." : result);;
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

record TestResult(string description, string expected, string actual, bool isCorrect)
{
    public string Description {get; set;} = description;
    public string Expected {get; set;} = expected;

    public string Actual {get; set;} = actual;
    public bool IsCorrect {get; set;} = isCorrect;

    public void SetCorrectAndActual(bool isCorrect)
    {
        IsCorrect = isCorrect;
        Actual = isCorrect.ToString();
    }
}

namespace Reference
{
    class Zamestnanec
    {
        public string Jmeno {get; set;} = string.Empty;
        protected decimal plat = 50000m;
        private string heslo = "abc123";

        public void VypisHeslo()
        {
            TestConsole.WriteLine($"Heslo: {heslo}");
        }
    }

    class Manazer : Zamestnanec
    {
        public void VypisUdaje()
        {
            TestConsole.WriteLine($"Jmeno: {Jmeno}, Plat: {plat}");
        }
    }
}
