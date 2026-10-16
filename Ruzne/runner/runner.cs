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

var vozidloExpectedProperties = GetProperties(typeof(Reference.Vozidlo));
var vozidloExpectedConstructors = GetConstructorsTypes(typeof(Reference.Vozidlo));
var nakladniVozidloExpectedProperties = GetProperties(typeof(Reference.NakladniVozidlo));
var nakladniVozidloAllExpectedProperties = GetAllProperties(typeof(Reference.NakladniVozidlo));
var nakladniVozidloExpectedConstructors = GetConstructorsTypes(typeof(Reference.NakladniVozidlo));

var t1 = new TestResult("Vozidlo definovano", true.ToString(), false.ToString(), false);
var t2 = new TestResult("Vozidlo properties", string.Join(", ", vozidloExpectedProperties), "can not evaluate", false);
var t3 = new TestResult("Vozidlo constructor types", string.Join(", ", vozidloExpectedConstructors), "can not evaluate", false);
var t4 = new TestResult("NakladniVozidlo definovano", true.ToString(), false.ToString(), false);
var t5 = new TestResult("NakladniVozidlo properties", string.Join(", ", nakladniVozidloExpectedProperties), "can not evaluate", false);
var t6 = new TestResult("NakladniVozidlo constructor types", string.Join(", ", nakladniVozidloExpectedConstructors), "can not evaluate", false);
var t7 = new TestResult("NakladniVozidlo dedi od Vozidla", true.ToString(), "can not evaluate", false);
object[] parametersA = ["Tatra", "Phoenix", 11000];
var t8 = new TestResult($"""new NakladniVozidlo({string.Join(", ", parametersA)})""", TestRun(typeof(Reference.NakladniVozidlo), parametersA, nakladniVozidloAllExpectedProperties), "can not evaluate", false);
object[] parametersB = ["Avia", "Avia A15", 1810];
var t9 = new TestResult($"""new NakladniVozidlo({string.Join(", ", parametersB)})""", TestRun(typeof(Reference.NakladniVozidlo), parametersB, nakladniVozidloAllExpectedProperties), "can not evaluate", false);


if(Type.GetType("Vozidlo") is Type vozidloType)
{
    t1.SetCorrectAndActual(true);

    var vozidloActualProperties = GetProperties(vozidloType);

    t2.Actual = string.Join(", ", vozidloExpectedProperties);
    t2.IsCorrect = new HashSet<(string, string)>(vozidloExpectedProperties).SetEquals(vozidloActualProperties);

    var vozidloActualConstructors = GetConstructorsTypes(vozidloType);

    t3.Actual = string.Join(", ", vozidloActualConstructors);
    t3.IsCorrect = vozidloExpectedConstructors.SequenceEqual(vozidloActualConstructors);
}

if(Type.GetType("NakladniVozidlo") is Type nakladniVozidloType)
{
    t4.SetCorrectAndActual(true);

    var nakladniVozidloActualProperties = GetProperties(nakladniVozidloType);

    t5.Actual = string.Join(", ", nakladniVozidloActualProperties);
    t5.IsCorrect = new HashSet<(string, string)>(nakladniVozidloExpectedProperties).SetEquals(nakladniVozidloActualProperties);

    var nakladniVozidloActualConstructors = GetConstructorsTypes(nakladniVozidloType);

    t6.Actual = string.Join(", ", nakladniVozidloActualConstructors);
    t6.IsCorrect = nakladniVozidloExpectedConstructors.SequenceEqual(nakladniVozidloActualConstructors);

    if(Type.GetType("Vozidlo") is Type vozidloTypeIndividual)
    {  
        t7.SetCorrectAndActual(nakladniVozidloType.BaseType == vozidloTypeIndividual);

        string actualRunA = TestRun(nakladniVozidloType, parametersA, nakladniVozidloAllExpectedProperties);

        t8.Actual = actualRunA;
        t8.IsCorrect = t8.Expected == actualRunA;

        string actualRunB = TestRun(nakladniVozidloType, parametersB, nakladniVozidloAllExpectedProperties);

        t9.Actual = actualRunB;
        t9.IsCorrect = t9.Expected == actualRunB;
    }
}


TestResult[] testResults = [t1, t2, t3, t4, t5, t6, t7, t8, t9];

int body = 0;

if(t1.IsCorrect) body += 1;
if(t2.IsCorrect) body += 1;
if(t3.IsCorrect) body += 1;
if(t4.IsCorrect) body += 1;
if(t5.IsCorrect) body += 1;
if(t6.IsCorrect) body += 1;
if(t7.IsCorrect) body += 1;
if(t8.IsCorrect) body += 1;
if(t9.IsCorrect) body += 1;

double fraction = body / 9.0;

Console.SetOut(stdout);

var header = new JsonArray("Test","Ocekavana hodnota","Vysledek","IsCorrect");
var data = testResults.Select(x => new JsonArray(x.Description, x.Expected, x.Actual, x.IsCorrect));

var json = new JsonObject()
{
  ["fraction"] = fraction,
  ["testresults"] = new JsonArray([header, ..data])
};

Console.WriteLine(json.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));

IEnumerable<(string, string)> GetProperties(Type type)
{
    var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(p => (p.Name, p.PropertyType.Name));
    return properties;
}

IEnumerable<(string, string)> GetAllProperties(Type type)
{
    var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance ).Select(p => (p.Name, p.PropertyType.Name));
    return properties;
}

IEnumerable<string> GetConstructorsTypes(Type type)
{
    var types = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(c => string.Join(", ", c.GetParameters().Select(p => p.ParameterType.Name)));

    return types;
}

string TestRun(Type nakladniVozidloType, object[] parameters, IEnumerable<(string, string)> nakladniVozidloExpectedProperties)
{
    StringBuilder sb = new();

    try
    {
        object? instance = Activator.CreateInstance(nakladniVozidloType, parameters);

        if(instance is null)
        {
            return $"Can not create instance of {nakladniVozidloType.Name}";
        }

        foreach(var (name, typeName) in nakladniVozidloExpectedProperties)
        {
            PropertyInfo? property = nakladniVozidloType.GetProperty(name);

            if(property is null)
            {
                return $"Missing property {name}";
            }

            var propertyType = property.PropertyType;

            object? value = property.GetValue(instance);

            if(value is null)
            {
                return $"Property {name} is null";
            }

            sb.Append($"{name}: {value} ");
        }
    }
    catch(Exception ex)
    {
        return $"run exception: {ex.Message}";
    }

    string result = sb.ToString();

    return System.Net.WebUtility.HtmlEncode(result.Length > 128 ? result[..128] + ".." : result);
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

class TestResult(string description, string expected, string actual, bool isCorrect)
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
    class Vozidlo
    {
        public string Znacka {get; } = string.Empty;
        public string Model {get; } = string.Empty;
        
        public Vozidlo(string znacka, string model)
        {
            Znacka = znacka;
            Model = model;
        }
    }

    class NakladniVozidlo : Vozidlo
    {
        public int Nosnost {get; }

        public NakladniVozidlo(string znacka, string model, int nosnost) : base(znacka, model)
        {
            Nosnost = nosnost;
        }
    }
}
