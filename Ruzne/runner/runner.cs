using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

var stdout = Console.Out;
var dummyWritter = new StringWriter();
Console.SetOut(dummyWritter);

Solution solution = new();

bool exceptionThrown = false;

try
{
    solution.Run();
}
catch (Exception)
{
    exceptionThrown = true;
}

bool t1 = MujStream.called;
bool t2 = MujStream.disposed;

var code = File.ReadAllText("solution.cs");

bool t3 = !Regex.IsMatch(code, @"\bfinally\b");

bool t4 = exceptionThrown;

bool t5 = !Regex.IsMatch(code, @"\bstream\s*\.\s*Dispose\b");

double fraction = t1 && t2 && t3 && t4 && t5 ? 1.0 : 0.0;

Console.SetOut(stdout);

string message(bool t) => t ? "prosel" : "chyba";
string jsonBool(bool t) => t ? "true" : "false";

string jsonOutput = $$"""
{
    "fraction": {{fraction.ToString("G", CultureInfo.InvariantCulture)}},
    "testresults":
    [
        ["Nazev testu", "Ocekavana hodnota", "Vysledek", "iscorrect"],
        ["Vola se WriteLine", "ano", "{{message(t1)}}", {{jsonBool(t1)}}],
        ["Vola se vzdy Dispose", "ano", "{{message(t2)}}", {{jsonBool(t2)}}],
        ["Nepouziva se try finally", "ano", "{{message(t3)}}", {{jsonBool(t3)}}],
        ["Nepouziva se try catch", "ano", "{{message(t4)}}", {{jsonBool(t4)}}],
        ["Nepouziva se uvolneni v kodu", "ano", "{{message(t5)}}", {{jsonBool(t5)}}]
    ]
}
""";

Console.Write(jsonOutput);

class TestConsole
{
    public static string Message {get; private set;} = string.Empty;

    public static void WriteLine(string message)
    {
        Message = message;
    }
}

class MujStream : IDisposable
{
    public static bool disposed = false;
    public static bool called = false;

    public void WriteLine(string message)
    {
        called = true;
        throw new Exception();
    }

    public void Dispose()
    {
        disposed = true;
    }
}