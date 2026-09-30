using System;
using System.Globalization;
using System.IO;

var stdout = Console.Out;
var dummyWritter = new StringWriter();
Console.SetOut(dummyWritter);

Solution solution = new();

solution.Test(3);
bool t1 = TestConsole.Message == "obvod: 12";
solution.Test(4);
bool t2 = TestConsole.Message == "obvod: 16";
solution.Test(7);
bool t3 = TestConsole.Message == "obvod: 28";
solution.Test(9);
bool t4 = TestConsole.Message == "obvod: 36";


double fraction = t1 && t2 && t3 && t4 ? 1.0 : 0.0;

Console.SetOut(stdout);

string message(bool t) => t ? "prosel" : "chyba";
string jsonBool(bool t) => t ? "true" : "false";

string jsonOutput = $$"""
{
    "fraction": {{fraction.ToString("G", CultureInfo.InvariantCulture)}},
    "testresults":
    [
        ["Nazev testu", "Ocekavana hodnota", "Vysledek", "iscorrect"],
        ["solution.Test(3)", "obvod: 12", "{{message(t1)}}", {{jsonBool(t1)}}],
        ["solution.Test(4)", "obvod: 16", "{{message(t2)}}", {{jsonBool(t2)}}],
        ["solution.Test(7)", "obvod: 28", "{{message(t3)}}", {{jsonBool(t3)}}],
        ["solution.Test(9)", "obvod: 36", "{{message(t4)}}", {{jsonBool(t4)}}]
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