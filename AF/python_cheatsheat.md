# C# for Python Programmers: Quick Reference Sheet

Tento přehled slouží k rychlému „přepnutí“ z Pythonu do C#. Pozor na nejčastější chyby: **středníky**, **závorky** a **datové typy**.

---

## 1. Základní syntaxe


| Vlastnost | Python 🐍 | C# 🎯 |
| :--- | :--- | :--- |
| **Ukončení příkazu** | Nový řádek | **Středník ;** |
| **Bloky kódu** | Odsazení (indent) | **Složené závorky { }** |
| **Logické operátory** | and, or, not | &&, ||, ! |
| **Komentáře** | # komentář | // (řádkový) nebo /* */ (blokový) |

---

## 2. Proměnné a typy
C# vyžaduje deklaraci typu (static typing).

```csharp
// Python: x = 10
int x = 10; 

// Python: name = "Alice"
string name = "Alice"; 

// Python: pi = 3.14
double pi = 3.14; 

// Automatický odhad typu (pouze uvnitř metod)
var autoTyp = "Tohle bude string"; 
```
---

## 3. Podmínky a Cykly

Podmínka musí být vždy v **kulatých závorkách ( )**.

### If-Else

Python:

```python
if x > 0:
    print("Kladné")
elif x < 0:
    print("Záporné")
else:
    print("Nula")
```

C#

```csharp
if (x > 0) 
{
    Console.WriteLine("Kladné");
} 
else if (x < 0) 
{
    Console.WriteLine("Záporné");
} 
else 
{
    Console.WriteLine("Nula");
}
```

### Pole


```csharp
// Python: pole = [1, 2, 3]
int[] pole = [1, 2, 3];

// Python: 
// pole = [0] * 3
// delka_pole = len(pole)
int[] pole = new int[3]; // tri defaultni prvky
int delkaPole = pole.Length;

// Python: 
// dynamickePole = []
// pocet_prvku = len(dynamicke_pole)
List<int> dynamickePole = [];
int pocetPrvku = dynamickePole.Count;
```

### Cykly

```csharp
// For cyklus (od 0 do 4)

// Python: 
// for i in range(5):
//     print(i)
for (int i = 0; i < 5; i++) 
{
    Console.WriteLine(i);
}

// Foreach (iterace kolekcí)

// Python: 
// for prvek in pole:
//     print(prvek)
foreach (var prvek in pole) 
{
    Console.WriteLine(prvek);
}
```

---

## 4. Metody (Funkce)
V C# musí mít každá metoda definovaný návratový typ (nebo void).

```csharp
// Python: def pozdrav(jmeno):
public void Pozdrav(string jmeno) 
{
    Console.WriteLine($"Ahoj {jmeno}");
}

// Python: def scitej(a, b): return a + b
public int Scitej(int a, int b) 
{
    return a + b;
}
```

---

## 5. Třídy a instance tříd

Místo self používáme this. Konstruktor má stejné jméno jako třída.

```csharp
public class Student 
{
    public string Jmeno { get; set; } // Property
    public int Body { get; set; }

    // Konstruktor (Python __init__)
    public Student(string jmeno) 
    {
        this.Jmeno = jmeno;
    }
}
```
Instance třídy

```csharp
// Python: pavel = Student("Pavel")
Student pavel = new Student("Pavel");

// Python: 
// karel = Student("Karel")
// karel.body = 40
Student karel = new Karel("Karel") { Body = 40 }; // Object initializer 
```
---

## 6. LINQ vs. List Comprehension
Místo zkratek z Pythonu používáme v C# LINQ.

```csharp
// Python: filtered = [x for x in data if x > 5]
var filtered = data.Where(x => x > 5).ToList();

// Python: names = [s.name for s in students]
var names = students.Select(s => s.Jmeno).ToList();
```

---

## 7. Práce s null (None)
Pozor na NullReferenceException.
- **Python:** ``` if x is None``` :
- **C#:** ``` if (x == null) { ... } nebo moderní if (x is null) { ... } ```

---

### 💡 Rychlá pomoc při chybách:
1. **Překladač píše "Semicolon expected"?** Chybí ti ; na konci řádku.
2. **Překladač píše "The name '...' does not exist in the current context"?** Zkontroluj velká/malá písmena (C# je case-sensitive) nebo zda máš správný using.
3. **Pleteš si ' a "?** V C# jsou 'a' (char - jeden znak) a "abc" (string - text) dva různé typy.
