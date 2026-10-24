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

var controlExpectedProperties = GetProperties(typeof(Reference.Control));
var buttonExpectedProperties = GetProperties(typeof(Reference.Button));
var dropDownExpectedProperties = GetProperties(typeof(Reference.DropDown));

TestCase t1 = ExistenceTestCase(1, [], "Control definován", "Control");
TestCase t2 = PropertiesTestCase(1,[], "Control properties", "Control", controlExpectedProperties);
TestCase t3 = ExistenceTestCase(1, [], "Button definován", "Button");
TestCase t4 = PropertiesTestCase(1, [], "Button properties", "Button", buttonExpectedProperties);
TestCase t5 = ExistenceTestCase(1, [], "DropDown definován", "DropDown");
TestCase t6 = PropertiesTestCase(1, [], "DropDown properties", "DropDown", dropDownExpectedProperties);
TestCase t7 = InheritanceTestCase(1, [], "Button dědí od Control", "Control", "Button");
TestCase t8 = InheritanceTestCase(1, [], "DropDown dědí od Control", "Control", "DropDown");
Reference.Button button = new() { Id = 1, X = 10, Y = 20, Text = "Click me!", Enabled = true};
TestCase t9 = ComparePropertiesTestCase(1, [t7.Result], """new Button() { Id = 1, X = 10, Y = 20, Text = "Click me!", Enabled = true}""", "Button", [], button);
Reference.DropDown dropDown = new() { Id = 2, X = 30, Y = 40, Items = ["Option 1", "Option 2", "Option 3"], SelectedIndex = 0};
TestCase t10 = ComparePropertiesTestCase(1, [t8.Result], """new DropDown() { Id = 2, X = 30, Y = 40, Items = ["Option 1", "Option 2", "Option 3"], SelectedIndex = 0}""", "DropDown", [], dropDown);

TestCase[][] testCases = [[t1],[t2], [t3], [t4], [t5], [t6], [t7], [t8], [t9], [t10]];

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
            true => new JsonArray(tc.Result.Description, tc.Result.Expected, tc.Result.Actual, tc.Result.IsCorrect),
            false => new JsonArray(tc.Result.Description, tc.Result.Expected, $"{tc.Result.Actual} správně, ale chybí související testy: [{string.Join(", ", missing.Select(m => m.Result.Description))}]", groupAllCorrect)
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

IEnumerable<string> GetProperties(Type type)
{
    var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(p => p.Name);
    return properties;
}

IEnumerable<string> GetConstructorsTypes(Type type)
{
    var types = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(c => string.Join(", ", c.GetParameters().Select(p => p.ParameterType.Name)));

    return types;
}

TestCase ExistenceTestCase(int points, TestResult[] dependencies, string description, string typeName)
{
    return new TestCase(points, dependencies, dep =>
    {
        if(Type.GetType(typeName) is null)
        {
            return new TestResult(description, true.ToString(), false.ToString(), false, 0);
        }
        else
        {
            return new TestResult(description, true.ToString(), true.ToString(), true, points);
        }
    });
}

TestCase PropertiesTestCase(int points, TestResult[] dependencies, string description, string typeName, IEnumerable<string> expectedProperties)
{
    return new TestCase(points, dependencies, dep =>
    {
        if(Type.GetType(typeName) is Type type)
        {
            var actualProperties = GetProperties(type);

            bool isCorrect = new HashSet<string>(expectedProperties).SetEquals(actualProperties);

            return new TestResult(description, string.Join(", ", expectedProperties), string.Join(", ", actualProperties), isCorrect, isCorrect ? points : 0);
        }
        else
        {
            return new TestResult(description, string.Join(", ", expectedProperties), "can not evaluate", false, 0);
        }
    });
}

TestCase InheritanceTestCase(int points, TestResult[] dependencies, string description, string parentTypeName, string childTypeName)
{
    return new TestCase(points, dependencies, dep =>
    {
        if(Type.GetType(parentTypeName) is Type parentType && Type.GetType(childTypeName) is Type childType)
        {
            bool isCorrect = childType.BaseType == parentType;

            return new TestResult(description, true.ToString(), isCorrect.ToString(), isCorrect, isCorrect ? points : 0);
        }
        else
        {
            return new TestResult(description, true.ToString(), false.ToString(), false, 0);
        }
    });
}

// zkusim zapsat hodnoty do properties z reference instance jestli jsou pouzite spravne typy
TestCase ComparePropertiesTestCase(int points, TestResult[] dependencies, string description, string typeName, object[] constructorParameters, object referenceInstance)
{
    return new TestCase(points, dependencies, dep =>
    {
        if(dep.Any(d => !d.IsCorrect))
        {
            return new TestResult(description, $"Podminky: {string.Join(", ", dep.Select(d => d.Description))}", $"Nesplnene: {string.Join(", ", dep.Where(d => !d.IsCorrect).Select(d => d.Description))}", false, 0);
        }

        Type referenceType = referenceInstance.GetType();
        var expectedProperties = referenceType.GetProperties(BindingFlags.Public | BindingFlags.Instance ).Select(p => p.Name);

        if(Type.GetType(typeName) is Type type)
        {
            try
            {
                object? instance = Activator.CreateInstance(type, constructorParameters);

                if(instance is null)
                {
                    return new TestResult(description, $"Muzu vytvorit instanci {type.Name}", $"Neslo vytvorit instanci {type.Name}", false, 0);
                }

                foreach(var propertyName in expectedProperties)
                {
                    PropertyInfo? property = type.GetProperty(propertyName);

                    if(property is null)
                    {
                        return new TestResult(description, $"Property {propertyName} existuje", "Property {propertyName} not found", false, 0);
                    }        

                    object? expectedValue = referenceType.GetProperty(propertyName)?.GetValue(referenceInstance);

                    property.SetValue(instance, expectedValue);

                    object? value = property.GetValue(instance);

                    if(value is null)
                    {
                        return new TestResult(description, $"Hodnota {propertyName} neni null", $"Hodnota {propertyName} je null", false, 0);
                    }

                    if(!value!.Equals(expectedValue))
                    {
                        return new TestResult(description, $"{propertyName}: {expectedValue}", $"{propertyName}: {value}", false, 0);
                    }
                }
            }
            catch(Exception ex)
            {
                return new TestResult(description, "Property jsou nastavit na hodnoty", $"Exception: {ex.Message}", false, 0);
            }
        }
        else
        {
            return new TestResult(description, $"Existuje typ {typeName}", $"Existuje typ {typeName}", false, 0);
        }

        return new TestResult(description, "Jde nastavit hodnoty", "Jde nastavit hodnoty", true, points);
        
    });
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

namespace Reference
{
   class Control()
        {
        public int Id {get; set;}
        public int X {get; set;}
        public int Y {get; set;}
    }

    class Button : Control
    {
        public string Text {get; set;} = string.Empty;
        public bool Enabled {get; set;}
    }

    class DropDown : Control
    {
        public string[] Items {get; set;} = [];
        public int SelectedIndex {get; set;}
    }
}
