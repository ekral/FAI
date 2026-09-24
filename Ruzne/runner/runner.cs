using System;
using System.Globalization;
using System.IO;

var stdout = Console.Out;
var dummyWritter = new StringWriter();
Console.SetOut(dummyWritter);

Solution solution = new();

bool t1 = solution.VratDruhouMocninu(5) == 25;
bool t2 = solution.VratDruhouMocninu(10) == 100;
bool t3 = solution.VratDruhouMocninu(-3) == 9;

int celkoveBody = (t1 ? 3 : 0) + (t2 ? 3 : 0) + (t3 ? 4 : 0);
double fraction = celkoveBody / 10.0;

Console.SetOut(stdout);

string message(bool t) => t ? "prosel" : "chyba";
string jsonBool(bool t) => t ? "true" : "false";

string jsonOutput = $$"""
{
    "fraction": {{fraction.ToString("G", CultureInfo.InvariantCulture)}},
    "testresults":
    [
    ["Nazev testu", "Ocekavana hodnota", "Vysledek", "iscorrect"],
    ["Test 1: VratDruhouMocninu(5)", "25", "{{message(t1)}}", {{jsonBool(t1)}}],
    ["Test 2: VratDruhouMocninu(10)", "100", "{{message(t2)}}", {{jsonBool(t2)}}],
    ["Test 3: VratDruhouMocninu(-3)", "9", "{{message(t3)}}", {{jsonBool(t3)}}]
    ]
}
""";

Console.Write(jsonOutput);