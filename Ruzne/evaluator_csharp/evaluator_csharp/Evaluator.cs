using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.Loader;

namespace Evaluator
{
    public class Evaluator(string code)
    {
        private readonly string code = code;
        private SyntaxTree? syntaxTree;
        private CSharpCompilation? compilation;
        private ImmutableArray<Diagnostic>? diagnostics;
        private Assembly? assembly;

        private readonly PortableExecutableReference[] references =
        [
            MetadataReference.CreateFromStream(typeof(Program).Assembly.GetManifestResourceStream("refs/System.Collections.dll")!),
            MetadataReference.CreateFromStream(typeof(Program).Assembly.GetManifestResourceStream("refs/System.Linq.dll")!),
            MetadataReference.CreateFromStream(typeof(Program).Assembly.GetManifestResourceStream("refs/System.Runtime.dll")!),
            CreateSelfReference()
        ];

        public SyntaxTree SyntaxTree
        {
            get
            {
                return syntaxTree ??= CSharpSyntaxTree.ParseText(code);
            }
        }

        public CSharpCompilation Compilation
        {
            get
            {
                return compilation ??= CSharpCompilation.Create(
                    assemblyName: "Submission",
                    syntaxTrees: [SyntaxTree],
                    references: references
                ).WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            }
        }

        public ImmutableArray<Diagnostic> Diagnostics
        {
            get
            {
                if (!diagnostics.HasValue)
                {
                    Compile();
                }

                return diagnostics!.Value;
            }
        }

        public Assembly? Assembly
        {
            get
            {
                if (!diagnostics.HasValue)
                {
                    Compile();
                }

                return assembly;
            }
        }

        private void Compile()
        {
            using var ms = new MemoryStream();

            EmitResult result = Compilation.Emit(ms);
            diagnostics = result.Diagnostics;

            if (result.Success)
            {
                ms.Position = 0;
                var context = new AssemblyLoadContext("dynamic", isCollectible: true);
                assembly = context.LoadFromStream(ms);
            }
        }

        private static unsafe PortableExecutableReference CreateSelfReference()
        {
            Assembly assembly = typeof(Program).Assembly;

            if (!assembly.TryGetRawMetadata(out byte* blob, out int length))
                throw new InvalidOperationException("Nepodařilo se získat metadata assembly.");

            var moduleMetadata = ModuleMetadata.CreateFromMetadata((IntPtr)blob, length);
            return AssemblyMetadata.Create(moduleMetadata).GetReference();
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

        private IMethodSymbol? MethodDeclaration(string methodName, TypeDeclarationSyntax typeDeclaration, Accessibility accessibility, string returnType, string[] parametersTypes, out string expected, out string actual)
        {
            SemanticModel semanticModel = Compilation.GetSemanticModel(typeDeclaration.SyntaxTree);

            INamedTypeSymbol? declaredSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration);

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
                method.ReturnType.Name == returnType &&
                method.Parameters.Select(p => p.Type.Name).SequenceEqual(parametersTypes);

                if (match)
                {
                    resultMethod = method;
                    break;
                }
            }

            expected = $"Návratový typ: {returnType}, parametry: {string.Join(",", parametersTypes)}";

            if (resultMethod is null)
            {
                actual = $"Žádná metoda nemá odpovidající typy";
                return null;
            }

            actual = expected;

            return resultMethod;
        }

        private bool ClassImplementsInterface(InterfaceDeclarationSyntax interfaceDeclaration, ClassDeclarationSyntax classDeclaration, out string expected, out string actual)
        {
            SemanticModel semanticModel = Compilation.GetSemanticModel(SyntaxTree);

            if (semanticModel.GetDeclaredSymbol(classDeclaration) is not INamedTypeSymbol classSymbol)
            {
                expected = $"Přeložený kód obsahuje deklaraci třídy {classDeclaration.Identifier.ValueText}";
                actual = $"Přeložený kód neobsahuje deklaraci třídy {classDeclaration.Identifier.ValueText}";
                return false;
            }

            if (semanticModel.GetDeclaredSymbol(interfaceDeclaration) is not INamedTypeSymbol interfaceSymbol)
            {
                expected = $"Přeložený kód obsahuje deklaraci rozhraní {interfaceDeclaration.Identifier.ValueText}";
                actual = $"Přeložený kód neobsahuje deklaraci rozhraní {interfaceDeclaration.Identifier.ValueText}";
                return false;
            }

            if(!classSymbol.Interfaces.Contains(interfaceSymbol, SymbolEqualityComparer.Default))
            {
                expected = $"Třída {classDeclaration.Identifier.ValueText} dědí od {interfaceDeclaration.Identifier.ValueText}";
                actual = $"Třída {classDeclaration.Identifier.ValueText} nedědí od {interfaceDeclaration.Identifier.ValueText}";
                return false;
            }

            expected = $"Třída {classDeclaration.Identifier.ValueText} implementuje rozhraní {interfaceDeclaration.Identifier.ValueText}";

            foreach (ISymbol symbol in interfaceSymbol.GetMembers())
            {
                if (classSymbol.FindImplementationForInterfaceMember(symbol) is null)
                {
                    actual = $"Třída {classDeclaration.Identifier.ValueText} neimplementuje rozhraní {interfaceDeclaration.Identifier.ValueText}";
                    return false;
                }
            }

            actual = expected;

            return true;
        }

        private bool ClassHasConstructor(ClassDeclarationSyntax classDeclaration, string[] parametersTypes, out string expected, out string actual)
        {
            SemanticModel semanticModel = Compilation.GetSemanticModel(SyntaxTree);

            if (semanticModel.GetDeclaredSymbol(classDeclaration) is not INamedTypeSymbol classSymbol)
            {
                expected = $"Přeložený kód obsahuje deklaraci třídy {classDeclaration.Identifier.ValueText}";
                actual = $"Přeložený kód neobsahuje deklaraci třídy {classDeclaration.Identifier.ValueText}";
                return false;
            }

            IMethodSymbol? constructor = classSymbol.InstanceConstructors
               .FirstOrDefault(ms => ms.Parameters
                    .Select(p => p.Type.Name)
                    .SequenceEqual(parametersTypes));

            expected = $"Třída {classDeclaration.Identifier.ValueText} má správný konstruktor";

            if(constructor is null)
            {
                actual = $"Třída {classDeclaration.Identifier.ValueText} nemá správný konstruktor";
                return false;
            }

            actual = expected;

            return true;
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

        public TestResult TestIfConstructorExists(int maxPoints, string className, Accessibility accessibility, string[] parametersTypes)
        {
            string description = $"Konstruktor třídy {className}";

            if (TypeDeclaration<ClassDeclarationSyntax>(className, out string expectedClass, out string actualClass) is not ClassDeclarationSyntax classDeclarationSyntax)
            {
                return new TestResult(description, expectedClass, actualClass, false, maxPoints, 0);
            }

            if (!ClassHasConstructor(classDeclarationSyntax, parametersTypes, out string expectedConstructor, out string actualConstructor))
            {
                return new TestResult(description, expectedConstructor, actualConstructor, false, maxPoints, 0);
            }

            return new TestResult(description, expectedConstructor, actualConstructor, true, maxPoints, maxPoints);
        }

        public TestResult TestIfMethodExists(int maxPoints, string className, string methodName, Accessibility accessibility, string returnType, string[] parametersTypes)
        {
            string description = $"Třída {className} metoda: {methodName}";

            if (TypeDeclaration<TypeDeclarationSyntax>(className, out string expectedClass, out string actualClass) is not TypeDeclarationSyntax typeDeclarationSyntax)
            {
                return new TestResult(description, expectedClass, actualClass, false, maxPoints, 0);
            }

            if (MethodDeclaration(methodName, typeDeclarationSyntax, accessibility, returnType, parametersTypes, out string expectedMethod, out string actualMethod) is not IMethodSymbol methodSymbol)
            {
                return new TestResult(description, expectedMethod, actualMethod, false, maxPoints, 0);
            }

            return new TestResult(description, expectedMethod, actualMethod, true, maxPoints, maxPoints);
        }

        public TestResult TestIfClassImplementsInterface(int maxPoints, string interfaceName, string className)
        {
            string description = $"Třída {className} a rozhraní {interfaceName}";

            if (TypeDeclaration<ClassDeclarationSyntax>(className, out string expectedClass, out string actualClass) is not ClassDeclarationSyntax classDeclarationSyntax)
            {
                return new TestResult(description, expectedClass, actualClass, false, maxPoints, 0);
            }

            if (TypeDeclaration<InterfaceDeclarationSyntax>(interfaceName, out string expectedInterface, out string actualInterface) is not InterfaceDeclarationSyntax interfaceDeclarationSyntax)
            {
                return new TestResult(description, expectedClass, actualClass, false, maxPoints, 0);
            }

            if(!ClassImplementsInterface(interfaceDeclarationSyntax, classDeclarationSyntax, out string expectedImplementation, out string actualImplementation))
            {
                return new TestResult(description, expectedImplementation, actualImplementation, false, maxPoints, 0);
            }

            return new TestResult(description, expectedImplementation, actualImplementation, true, maxPoints, maxPoints);
        }
    }
}
