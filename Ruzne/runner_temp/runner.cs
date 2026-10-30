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

TestCase tc1 = ExistenceTestCase(1, [], "Zviratko definováno", nameof(Reference.Zviratko));
TestCase tc2 = ConstructorTestCase(1, [], "Zviratko má správný konstruktor", nameof(Reference.Zviratko), ["String"]);
TestCase tc3 = NormalMethodTestCase(1, [], "Zviratko má správnou metodu", nameof(Reference.Zviratko), nameof(Reference.Zviratko.Zvuk), [], "String");
TestCase tc4 = ExistenceTestCase(1, [], "Pejsek definován", nameof(Reference.Pejsek));
TestCase tc5 = ConstructorTestCase(1, [], "Pejsek má správný konstruktor", nameof(Reference.Pejsek), ["String"]);
TestCase tc6 = NormalMethodTestCase(1, [], "Pejsek má správnou metodu", nameof(Reference.Pejsek), nameof(Reference.Pejsek.Zvuk), [], "String");
TestCase tc7 = InheritanceTestCase(1, [], "Správná dědičnost Pejsek", nameof(Reference.Zviratko), nameof(Reference.Pejsek));
int x = Random.Shared.Next(1, 10);
int y = Random.Shared.Next(1, 10);
TestCase tc8 = RunTestCase(1, [tc1.Result], nameof(Reference.Zviratko), nameof(Reference.Zviratko), ["Fik"]);
TestCase tc9 = RunTestCase(1, [tc1.Result, tc4.Result], nameof(Reference.Zviratko), nameof(Reference.Pejsek), ["Rex"]);
TestCase tc10 = RunTestCase(1, [tc4.Result], nameof(Reference.Pejsek), nameof(Reference.Pejsek), ["Max"]);

TestCase[][] testCases = [[tc1], [tc2], [tc3], [tc4], [tc5], [tc6], [tc7], [tc8, tc9, tc10]];

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


TestCase ConstructorTestCase(int points, TestResult[] dependencies, string description, string typeName, string[] expectedTypes)
{
    return new TestCase(points, dependencies, dep =>
    {
        if(Type.GetType(typeName) is not Type type)
        {
            return new TestResult(description, string.Join(", ", expectedTypes), $"Typ {typeName} neexistuje", false, 0);
        }

        var types = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(c => string.Join(", ", c.GetParameters().Select(p => p.ParameterType.Name)));

        if(!types.SequenceEqual(expectedTypes))
        {
            return new TestResult(description, string.Join(", ", expectedTypes), string.Join(", ", types), false, 0);
        }

        return new TestResult(description, true.ToString(), true.ToString(), true, points);
        
    });
}

TestCase NormalMethodTestCase(int points, TestResult[] dependencies, string description, string typeName, string methodName, string[] expectedParameters, string expectedReturnType)
{
    return new TestCase(points, dependencies, dep =>
    {
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

        if(method.IsVirtual)
        {
            return new TestResult(description, "správný typ metody", "typ metody neni správný", false, 0);
        }

        return new TestResult(description, true.ToString(), true.ToString(), true, points);

    });
}

TestCase VirtualMethodTestCase(int points, TestResult[] dependencies, string description, string typeName, string methodName, string[] expectedParameters, string expectedReturnType)
{
    return new TestCase(points, dependencies, dep =>
    {
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

        if(!method.IsVirtual)
        {
            return new TestResult(description, "správný typ metody", "typ metody neni správný", false, 0);
        }

        return new TestResult(description, true.ToString(), true.ToString(), true, points);

    });
}

TestCase OverridedMethodTestCase(int points, TestResult[] dependencies, string description, string typeName, string methodName, string[] expectedParameters, string expectedReturnType)
{
    return new TestCase(points, dependencies, dep =>
    {
        if(Type.GetType(typeName) is not Type type)
        {
            return new TestResult(description, true.ToString(), $"Typ {typeName} neexistuje", false, 0);
        }
     
        if(type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) is not MethodInfo method)
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

        if(method.DeclaringType != type || method.GetBaseDefinition() == method || !method.IsVirtual)
        {
            return new TestResult(description, "správný typ metody", "typ metody neni správný", false, 0);
        }

        return new TestResult(description, true.ToString(), true.ToString(), true, points);
        
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

TestCase RunTestCase(int points, TestResult[] dependencies, string targetTypeName, string typeName, object[] constructorArgs)
{
    TestCase testCase = new(points, dependencies, deps => 
    {
        string description = $"objekt: {typeName} metoda: {targetTypeName}.Zvuk()";

        if(deps.Any(d => !d.IsCorrect))
        {
            return new TestResult(description, $"Podmínky: {string.Join(", ", deps.Select(d => d.Description))}", $"Nesplněné: {string.Join(", ", deps.Where(d => !d.IsCorrect).Select(d => d.Description))}", false, 0);
        }
    
        string expected = Run($"Reference.{targetTypeName}", $"Reference.{typeName}", constructorArgs);
        string actual = Run(targetTypeName, typeName, constructorArgs);

        if(expected != actual)
        {
            return new TestResult(description, expected, actual, false, 0);
        }

        return new TestResult(description, expected, actual, true, points);
    });

    return testCase;
}

string Run(string targetTypeName, string typeName, object[] constructorArgs)
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

        if(targetType.GetMethod("Zvuk") is not MethodInfo method)
        {
            return $"Nemůžu najít metodu Zvuk v {targetTypeName}";
        }

        object? result2 = method.Invoke(instance, []);
        if(method.Invoke(instance, []) is not string result)
        {
            return $"Metoda Zvuk v {typeName} nevrátila hodnotu typu string";
        }

        return result;
    }
    catch(Exception ex)
    {
        return $"Exception: {ex.Message}";
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
    class Zviratko(string jmeno)
    {
        public string Jmeno { get; set; } = jmeno;
        public string Zvuk()
        {
            return Jmeno;
        }
    }

    class Pejsek(string jmeno) : Zviratko(jmeno)
    {
        public string Zvuk()
        {
            return $"{Jmeno} dela haf haf!";        
        }
    }
}
