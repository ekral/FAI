using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Evaluator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string code = File.ReadAllText("Submission.cs");
            
            Evaluator evaluator = new(code);

            TestResult tr1 = evaluator.TestIfClassExists(1, nameof(Reference.EmailSender));
            TestResult tr2 = evaluator.TestIfClassExists(1, nameof(Reference.SmsSender));
            TestResult tr3 = evaluator.TestIfInterfaceExists(1, nameof(Reference.INotificationSender));
            TestResult tr4 = evaluator.TestIfMethodExists(1, nameof(Reference.EmailSender), nameof(Reference.EmailSender.Send), Accessibility.Public, "Void", ["String", "String"]);
            TestResult tr5 = evaluator.TestIfMethodExists(1, nameof(Reference.SmsSender), nameof(Reference.EmailSender.Send), Accessibility.Public, "Void", ["String", "String"]);
            TestResult tr6 = evaluator.TestIfMethodExists(1, nameof(Reference.INotificationSender), nameof(Reference.EmailSender.Send), Accessibility.Public, "Void", ["String", "String"]);
            TestResult tr7 = evaluator.TestIfClassImplementsInterface(1, nameof(Reference.INotificationSender), nameof(Reference.EmailSender));
            TestResult tr8 = evaluator.TestIfClassImplementsInterface(1, nameof(Reference.INotificationSender), nameof(Reference.SmsSender));
            TestResult tr9 = evaluator.TestIfConstructorExists(1, nameof(Reference.Application), Accessibility.Public, [nameof(Reference.INotificationSender)]);

            var stdout = Console.Out;
            Console.SetOut(new StringWriter());
            
            if(evaluator.Assembly is Assembly assembly)
            {
                // TODO vytvorit metodu pro spusteni s nazvy typu v parametrech vracejici string jako vysledek, bez sloziteho overovani, jen vysledek
                // Type? type = assembly.GetType(classSymbol.ToDisplayString());
                // object? instance = Activator.CreateInstance(type, 42, "text"); // 42, "text" jsou parametry konstruktoru
                // MethodInfo? method = type.GetMethod("MojeMetoda"); // type muze byt interface
                // object? returnValue = method?.Invoke(instance, [arg1, arg2]);
            }

            Console.SetOut(stdout);

            TestResult[][] testResults = [[tr1], [tr2], [tr3], [tr4], [tr5], [tr6], [tr7], [tr8], [tr9]];

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

#if DEBUG
            foreach (Diagnostic diagnostic in evaluator.Compilation.GetDiagnostics())
            {
                Console.WriteLine(diagnostic.ToString());
            }

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

    }
}


