#define ASSIGNMENT_REPORT
#pragma warning disable CS8321
#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

var stdout = Console.Out;
var dummyWritter = new StringWriter();
Console.SetOut(dummyWritter);

string ResultFunction(object instance, Type type, object[] args)
{
    string methodName = "Run";

    try
    {
        if(type.GetMethod(methodName) is not MethodInfo method)
        {
            return $"Nemůžu najít metodu {methodName} v {type.Name}";
        }

        if(method.Invoke(instance, args) is not List<int> result)
        {
            return $"Metoda {methodName} v {type.Name} nevrátila správný typ List<int>";
        }

        return $"[{string.Join(", ", result)}]";
    }
    catch (Exception ex)
    {
        return $"Exception: {ex.Message}";
    }
}

TestCase tc1 = RunTestCase(1, "Test 1" , [], nameof(Reference.Solution), nameof(Reference.Solution), [], [], [(int[])[]], ResultFunction);
TestCase tc2 = RunTestCase(1, "Test 2" , [], nameof(Reference.Solution), nameof(Reference.Solution), [], [], [(int[])[ 0 ]], ResultFunction);
TestCase tc3 = RunTestCase(1, "Test 3" , [], nameof(Reference.Solution), nameof(Reference.Solution), [], [], [(int[])[ 1 ]], ResultFunction);
TestCase tc4 = RunTestCase(1, "Test 3" , [], nameof(Reference.Solution), nameof(Reference.Solution), [], [], [(int[])[ -1 ]], ResultFunction);
TestCase tc5 = RunTestCase(1, "Test 4" , [], nameof(Reference.Solution), nameof(Reference.Solution), [], [], [(int[])[ -1,-2,-3,4,5,6,7,8,9,10 ]], ResultFunction);

TestCase[][] testCases = [[tc1], [tc2], [tc3], [tc4], [tc5]];

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

TestCase FieldTestCase(int points, TestResult[] dependencies, string typeName, string fieldName, string expectedTypeName, Func<FieldInfo, Type, bool> additionalCheck)
{
    return new TestCase(points, dependencies, dep =>
    {
        string description = $"Typ {typeName} má správný field {fieldName}";

        if(Type.GetType(typeName) is not Type type)
        {
            return new TestResult(description, true.ToString(), $"Typ {typeName} neexistuje", false, 0);
        }
     
        if(type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly) is not FieldInfo field)
        {
            return new TestResult(description, true.ToString(), $"Field {fieldName} neexistuje", false, 0);
        }
        

        string actualTypeName = field.FieldType.Name;

        if(expectedTypeName != actualTypeName)
        {
            return new TestResult(description, $"type: {expectedTypeName}", $"type: {actualTypeName}", false, 0);
        }

        if(!additionalCheck(field, type))
        {
            return new TestResult(description, "správný typ fieldu", "typ fieldu není správný", false, 0);
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

TestCase RunTestCase(int points, string info, TestResult[] dependencies, string targetTypeName, string typeName, object[] constructorArgs, (string, object)[] propertyParameters, object[] args, Func<object, Type, object[], string> resultFunction)
{
    TestCase testCase = new(points, dependencies, deps => 
    {
        var constructorArgsDescription = constructorArgs.Select(a => a is System.Collections.IEnumerable result && a is not string ? string.Join(", ", result.Cast<object>()) : a?.ToString() ?? "null");
        var argsDescription = args.Select(a => a is System.Collections.IEnumerable result && a is not string ? string.Join(", ", result.Cast<object>()) : a?.ToString() ?? "null");


        string description = $"{info} args: ({string.Join(", ", argsDescription)}), reference: {targetTypeName}, object: {targetTypeName}, constructor args: ({string.Join(", ", constructorArgsDescription)}), properties: [{string.Join(", ", propertyParameters.Select(p => $"{p.Item1}: {p.Item2}"))}]";

        if(deps.Any(d => !d.IsCorrect))
        {
            return new TestResult(description, $"Podmínky: {string.Join(", ", deps.Select(d => d.Description))}", $"Nesplněné: {string.Join(", ", deps.Where(d => !d.IsCorrect).Select(d => d.Description))}", false, 0);
        }
    
        string expected = Run($"Reference.{targetTypeName}", $"Reference.{typeName}", constructorArgs, propertyParameters, args, resultFunction);
        string actual = Run(targetTypeName, typeName, constructorArgs, propertyParameters, args, resultFunction);

        if(expected != actual)
        {
            return new TestResult(description, expected, actual, false, 0);
        }

        return new TestResult(description, expected, actual, true, points);
    });

    return testCase;
}

string Run(string targetTypeName, string typeName, object[] constructorArgs, (string, object)[] propertyParameters, object[] args, Func<object, Type, object[], string> resultFunction)
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

        return resultFunction(instance, targetType, args);

    }
    catch(Exception ex)
    {
        return $"Exception: {ex.Message}";
    }
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

        return value?.ToString() ?? "null";
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
        message = message.Length > 256 ? message[..256] : message;

        message = System.Net.WebUtility.HtmlEncode(message);

        writer.Write(message);
    }
}

namespace Reference
{
    class Solution
    {
        public List<int> Run(int[] pole)
        {
            int delka = pole.Length;
        
            return [.. pole.Where(x => x > 0)];
        }
    }
}

/*
// Example of class to test IDisposable implementation and method invocation
class Trida : IDisposable
{
    public static bool ThrowConstructor {get; set;}
    public static bool ThrowWriteLine {get; set;}
    public static int DisposedCount {get; set;}
    public static bool MethodCalled {get; set;}

    public Trida()
    {
        if (ThrowConstructor)
        {
            throw new Exception();
        }
    }

    public void Metoda()
    {
        MethodCalled = true;

        if (ThrowWriteLine)
        {
            throw new Exception();
        }
    }

    public void Dispose()
    {
        ++DisposedCount;
    }
}

// Example of result function
class ResultFunction(bool throwConstructor, bool throwWriteLine)
{
    public string Function(object instance, Type type)
    {
        Trida.ThrowConstructor = throwConstructor;
        Trida.ThrowWriteLine = throwWriteLine;

        Trida.DisposedCount = 0;
        Trida.MethodCalled = false;

        bool exceptionThrown = false;

        try
        {
            if(type.GetMethod("Run") is not MethodInfo method)
            {
                return $"Nemůžu najít metodu Run v {type.Name}";
            }

            method.Invoke(instance, null);
        }
        catch (Exception)
        {
            exceptionThrown = true;
        }

        return $"Počet volání Dispose: {Trida.DisposedCount}, Metoda zavolána: {Trida.MethodCalled}, Vyvolaná výjimka: {exceptionThrown}";
    }
}
*/
