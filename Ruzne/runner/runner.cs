using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;

var stdout = Console.Out;
var dummyWritter = new StringWriter();
Console.SetOut(dummyWritter);

bool tZakaznikProperty = typeof(Zakaznik).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(p => p.GetSetMethod() is null).Select(p => p.Name).SequenceEqual(["Jmeno", "Email"]);
bool tFakturaProperty = typeof(Faktura).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(p => p.GetSetMethod() is null).Select(p => p.Name).SequenceEqual(["PocetPolozek", "CenaCelkem", "Zakaznik"]);

var zakaznikKonstruktory = typeof(Zakaznik).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
bool tZakaznikKonstruktor = zakaznikKonstruktory.Length == 1 && zakaznikKonstruktory.First().GetParameters().Select(p => p.Name).SequenceEqual(["jmeno", "email"]);
var fakturaKonstruktory = typeof(Faktura).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
bool tFakturaKonstruktor = fakturaKonstruktory.Length == 1 && fakturaKonstruktory.First().GetParameters().Select(p => p.Name).SequenceEqual(["pocetPolozek", "cenaCelkem", "jmeno", "email"]);

bool tZadnaDedicnost = !typeof(Zakaznik).IsAssignableFrom(typeof(Faktura)) && !typeof(Faktura).IsAssignableFrom(typeof(Zakaznik));

bool t1;

try
{
    Faktura f1 = new(3, 1500.50m, "Petr", "petr@example.com");

    t1 = f1.PocetPolozek == 3 && f1.CenaCelkem == 1500.50m && f1.Zakaznik?.Jmeno == "Petr" && f1.Zakaznik?.Email == "petr@example.com";
}
catch
{
    t1 = false;
}

bool t2;

try
{
    Faktura f2 = new(5, 2500.00m, "Jana", "jana@example.com");

    t2 = f2.PocetPolozek == 5 && f2.CenaCelkem == 2500.00m && f2.Zakaznik?.Jmeno == "Jana" && f2.Zakaznik?.Email == "jana@example.com";
}
catch
{
    t2 = false;
}

int body = 0;

if (tZakaznikProperty) body += 1;
if (tZakaznikKonstruktor) body += 1;
if (tFakturaProperty) body += 1;
if (tFakturaKonstruktor) body += 1;
if (tZadnaDedicnost) body += 1;
if (t1) body += 1;
if (t2) body += 1;

double fraction = body / 7.0;

Console.SetOut(stdout);

string message(bool t) => t ? "prosel" : "chyba";
string jsonBool(bool t) => t ? "true" : "false";

string jsonOutput = $$"""
{
    "fraction": {{fraction.ToString("G", CultureInfo.InvariantCulture)}},
    "testresults":
    [
        ["Nazev testu", "Ocekavana hodnota", "Vysledek", "iscorrect"],
        ["property Jmeno a Email", "OK", "{{message(tZakaznikProperty)}}", {{jsonBool(tZakaznikProperty)}}],
        ["property PocetPolozek a CenaCelkem", "OK", "{{message(tFakturaProperty)}}", {{jsonBool(tFakturaProperty)}}],
        ["konstruktor Zakaznik", "OK", "{{message(tZakaznikKonstruktor)}}", {{jsonBool(tZakaznikKonstruktor)}}],
        ["konstruktor Faktura", "OK", "{{message(tFakturaKonstruktor)}}", {{jsonBool(tFakturaKonstruktor)}}],
        ["Dedicnost", "NE", "{{message(tZadnaDedicnost)}}", {{jsonBool(tZadnaDedicnost)}}],
        ["new Faktura(3, 1500.50m, \"Petr\", \"petr@example.com\")", "OK", "{{message(t1)}}", {{jsonBool(t1)}}],
        ["new Faktura(5, 2500.00m, \"Jana\", \"jana@example.com\")", "OK", "{{message(t2)}}", {{jsonBool(t2)}}]
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