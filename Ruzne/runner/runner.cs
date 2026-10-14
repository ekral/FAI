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


bool jmenoPublic = typeof(Zamestnanec).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(p => p.Name).SequenceEqual(["Jmeno"]);
var t1 = new TestResult("Jmeno pristupne vsude", "True", jmenoPublic.ToString(), jmenoPublic);

bool platProtected = typeof(Zamestnanec).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(f => f.IsFamily).Select(p => p.Name).Contains("plat");
var t2 = new TestResult("plat pristupny u rodice a potomka", "True", platProtected.ToString(), platProtected);

bool hesloPrivate = typeof(Zamestnanec).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(f => f.IsPrivate).Select(p => p.Name).Contains("heslo");
var t3 = new TestResult("heslo pristupne pouze uvnitr rodice", "True", hesloPrivate.ToString(), hesloPrivate);

bool dedicnost = typeof(Zamestnanec).IsAssignableFrom(typeof(Manazer));
var t4 = new TestResult("Manazer je Zamestnanec", "True", dedicnost.ToString(), dedicnost);

var t5 = TestCase.TestZamestnanec("Petr");
var t6 = TestCase.TestManazer("Jiri");

TestResult[] testResults = [t1, t2, t3, t4, t5, t6];

int body = 0;

if(t1.IsCorrect) body += 1;
if(t2.IsCorrect) body += 1;
if(t3.IsCorrect) body += 1;
if(t4.IsCorrect) body += 1;
if(t5.IsCorrect) body += 1;
if(t6.IsCorrect) body += 1;

double fraction = body / 6.0;

Console.SetOut(stdout);

var header = new JsonArray("Test","Ocekavana hodnota","Vysledek","IsCorrect");
var data = testResults.Select(x => new JsonArray(x.Description, x.Expected, x.ActualSafe, x.IsCorrect));

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
    public static TestResult TestZamestnanec(string jmeno)
    {
        string description = "Create Zamestnanec";

        TestConsole.Clear();

        Reference.Zamestnanec expectedObject = Reference.Solution.CreateZamestnanec(jmeno);

        expectedObject.VypisHeslo();

        string expected = TestConsole.Message;

        TestConsole.Clear();

        Zamestnanec actualObject = CreateZamestnanec(jmeno);
        
        actualObject.VypisHeslo();

        string actual = TestConsole.Message;

        TestConsole.Clear();

        bool isCorrect = expected == actual;

        string actualSafe = System.Net.WebUtility.HtmlEncode(actual.Length > 128 ? actual[..128] + ".." : actual);

        return new TestResult(description, expected, actualSafe, isCorrect);
    }

    public static TestResult TestManazer(string jmeno)
    {
        string description = "Create Manazer";

        TestConsole.Clear();

        Reference.Manazer expectedObject = Reference.Solution.CreateManazer(jmeno);

        expectedObject.VypisUdaje();

        string expected = TestConsole.Message;

        TestConsole.Clear();

        Manazer actualObject = CreateManazer(jmeno);
        
        actualObject.VypisUdaje();

        string actual = TestConsole.Message;

        TestConsole.Clear();

        bool isCorrect = expected == actual;

        string actualSafe = System.Net.WebUtility.HtmlEncode(actual.Length > 128 ? actual[..128] + ".." : actual);

        return new TestResult(description, expected, actualSafe, isCorrect);
    }

    public static Zamestnanec CreateZamestnanec(string jmeno)
    {
        return new Zamestnanec() { Jmeno = jmeno};
    }

    public static Manazer CreateManazer(string jmeno)
    {
        return new Manazer() { Jmeno = jmeno};
    }
}


namespace Reference
{
    class Solution
    {
        public static Reference.Zamestnanec CreateZamestnanec(string jmeno)
        {
            return new Reference.Zamestnanec() { Jmeno = jmeno};
        }

        public static Reference.Manazer CreateManazer(string jmeno)
        {
            return new Reference.Manazer() { Jmeno = jmeno};
        }
    }

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
