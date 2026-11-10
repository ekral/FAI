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

            TestResult tr1 = TestIfClassExists(1, nameof(Reference.EmailSender), root);

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

            TestResult[][] testResults = [[tr1]];

            var header = new JsonArray("Test", "Očekávaná hodnota", "Výsledek", "IsCorrect");

            JsonArray array = [header];

            int points = 0;
            int maxPoints = 0;

            foreach (TestResult[] group in testResults)
            {
                var missing = group.Where(tr => !tr.IsCorrect);
                bool groupAllCorrect = !missing.Any();

                int groupMaxPoints = 0;

                foreach (TestResult testResult in group)
                {
                    groupMaxPoints += testResult.MaxPoints;

                    var jsonArray = groupAllCorrect switch
                    {
                        true => new JsonArray(testResult.Description, testResult.Expected, testResult.Actual, true),
                        false => testResult.IsCorrect switch
                        {
                            true => new JsonArray(testResult.Description, testResult.Expected, $"{testResult.Actual} správně, ale chybí související testy: [{string.Join(", ", missing.Select(m => m.Description))}]", false),
                            false => new JsonArray(testResult.Description, testResult.Expected, testResult.Actual, false)
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

            foreach (TestResult[] group in testResults)
            {
                string indentation = string.Empty;

                bool isGroup = group.Count() > 1;

                if (isGroup)
                {
                    int groupMaxPoints = group.Sum(g => g.MaxPoints);
                    Console.WriteLine($"<li>Skupina úkolů, které musí být splněny současně (body: {groupMaxPoints}):");
                    Console.WriteLine($"<ul>");
                }

                foreach (TestResult testResult in group)
                {
                    Console.WriteLine($"<li>{testResult.Description} (body: {testResult.MaxPoints}).</li>");
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

        private static TestResult TestIfClassExists(int points, string className, SyntaxNode root)
        {
            string description = $"Třída {className}";

            if (TestClass(root, className, out string expected, out string actual) is not SyntaxNode node)
            {
                return new TestResult(description, expected, actual, false, points, 0);
            }

            return new TestResult(description, expected, actual, true, points, points);
        }

        private static SyntaxNode? TestClass(SyntaxNode root, string className, out string expected, out string actual)
        {
            expected = $"Třída {className} je definovaná";

            SyntaxNode? node = root
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(c => c.Identifier.ValueText == className);

            if(node is not null)
            {
                actual = expected;
            }
            else
            {
                actual = $"Třída {className} není definovaná";
            }

            return node;
        }
    }

    record TestResult(string Description, string Expected, string Actual, bool IsCorrect, int MaxPoints, int Points);

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
