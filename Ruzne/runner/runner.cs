using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;

var stdout = Console.Out;
var dummyWritter = new StringWriter();
Console.SetOut(dummyWritter);

bool test(Func<bool> action)
{
    bool result;

    try
    {
        result = action.Invoke();
    }
    catch
    {
        result = false;
    }

    return result;
}

bool t1 = test( () => { TestConsole.Message = string.Empty; Solution.Test(2, 1); return TestConsole.Message == "vetsi je a: 2"; });
bool t2 = test( () => { TestConsole.Message = string.Empty; Solution.Test(1, 3); return TestConsole.Message == "vetsi je b: 3"; });
bool t3 = test( () => { TestConsole.Message = string.Empty; Solution.Test(6, 5); return TestConsole.Message == "vetsi je a: 6"; });
bool t4 = test( () => { TestConsole.Message = string.Empty; Solution.Test(7, 8); return TestConsole.Message == "vetsi je b: 8"; });
bool t5 = test( () => { TestConsole.Message = string.Empty; Solution.Test(4, 4); return TestConsole.Message == "jsou stejne: a: 4 b: 4"; });
bool t6 = test( () => { TestConsole.Message = string.Empty; Solution.Test(9, 9); return TestConsole.Message == "jsou stejne: a: 9 b: 9"; });
bool t7 = test( () => { TestConsole.Message = string.Empty; Solution.Test(4, 9); return TestConsole.Message != "jsou stejne: a: 4 b: 9"; });

int body = 0;

if (t1 && t2 && t3 && t4) body += 1;
if (t5 && t6 && t7) body += 1;

double fraction = body / 2.0;

Console.SetOut(stdout);

string message(bool t) => t ? "prosel" : "chyba";
string jsonBool(bool t) => t ? "true" : "false";

string jsonOutput = $$"""
{
    "fraction": {{fraction.ToString("G", CultureInfo.InvariantCulture)}},
    "testresults":
    [
        ["Nazev testu", "Ocekavana hodnota", "Vysledek", "iscorrect"],
        ["Test(2, 1)", "vetsi je a: 2", "{{message(t1)}}", {{jsonBool(t1)}}],
        ["Test(1, 2)", "vetsi je b: 2", "{{message(t2)}}", {{jsonBool(t2)}}],
        ["Test(6, 5)", "vetsi je a: 6", "{{message(t3)}}", {{jsonBool(t3)}}],
        ["Test(7, 8)", "vetsi je b: 8", "{{message(t4)}}", {{jsonBool(t4)}}],
        ["Test(4, 4)", "jsou stejne: a: 4 b: 4", "{{message(t5)}}", {{jsonBool(t5)}}],
        ["Test(9, 9)", "jsou stejne: a: 9 b: 9", "{{message(t6)}}", {{jsonBool(t6)}}],
        ["Test(4, 9)", "nejsou stejne", "{{message(t7)}}", {{jsonBool(t7)}}]
    ]
}
""";

Console.Write(jsonOutput);

public static class TestConsole
{
    public static string Message {get; set;} = string.Empty;

    public static void WriteLine(string message)
    {
        Message += message;
    }
}