using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;

var stdout = Console.Out;
var dummyWritter = new StringWriter();
Console.SetOut(dummyWritter);

bool tVozidloProperty = typeof(Vozidlo).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(p => p.GetSetMethod() is null).Select(p => p.Name).SequenceEqual(["Znacka", "Model"]);
bool tNakladniVozidloProperty = typeof(NakladniVozidlo).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(p => p.GetSetMethod() is null).Select(p => p.Name).SequenceEqual(["Nosnost"]);

var vozidloKonstruktory = typeof(Vozidlo).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
bool tVozidloKonstruktor = vozidloKonstruktory.Length == 1 && vozidloKonstruktory[0].GetParameters().Length > 0;

var nakladniVozidloKonstruktory = typeof(NakladniVozidlo).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
bool tNakladniVozidloKonstruktor = nakladniVozidloKonstruktory.Length == 1 && nakladniVozidloKonstruktory[0].GetParameters().Length > 0;

bool tDedicnost = typeof(Vozidlo).IsAssignableFrom(typeof(NakladniVozidlo));

bool t1;

try
{
    NakladniVozidlo vozidlo = new("Tatra", "Phoenix", 11000);

    t1 = vozidlo.Znacka == "Tatra" && vozidlo.Model == "Phoenix" && vozidlo.Nosnost == 11000;
}
catch
{
    t1 = false;
}

bool t2;

try
{
    NakladniVozidlo vozidlo = new("Citroen", "Jumper", 1500);

    t2 = vozidlo.Znacka == "Citroen" && vozidlo.Model == "Jumper" && vozidlo.Nosnost == 1500;
}
catch
{
    t2 = false;
}

int body = 0;


if (tNakladniVozidloProperty) body += 1;
if (tNakladniVozidloKonstruktor) body += 1;
if (t1) body += 1;
if (t2) body += 1;

if (!tVozidloProperty || !tVozidloKonstruktor || !tDedicnost) body = 0;

double fraction = body / 4.0;

Console.SetOut(stdout);

string message(bool t) => t ? "prosel" : "chyba";
string jsonBool(bool t) => t ? "true" : "false";

string jsonOutput = $$"""
{
    "fraction": {{fraction.ToString("G", CultureInfo.InvariantCulture)}},
    "testresults":
    [
        ["Nazev testu", "Ocekavana hodnota", "Vysledek", "iscorrect"],
        ["property Znacka a Model", "OK", "{{message(tVozidloProperty)}}", {{jsonBool(tVozidloProperty)}}],
        ["property Nosnost", "OK", "{{message(tNakladniVozidloProperty)}}", {{jsonBool(tNakladniVozidloProperty)}}],
        ["konstruktor Vozidlo", "OK", "{{message(tVozidloKonstruktor)}}", {{jsonBool(tVozidloKonstruktor)}}],
        ["konstruktor NakladniVozidlo", "OK", "{{message(tNakladniVozidloKonstruktor)}}", {{jsonBool(tNakladniVozidloKonstruktor)}}],
        ["Dedicnost", "OK", "{{message(tDedicnost)}}", {{jsonBool(tDedicnost)}}],
        ["new NakladniVozidlo(\"Tatra\", \"Phoenix\", 11000)", "OK", "{{message(t1)}}", {{jsonBool(t1)}}],
        ["new NakladniVozidlo(\"Citroen\", \"Jumper\", 1500)", "OK", "{{message(t2)}}", {{jsonBool(t2)}}]
    ]
}
""";

Console.Write(jsonOutput);

public static class TestConsole
{
    public static string Message {get; private set;} = string.Empty;

    public static void WriteLine(string message)
    {
        Message = message;
    }
}