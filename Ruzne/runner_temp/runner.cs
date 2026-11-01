#define ASSIGNMENT_REPORT
#nullable enable

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

var stdout = Console.Out;
var dummyWritter = new StringWriter();
Console.SetOut(dummyWritter);

TestCase tc1 = ExistenceTestCase(1, [], nameof(Reference.TridaA));
TestCase tc2 = ConstructorTestCase(1, [], nameof(Reference.TridaA), [typeof(int).Name]);
TestCase tc3 = PropertyTestCase(1, [], nameof(Reference.TridaA), nameof(Reference.TridaA.X), typeof(int).Name, (p, _) => p.GetMethod?.IsPublic ?? false);
TestCase tc4 = ExistenceTestCase(1, [], nameof(Reference.TridaB));
TestCase tc5 = ConstructorTestCase(1, [], nameof(Reference.TridaB), [typeof(int).Name, typeof(int).Name]);
TestCase tc6 = PropertyTestCase(1, [], nameof(Reference.TridaB), nameof(Reference.TridaB.A), nameof(Reference.TridaA), (p, _) => p.GetMethod?.IsPublic ?? false);

string testFunction(object o, Type t) => GetPropertiesValues(o, t);

TestCase tc7 = RunTestCase(1, [], nameof(Reference.TridaA), nameof(Reference.TridaA), [1], [], testFunction);
TestCase tc8 = RunTestCase(1, [], nameof(Reference.TridaB), nameof(Reference.TridaB), [2, 3], [], testFunction);

TestCase[][] testCases = [[tc1], [tc2], [tc3], [tc4], [tc5], [tc6], [tc7, tc8]];

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
                true => new JsonArray(tc.Result.Description, tc.Result.Expected, $"{tc.Result.Actual} správně, ale chybí související testy: [{string.Join(", ", missing.Select(m => m.Result.Description))}]", false),
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

Console.WriteLine($"<p>Výsledné body se spočítají jako: (počet bodů za splněné kritéria / {maxPoints}) x celkový počet bodů za úlohu.</p>");


Console.WriteLine($"<ol>");

foreach(var group in testCases)
{
    string indentation = string.Empty;

    bool isGroup = group.Count() > 1;

    if(isGroup)
    {
        int groupMaxPoints = group.Sum(g => g.MaxPoints);
        Console.WriteLine($"<li>Skupina úkolů, které musí být splněny současně (body: {groupMaxPoints}):");
        Console.WriteLine($"<ul>");
    }

    foreach(var testCase in group)
    {
        Console.Write($"<li>{testCase.Result.Description} (body: {testCase.MaxPoints}).");

        if(testCase.Dependencies.Length > 0)
        {
            Console.Write(" (závislosti: ");
            Console.Write(string.Join(", ", testCase.Dependencies.Select(d => d.Description)));
            Console.Write(")");
        }

        Console.WriteLine($"</li>");
    }
    
    if(isGroup)
    {
        Console.WriteLine($"</ul>");
        Console.WriteLine($"</li>");
    }   
}

Console.WriteLine($"</ol>");

#endif

TestCase ExistenceTestCase(int points, TestResult[] dependencies, string typeName)
{
    string description = $"Typ {typeName} definován";

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


TestCase ConstructorTestCase(int points, TestResult[] dependencies, string typeName, string[] expectedTypes)
{
    string description = $"Typ {typeName} má správný konstruktor";

    return new TestCase(points, dependencies, dep =>
    {
        if(Type.GetType(typeName) is not Type type)
        {
            return new TestResult(description, string.Join(", ", expectedTypes), $"Typ {typeName} neexistuje", false, 0);
        }

        var constructor = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).FirstOrDefault();

        if(constructor is null)
        {
            return new TestResult(description, string.Join(", ", expectedTypes), $"Typ {typeName} nemá žádný konstruktor", false, 0);
        }

        var types = constructor.GetParameters().Select(p => p.ParameterType.Name);

        if(!types.SequenceEqual(expectedTypes))
        {
            return new TestResult(description, string.Join(", ", expectedTypes), string.Join(", ", types), false, 0);
        }

        return new TestResult(description, string.Join(", ", expectedTypes), string.Join(", ", types), true, points);
        
    });
}

// overrided: (m, t) => m.DeclaringType == t && m.GetBaseDefinition() != m && m.IsVirtual

TestCase MethodTestCase(int points, TestResult[] dependencies, string typeName, string methodName, string[] expectedParameters, string expectedReturnType, Func<MethodInfo, Type, bool> additionalCheck)
{
    return new TestCase(points, dependencies, dep =>
    {
        string description = $"Typ {typeName} má správnou metodu {methodName}";

        if(Type.GetType(typeName) is not Type type)
        {
            return new TestResult(description, true.ToString(), $"Typ {typeName} neexistuje", false, 0);
        }
     
        if(type.GetMethod(methodName) is not MethodInfo method)
        {
            return new TestResult(description, true.ToString(), $"Metoda {methodName} neexistuje", false, 0);
        }
        
        var actualParameters = method.GetParameters().Select(p => p.ParameterType.Name);

        if(!actualParameters.SequenceEqual(expectedParameters))
        {
            return new TestResult(description, $"parametery: {string.Join(", ", expectedParameters)}", $"parametry: {string.Join(", ", actualParameters)}", false, 0);
        }

        string actualReturnType = method.ReturnType.Name;

        if(actualReturnType != expectedReturnType)
        {
            return new TestResult(description, $"return type: {expectedReturnType}", $"return type: {actualReturnType}", false, 0);
        }

        if(!additionalCheck(method, type))
        {
            return new TestResult(description, "správný typ metody", "typ metody neni správný", false, 0);
        }

        return new TestResult(description, true.ToString(), true.ToString(), true, points);

    });
}

TestCase PropertyTestCase(int points, TestResult[] dependencies, string typeName, string propertyName, string expectedTypeName, Func<PropertyInfo, Type, bool> additionalCheck)
{
    return new TestCase(points, dependencies, dep =>
    {
        string description = $"Typ {typeName} má správnou property {propertyName}";

        if(Type.GetType(typeName) is not Type type)
        {
            return new TestResult(description, true.ToString(), $"Typ {typeName} neexistuje", false, 0);
        }
     
        if(type.GetProperty(propertyName) is not PropertyInfo property)
        {
            return new TestResult(description, true.ToString(), $"Property {propertyName} neexistuje", false, 0);
        }
        

        string actualTypeName = property.PropertyType.Name;

        if(expectedTypeName != actualTypeName)
        {
            return new TestResult(description, $"type: {expectedTypeName}", $"type: {actualTypeName}", false, 0);
        }

        if(!additionalCheck(property, type))
        {
            return new TestResult(description, "správný typ property", "typ property není správný", false, 0);
        }

        return new TestResult(description, true.ToString(), true.ToString(), true, points);

    });
}


TestCase InheritanceTestCase(int points, TestResult[] dependencies, string parentTypeName, string childTypeName)
{
    return new TestCase(points, dependencies, dep =>
    {
        string description = $"Typ {childTypeName} dědí z {parentTypeName}";

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

TestCase RunTestCase(int points, TestResult[] dependencies, string targetTypeName, string typeName, object[] constructorArgs, (string, object)[] propertyParameters, Func<object, Type, string> testFunction)
{
    TestCase testCase = new(points, dependencies, deps => 
    {
        string description = $"reference: {targetTypeName} objekt: {targetTypeName}, args: [{string.Join(", ", constructorArgs)}], properties: [{string.Join(", ", propertyParameters.Select(p => $"{p.Item1}={p.Item2}"))}]";

        if(deps.Any(d => !d.IsCorrect))
        {
            return new TestResult(description, $"Podmínky: {string.Join(", ", deps.Select(d => d.Description))}", $"Nesplněné: {string.Join(", ", deps.Where(d => !d.IsCorrect).Select(d => d.Description))}", false, 0);
        }
    
        string expected = Run($"Reference.{targetTypeName}", $"Reference.{typeName}", constructorArgs, propertyParameters, testFunction);
        string actual = Run(targetTypeName, typeName, constructorArgs, propertyParameters, testFunction);

        if(expected != actual)
        {
            return new TestResult(description, expected, actual, false, 0);
        }

        return new TestResult(description, expected, actual, true, points);
    });

    return testCase;
}

string Run(string targetTypeName, string typeName, object[] constructorArgs, (string, object)[] propertyParameters, Func<object, Type, string> testFunction)
{
    try
    {
        if(Type.GetType(typeName) is not Type type)
        {
            return $"Nemůžu najít typ {typeName}";
        }

        if(Type.GetType(targetTypeName) is not Type targetType)
        {
            return $"Nemůžu najít typ {targetTypeName}";
        }

        if(Activator.CreateInstance(type, constructorArgs) is not object instance)
        {
            return $"Nemůžu vytvořit instanci {typeName}";
        }

        if(!targetType.IsInstanceOfType(instance))
        {
            return $"Nemůžu převést instanci {typeName} na {targetTypeName}";
        }

        foreach(var (propertyName, value) in propertyParameters)
        {
            if(type.GetProperty(propertyName) is not PropertyInfo property)
            {
                return $"Nemůžu najít property {propertyName} v {typeName}";
            }

           property.SetValue(instance, value);
        }

        return testFunction(instance, targetType);

    }
    catch(Exception ex)
    {
        return $"Exception: {ex.Message}";
    }
}


string RunMethod(object instance, Type type, string methodName, object[] parameters )
{
    if(type.GetMethod(methodName) is not MethodInfo method)
    {
        return $"Nemůžu najít metodu {methodName} v {type.Name}";
    }

    object? result = method.Invoke(instance, parameters);

    if(result is null)
    {
        return "Void";
    }

    return result.ToString();
}

string GetPropertiesValues(object instance, Type type)
{
    var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

    string GetPropertyValue(PropertyInfo property)
    {
        object? value = property.GetValue(instance);

        if(value is null)
        {
            return "null";
        }

        Type valueType = value.GetType();

        if (valueType.IsClass && valueType != typeof(string))
        {
           return $"{{ {GetPropertiesValues(value, valueType)} }}"; 
        }

        return value.ToString();
    }

    var values = properties.OrderBy(p => p.Name).Select(p => $"{p.Name} ({p.DeclaringType?.Name ?? ""}): {GetPropertyValue(p)}");

    return string.Join(", ", values);
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

public static class TestConsole
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

namespace Reference
{
    class TridaA(int x)
    {
        public int X {get; set;} = x;
    }

    class TridaB(int x, int y)
    {
        public TridaA A {get; set;} = new TridaA(x);
    }
}
