using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evaluator
{
    public class Evaluator(string code)
    {
        private readonly string code = code;
        private SyntaxTree? syntaxTree;
        private readonly Dictionary<string, CSharpCompilation> compilations = [];

        private PortableExecutableReference[] references =
        [
            MetadataReference.CreateFromStream(typeof(Program).Assembly.GetManifestResourceStream("refs/System.Collections.dll")!),
            MetadataReference.CreateFromStream(typeof(Program).Assembly.GetManifestResourceStream("refs/System.Linq.dll")!),
            MetadataReference.CreateFromStream(typeof(Program).Assembly.GetManifestResourceStream("refs/System.Runtime.dll")!),
        ];

        public SyntaxTree SyntaxTree
        {
            get
            {
                return syntaxTree ??= CSharpSyntaxTree.ParseText(code);
            }
        }

        private CSharpCompilation GetCompilation(string assemblyName, SyntaxTree syntaxTree)
        {
            if (compilations.TryGetValue(assemblyName, out CSharpCompilation? cachedCompilation))
            {
                return cachedCompilation;
            }

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName: assemblyName,
                syntaxTrees: [syntaxTree],
                references: references
            ).WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            compilations[assemblyName] = compilation;

            return compilation;
        }

        private T? TypeDeclaration<T>(string className, out string expected, out string actual)
               where T : TypeDeclarationSyntax
        {
            expected = $"Typ {className} je definován";

            T? cds = SyntaxTree.GetRoot()
                .DescendantNodes()
                .OfType<T>()
                .FirstOrDefault(c => c.Identifier.ValueText == className);

            if (cds is null)
            {
                actual = $"Typ {className} není definován";
                return null;
            }

            if (cds.OpenBraceToken.IsMissing)
            {
                expected = $"Typ {className} má otevírací závorku";
                actual = $"Typ {className} nemá otevírací závorka";
                return null;
            }

            if (cds.CloseBraceToken.IsMissing)
            {
                expected = $"Typ {className} má uzavírací závorku";
                actual = $"Typ {className} nemá uzavírací závorka";
                return null;
            }

            actual = expected;

            return cds;
        }

        private IMethodSymbol? MethodDeclaration(string methodName, TypeDeclarationSyntax typeDeclaration, Accessibility accessibility, string returnType, string[] parametersNames, out string expected, out string actual)
        {
            CSharpCompilation compilation = GetCompilation($"{typeDeclaration.Identifier.ValueText}.{methodName}", typeDeclaration.SyntaxTree);

            SemanticModel semanticModel = compilation.GetSemanticModel(typeDeclaration.SyntaxTree);

            var declaredSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration);

            if (declaredSymbol is null)
            {
                expected = "Přeložený kód obsahuje deklaraci třídy";
                actual = "Přeložený kód neobsahuje deklaraci třídy";
                return null;
            }

            IEnumerable<IMethodSymbol> methods = declaredSymbol.GetMembers(methodName).OfType<IMethodSymbol>();

            if (!methods.Any())
            {
                expected = $"Přeložený kód neobsahuje metodu {methodName}";
                actual = $"Přeložený kód neobsahuje metodu {methodName}";
                return null;
            }

            IMethodSymbol? resultMethod = null;

            foreach (IMethodSymbol method in methods)
            {
                bool match =
                method.DeclaredAccessibility == accessibility &&
                method.ReturnType.ToDisplayString() == returnType &&
                method.Parameters.Select(p => p.Type.ToDisplayString()).SequenceEqual(parametersNames);

                if (match)
                {
                    resultMethod = method;
                    break;
                }
            }

            expected = $"Návratový typ: {returnType}, parametry: {string.Join(",", parametersNames)}";

            if (resultMethod is null)
            {
                actual = $"Žádná metoda nemá odpovidající typy";
            }

            actual = expected;

            return resultMethod;
        }

        public TestResult TestIfClassExists(int maxPoints, string className)
        {
            string description = $"Třída {className}";

            if (TypeDeclaration<ClassDeclarationSyntax>(className, out string expected, out string actual) is not ClassDeclarationSyntax cds)
            {
                return new TestResult(description, expected, actual, false, maxPoints, 0);
            }

            return new TestResult(description, expected, actual, true, maxPoints, maxPoints);
        }

        public TestResult TestIfInterfaceExists(int maxPoints, string className)
        {
            string description = $"Interface {className}";

            if (TypeDeclaration<InterfaceDeclarationSyntax>(className, out string expected, out string actual) is not InterfaceDeclarationSyntax cds)
            {
                return new TestResult(description, expected, actual, false, maxPoints, 0);
            }

            return new TestResult(description, expected, actual, true, maxPoints, maxPoints);
        }

        public TestResult TestIfMethodExist(int maxPoints, string className, string methodName, string returnTypeName, string[] parametersNames)
        {
            string description = $"Třída {className} metoda: {methodName}";

            if (TypeDeclaration<TypeDeclarationSyntax>(className, out string expectedClass, out string actualClass) is not TypeDeclarationSyntax typeDeclarationSyntax)
            {
                return new TestResult(description, expectedClass, actualClass, false, maxPoints, 0);
            }

            if (MethodDeclaration(methodName, typeDeclarationSyntax, Accessibility.Public, returnTypeName, parametersNames, out string expectedMethod, out string actualMethod) is not IMethodSymbol methodSymbol)
            {
                return new TestResult(description, expectedMethod, actualMethod, false, maxPoints, 0);
            }

            return new TestResult(description, expectedMethod, actualMethod, true, maxPoints, maxPoints);
        }      
    }
}
