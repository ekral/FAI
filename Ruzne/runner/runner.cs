using System;
using System.Globalization;
using System.IO;

Solution solution = new();

var stdout = Console.Out;
var dummyWritter = new StringWriter();
Console.SetOut(dummyWritter);

solution.Test(2, -7, 3);
bool t1 = TestConsole.Message == "diskriminant: 25.0";
solution.Test(4, 12, 9);
bool t2 = TestConsole.Message == "diskriminant: 0.0";
solution.Test(1, 2, 5);
bool t3 = TestConsole.Message == "diskriminant: -16.0";

double fraction = t1 && t2 && t3 ? 1.0 : 0.0;

Console.SetOut(stdout);

string message(bool t) => t ? "prosel" : "chyba";
string jsonBool(bool t) => t ? "true" : "false";

string jsonOutput = $$"""
{
    "fraction": {{fraction.ToString("G", CultureInfo.InvariantCulture)}},
    "testresults":
    [
        ["Nazev testu", "Ocekavana hodnota", "Vysledek", "iscorrect"],
        ["solution.Test(2, -7, 3)", "diskriminant: 25.0", "{{message(t1)}}", {{jsonBool(t1)}}],
        ["solution.Test(4, 12, 9)", "diskriminant: 0.0", "{{message(t2)}}", {{jsonBool(t2)}}],
        ["solution.Test(1, 2, 5)", "diskriminant: -16.0", "{{message(t3)}}", {{jsonBool(t3)}}]
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