using System;
using System.Globalization;
using System.IO;

var stdout = Console.Out;
var dummyWritter = new StringWriter();
Console.SetOut(dummyWritter);

Solution solution = new();

bool t1 = solution.VratSumu([ 0, 0, 0 ]) == 0;
bool t2 = solution.VratSumu([ 1, 2, 3 ]) == 6;
bool t3 = solution.VratSumu([ -7, -8]) == -15;
bool t4 = solution.VratSumu([ 7]) == 7;
bool t5 = solution.VratSumu([]) == 0;

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
    ["Test 1: VratSumu([ 0, 0, 0 ])", "0", "{{message(t1)}}", {{jsonBool(t1)}}],
    ["Test 2: VratSumu([ 1, 2, 3 ])", "6", "{{message(t2)}}", {{jsonBool(t2)}}],
    ["Test 3: VratSumu([ -7, -8 ])", "-15", "{{message(t3)}}", {{jsonBool(t3)}}],
    ["Test 4: VratSumu([ 7 ])", "7", "{{message(t4)}}", {{jsonBool(t4)}}],
    ["Test 5: VratSumu([])", "0", "{{message(t5)}}", {{jsonBool(t5)}}]
    ]
}
""";

Console.Write(jsonOutput);