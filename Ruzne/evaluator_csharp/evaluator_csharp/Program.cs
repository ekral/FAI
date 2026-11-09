#define ASSIGNMENT_REPORT

using evaluator_csharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace evaluator_csharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string code = """
            public class EmailSender 
            {
                public static string MyMethod() 
                {
                    return "Hello Roslyn";
                }
            }
            """;

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            SyntaxNode root = syntaxTree.GetRoot();

            TestCase tc1 = TypeTestCase(1, [], nameof(Reference.EmailSender), root);

            var baseDir = AppContext.BaseDirectory;

            Assembly asm = typeof(Program).Assembly;
    
            PortableExecutableReference[] references = 
            [
                MetadataReference.CreateFromStream(asm.GetManifestResourceStream("refs/System.Collections.dll")!),
                MetadataReference.CreateFromStream(asm.GetManifestResourceStream("refs/System.Linq.dll")!),
                MetadataReference.CreateFromStream(asm.GetManifestResourceStream("refs/System.Runtime.dll")!),
            ];

            var compilation = CSharpCompilation.Create("DynamicAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddSyntaxTrees(syntaxTree)
                .AddReferences(references);

            
            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                Console.WriteLine(result.Diagnostics);
            }

            Console.WriteLine("Succes");

            var stdout = Console.Out;
            Console.SetOut(new StringWriter());
            
            // Run student code

            Console.SetOut(stdout);

            TestCase[][] testCases = [[tc1]];

            var header = new JsonArray("Test", "Očekávaná hodnota", "Výsledek", "IsCorrect");

            JsonArray array = [header];

            int points = 0;
            int maxPoints = 0;

            foreach (var group in testCases)
            {
                var missing = group.Where(tc => !tc.Result.IsCorrect);
                bool groupAllCorrect = !missing.Any();

                int groupMaxPoints = 0;

                foreach (TestCase tc in group)
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

            foreach (var group in testCases)
            {
                string indentation = string.Empty;

                bool isGroup = group.Count() > 1;

                if (isGroup)
                {
                    int groupMaxPoints = group.Sum(g => g.MaxPoints);
                    Console.WriteLine($"<li>Skupina úkolů, které musí být splněny současně (body: {groupMaxPoints}):");
                    Console.WriteLine($"<ul>");
                }

                foreach (var testCase in group)
                {
                    Console.Write($"<li>{testCase.Result.Description} (body: {testCase.MaxPoints}).");

                    if (testCase.Dependencies.Length > 0)
                    {
                        Console.Write(" (závislosti: ");
                        Console.Write(string.Join(", ", testCase.Dependencies.Select(d => d.Description)));
                        Console.Write(")");
                    }

                    Console.WriteLine($"</li>");
                }

                if (isGroup)
                {
                    Console.WriteLine($"</ul>");
                    Console.WriteLine($"</li>");
                }
            }

            Console.WriteLine($"</ol>");

#endif
        }

        private static TestCase TypeTestCase(int points, TestResult[] dependencies, string typeName, SyntaxNode root)
        {
            string description = $"Typ {typeName} správně definován";
            string expected = "Ano";

            return new TestCase(points, dependencies, dep =>
            {
                bool hasProgramClass = root
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Any(c => c.Identifier.ValueText == typeName);

                if (!hasProgramClass)
                {
                    return new TestResult(description, expected, $"Typ {typeName} neexistuje", false, 0);
                }

                return new TestResult(description, expected, expected, true, points);

            });
        }
    }

    
    record TestResult(string Description, string Expected, string Actual, bool IsCorrect, int Points);

    class TestCase(int maxPoints, TestResult[] dependencies, Func<TestResult[], TestResult> testFunction)
    {
        private TestResult? result = null;
        private readonly Func<TestResult[], TestResult> testFunction = testFunction;
        public int MaxPoints { get; } = maxPoints;
        public TestResult[] Dependencies { get; } = dependencies;

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
        private readonly static int maxLength = 128;
        private readonly static StringWriter writer = new();

        public static string Message
        {
            get
            {
                string message = writer.ToString();

                message = message.Length > maxLength ? message[..maxLength] : message;
                message = System.Net.WebUtility.HtmlEncode(message);

                return message;
            }
        }

        public static void Clear()
        {
            writer.GetStringBuilder().Clear();
        }

        public static void WriteLine(string message)
        {
            writer.Write(message);
        }
    }   
}

namespace Reference
{
    interface INotificationSender
    {
        void Send(string recipient, string message);
    }

    class EmailSender : INotificationSender
    {
        public void Send(string recipient, string message)
        {
            TestConsole.WriteLine($"Sent email to {recipient}: {message}");
        }
    }

    class SmsSender : INotificationSender
    {
        public void Send(string recipient, string message)
        {
            TestConsole.WriteLine($"Sent SMS to {recipient}: {message}");
        }
    }
}
