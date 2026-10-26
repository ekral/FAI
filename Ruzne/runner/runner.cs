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

TestCase tc1 = ExistenceTestCase(1, [], "Zviratko definováno", "Zviratko");
TestCase tc2 = ExistenceTestCase(1, [], "Pejsek definován", "Pejsek");
TestCase tc3 = InheritanceTestCase(1, [], "Správná dědičnost", "Zviratko", "Pejsek");
TestCase tc4 = VirtualMethodTestCase(1, [], "Zviratko má správnou metodu", "Zviratko", "Zvuk", [], "Void");
TestCase tc5 = OverridedMethodTestCase(1, [], "Pejsek má správnou metodu", "Pejsek", "Zvuk", [], "Void");
TestCase tc6 = RunTestCase("Fik", "Rex", "Max", [tc1.Result, tc2.Result, tc3.Result]);
TestCase tc7 = RunTestCase("Azor", "Alik", "Kira", [tc1.Result, tc2.Result, tc3.Result]);

TestCase[][] testCases = [[tc1], [tc2], [tc3], [tc4], [tc5], [tc6, tc7]];

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

TestCase VirtualMethodTestCase(int points, TestResult[] dependencies, string description, string typeName, string methodName, string[] expectedParameters, string expectedReturnType)
{
    return new TestCase(points, dependencies, dep =>
    {
        if(Type.GetType(typeName) is not Type type)
        {
            return new TestResult(description, true.ToString(), $"Type {typeName} does not exist", false, 0);
        }
     
        if(type.GetMethod(methodName) is not MethodInfo method)
        {
            return new TestResult(description, true.ToString(), $"Method {methodName} does not exist", false, 0);
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
            return new TestResult(description, true.ToString(), $"Type {typeName} does not exist", false, 0);
        }
     
        if(type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) is not MethodInfo method)
        {
            return new TestResult(description, true.ToString(), $"Method {methodName} does not exist", false, 0);
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
    return new TestCase(points, dependencies, deps =>
    {
        if(deps.Any(d => !d.IsCorrect))
        {
            return new TestResult(description, $"Podminky: {string.Join(", ", deps.Select(d => d.Description))}", $"Nesplnene: {string.Join(", ", deps.Where(d => !d.IsCorrect).Select(d => d.Description))}", false, 0);
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

TestCase RunTestCase(string name1, string name2, string name3, TestResult[] dependencies)
{
    TestCase testCase = new(1, dependencies, deps => 
    {
        string description = $"Test {name1}, {name2} a {name3}";

        if(deps.Any(d => !d.IsCorrect))
        {
            return new TestResult(description, $"Podminky: {string.Join(", ", deps.Select(d => d.Description))}", $"Nesplnene: {string.Join(", ", deps.Where(d => !d.IsCorrect).Select(d => d.Description))}", false, 0);
        }
    
        string expected1 = Run("Reference.Zviratko", "Reference.Zviratko", name1);
        string actual1 = Run("Zviratko", "Zviratko", name1);

        if(expected1 != actual1)
        {
            return new TestResult(description, expected1, actual1, false, 0);
        }       

        string expected2 = Run("Reference.Zviratko", "Reference.Pejsek", name2);
        string actual2 = Run("Pejsek", "Zviratko", name2);

        if(expected2 != actual2)
        {
            return new TestResult(description, expected2, actual2, false, 0);
        }

        string expected3 = Run("Reference.Pejsek", "Reference.Pejsek", name3);
        string actual3 = Run("Pejsek", "Pejsek", name3);

        if(expected3 != actual3)
        {
            return new TestResult(description, expected3, actual3, false, 0);
        }

        TestConsole.Clear();

        return new TestResult(description, "OK", "OK", true, 1);
    });

    return testCase;
}

string Run(string targetTypeName, string typeName, string petName)
{
    TestConsole.Clear();

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

        if(Activator.CreateInstance(type, [petName]) is not object instance)
        {
            return $"Nemůžu vytvořit instanci {typeName}";
        }

        if(instance.GetType().GetMethod("Zvuk") is not MethodInfo method)
        {
            return $"Nemůžu najít metodu Zvuk v {typeName}";
        }


        if(Convert.ChangeType(instance, targetType) is not object convertedInstance)
        {
            return $"Nemůžu převést instanci {typeName} na {targetTypeName}";
        }

        method.Invoke(convertedInstance, null);
    }
    catch(Exception ex)
    {
        return $"Exception: {ex.Message}";
    }

    return TestConsole.Message;
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
    class Zviratko(string jmeno)
    {
        public string Jmeno { get; set; } = jmeno;
        public virtual void Zvuk()
        {
            TestConsole.WriteLine(Jmeno);
        }
    }

    class Pejsek(string jmeno) : Zviratko(jmeno)
    {
        public override void Zvuk()
        {
            TestConsole.WriteLine($"{Jmeno} dela haf haf!");
        }
    }
}
