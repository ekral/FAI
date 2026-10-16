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

IEnumerable<(string, string)> zakaznikExpectedProperties = GetProperties(typeof(Reference.Zakaznik));
var zakaznikExpectedConstructors = GetConstructorsTypes(typeof(Reference.Zakaznik));
var fakturaExpectedProperties = GetProperties(typeof(Reference.Faktura));
var fakturaExpectedConstructors = GetConstructorsTypes(typeof(Reference.Faktura));

var t1 = new TestResult("Zakaznik definovan", true.ToString(), false.ToString(), false);
var t2 = new TestResult("Zakaznik properties", string.Join(", ", zakaznikExpectedProperties), "can not evaluate", false);
var t3 = new TestResult("Zakaznik constructor types", string.Join(", ", zakaznikExpectedConstructors), "can not evaluate", false);
var t4 = new TestResult("Faktura properties", string.Join(", ", fakturaExpectedProperties), "can not evaluate", false);
var t5 = new TestResult("Faktura definovana", true.ToString(), false.ToString(), false);
var t6 = new TestResult("Faktura constructor types", string.Join(", ", fakturaExpectedConstructors), "can not evaluate", false);
object[] parametersA = [3, 1500.50m, "Petr", "petr@example.com"];
var t7 = new TestResult($"""new Faktura({string.Join(", ", parametersA)})""", TestRun(typeof(Reference.Faktura), typeof(Reference.Zakaznik), parametersA, fakturaExpectedProperties), "can not evaluate", false);
object[] parametersB = [2, 300.50m, "Alena", "alena@example.com"];
var t8 = new TestResult($"""new Faktura({string.Join(", ", parametersB)})""", TestRun(typeof(Reference.Faktura), typeof(Reference.Zakaznik), parametersB, fakturaExpectedProperties), "can not evaluate", false);


if(Type.GetType("Zakaznik") is Type zakaznikType)
{
    t1.SetCorrectAndActual(true);

    var zakaznikActualProperties = GetProperties(zakaznikType);

    t2.Actual = string.Join(", ", zakaznikActualProperties);
    t2.IsCorrect = new HashSet<(string, string)>(zakaznikExpectedProperties).SetEquals(zakaznikActualProperties);

    var zakaznikActualConstructors = GetConstructorsTypes(zakaznikType);

    t3.Actual = string.Join(", ", zakaznikActualConstructors);
    t3.IsCorrect = zakaznikExpectedConstructors.SequenceEqual(zakaznikActualConstructors);

    if(Type.GetType("Faktura") is Type fakturaType)
    {    
        string actualRunA = TestRun(fakturaType, zakaznikType, parametersA, fakturaExpectedProperties);

        t7.Actual = actualRunA;
        t7.IsCorrect = t7.Expected == actualRunA;

        string actualRunB = TestRun(fakturaType, zakaznikType, parametersB, fakturaExpectedProperties);

        t8.Actual = actualRunB;
        t8.IsCorrect = t8.Expected == actualRunB;
    }
}

if(Type.GetType("Faktura") is Type fakturaTypeIndividual)
{
    var fakturaActualProperties = GetProperties(fakturaTypeIndividual);

    t4.Actual = string.Join(", ", fakturaActualProperties);
    t4.IsCorrect = new HashSet<(string, string)>(fakturaExpectedProperties).SetEquals(fakturaActualProperties);

    t5.SetCorrectAndActual(true);

    var fakturaActualConstructors = GetConstructorsTypes(fakturaTypeIndividual);

    t6.Actual = string.Join(", ", fakturaActualConstructors);
    t6.IsCorrect = fakturaExpectedConstructors.SequenceEqual(fakturaActualConstructors);
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

IEnumerable<(string, string)> GetProperties(Type type)
{
    var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(p => (p.Name, p.PropertyType.Name));
    return properties;
}

IEnumerable<string> GetConstructorsTypes(Type type)
{
    var types = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(c => string.Join(", ", c.GetParameters().Select(p => p.ParameterType.Name)));

    return types;
}

string TestRun(Type fakturaType, Type zakaznikType, object[] parameters, IEnumerable<(string, string)> expectedZakaznikProperties)
{
    StringBuilder sb = new();

    try
    {
        object? expectedObject = Activator.CreateInstance(fakturaType, parameters);

        if(expectedObject is null)
        {
            return $"Can not create instance of {fakturaType.Name}";
        }

        foreach(var (name, typeName) in expectedZakaznikProperties)
        {
            PropertyInfo? property = fakturaType.GetProperty(name);

            if(property is null)
            {
                return $"Missing property {name}";
            }

            var propertyType = property.PropertyType;

            if(propertyType == zakaznikType)
            {
                var properties = propertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                foreach(var prop in properties)
                {
                    object? value = prop.GetValue(property.GetValue(expectedObject));

                    if(value is null)
                    {
                        return $"Property {name}.{prop.Name} is null";
                    }

                    sb.Append($"{name}.{prop.Name}: {value} ");
                }   
            }
            else
            {
                object? value = property.GetValue(expectedObject);

                if(value is null)
                {
                    return $"Property {name} is null";
                }

                sb.Append($"{name}: {value} ");
            }
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
    class Zakaznik(string jmeno, string email)
    {
        public string Jmeno {get; } = jmeno;
        public string Email {get; } = email;
    }

    class Faktura(int pocetPolozek, decimal cenaCelkem, string jmeno, string email)
    {
        public int PocetPolozek {get; } = pocetPolozek;
        public decimal CenaCelkem {get; } = cenaCelkem;
        public Zakaznik Zakaznik {get; } = new(jmeno, email);
    }
}
