# 03: Dědičnost a skládání
**autor: Erik Král ekral@utb.cz**

---


Obsah
- Dědičnost kódu.
- Dědičnost a konstruktor.
- Klíčové slovo protected.
- Kompozice.
- Dědičnost vs kompozice.


## Dědičnost kódu

Dědičnost kódu popisuje vztah specializace mezi třídami, například počítačová myš je (anglicky *IS A*) produkt, nebo tlačítko v aplikaci je ovládací prvek. 

Na následujím příkladu probereme co je to dědičnost kódu a jak ji zapsat.

Třída `Mys` a `Monitor` mají stejné property `Cena` a `Hodnoceni`.

```csharp
class Mys
{
    public double Cena { get; set; }
    public int Hodnoceni { get; set; }
    public int Dpi { get; set; }
}

class Monitor
{
    public double Cena { get; set; }
    public int Hodnoceni { get; set; }
    public int Uhlopricka { get; set; }
}
```

- Proměnné Cena a Hodnocení vytkneme a vložíme dobrodičovské třídy `Produkt`:

```csharp
class Produkt
{
    public double Cena { get; set; }
    public int Hodnoceni { get; set; }
}
```

- Jiná třída potom může zdědit kód této třídy. V následujícím příkladu máme třídu `Mys` jejíž součástí se díky dedičnosti stane kód třídy `Produkt`. Říkáme, že třída `Mys` je potomkem třídy `Produkt`.

```csharp
class Mys : Produkt
{
    public int Dpi { get; set; }
}
```

```csharp
class Monitor : Produkt
{
    public int Uhlopricka { get; set; }
}
```

Základní pojmy

- Třída od které dědíme - synonyma
  - Základní třída (base class)
  - Rodičovská třída (parent class)
  - Nadtřída (superclass)
- Třída, která dědí - synonyma
  -Odvozená třída (derived class)
  - Potomek třídy (child class)
  - Podtřída (subclass)
- Říkáme, že odvozená třída dědí od základní třídy. Ale také můžeme říct:
  - odvozená třída je **specializací**  (Specialization) základní třídy
  - základní třída je **zobecněním** (Generalization) odvozené třídy.
  - odvozená třída **rozšířuje** (Extends) základní třídu


## Dědičnost a konstruktor

Pokud má rodičovská třída **bezparametrický konstruktor**, tak se u potomka zavolá implicitně, nemusíme tedy nic psát.

Pokud má rodičovská třída **parametrický konstruktor**, tak jej musíme u potomka explicitně zavolat pomocí klíčového slova `base`.

Poznámka: v příkladech používáme readonly property, tedy takovou, která má jen ```get;``` a nemá ```set;```

- Nyní si přidáme do třídy `Produkt` parametrický konstruktor:

```csharp
class Produkt
{
    public decimal Cena { get; }
    public int Hodnoceni { get; }

    public Produkt(decimal cena, int hodnoceni)
    {
        Cena = cena;
        Hodnoceni = hodnoceni;
        Console.WriteLine("Produkt byl vytvořen.");
    }
}
```

- Pomocí klíčového slova `base` potom zavoláme parametrický konstruktor rodiče. Konrétně zápis `base(cena, hodnoceni)` v následujícím kódu zavolá parametrický konstruktor třídy `Produkt`. Pokud má rodičovská tříd konstruktor bez parametrů nebo nemá žádný konstruktor, tak klíčové slovo `base` nemusíme použít.

```csharp
class Mys : Produkt
{
    public int Dpi { get; }

    public Mys(decimal cena, int hodnoceni, int dpi) : base(cena, hodnoceni)
    {
        Dpi = dpi;
    }
}
```

- Nakonec vytvoříme instanci třídy `Mys`. Všimněte si, že proměnná `Mys` má property `Cena` a `Hodnocení`, které zdědila od třídy `Produkt`.

```csharp
decimal cena = 600m;
int hodnoceni = 8;
int dpi = 2000;

Mys mys = new (cena, hodnoceni, dpi);

Console.WriteLine($"Pocitacova mys cena: {mys.Cena} hodnoceni: {mys.Hodnoceni} dpi: {mys.Dpi}");
```

> Output:
>
> ```
> Pocitacova mys cena: 600 hodnoceni: 8 dpi: 2000
> 
> ```

### Dědičnost a primary konstruktor

U primary konstruktoru nepoužíváme klíčové slovo ```base```, ale název rodičovské třídy.

```csharp
class Produkt (decimal cena, int hodnoceni)
{
    public decimal Cena { get; } = cena;
    public int Hodnoceni { get; } = hodnoceni;
}

class Mys(decimal cena, int hodnoceni, int dpi) : Produkt(cena, hodnoceni)
{
    public int Dpi { get; } = dpi;
}

Mys mys = new (600m, 8, 2000);

Console.WriteLine($"Pocitacova mys cena: {mys.Cena} hodnoceni: {mys.Hodnoceni} dpi: {mys.Dpi}");
```

> Output:
>
> ```
> Pocitacova mys cena: 600 hodnoceni: 8 dpi: 2000
> 
> ```

- Poznámka: dědičnost kódu se samotná nepoužívá tak často jak by se zdálo, většinou se používá v kombinaci s polymorfismem.

## Klíčové slovo protected

Klíčové slovo `protected` představuje modifikátor přístupu používaný pouze v dědičnosti. Tímto modifikátorem označujeme metody a atributy, které očekáváme, že využije jeho potomek v rámci dědění, ale v klientském kodů mají být skryté. V následujícím příkladu je proměnná `cisloUctu` přístupná v rodičovské třídě `Osoba` a v třídě potomka `Student`, ale není přístupná v klientském kódu v metodě `Main`. Modifikátor příštupu `protected` není běžně používaný a dáváme přednost `private` a `public`, pokud je to možné.


```csharp
class Osoba(int cisloUctu, string jmeno)
{
    protected int cisloUctu = cisloUctu;
    public string Jmeno { get; } = jmeno;
}

class Student(int cisloUctu, string jmeno, string skupina) : Osoba(cisloUctu, jmeno)
{
    public string Skupina { get; } = skupina;

    public void Vypis()
    {
        Console.WriteLine($"{cisloUctu} {Jmeno} {Skupina}"); // jde prelozit
    }
}
```

V klientském kódu potom nemůžeme přistupovat k `protected` fieldu `cisloUctu`.

```csharp
// Klientsky kod
Student student = new (123, "Alena", "AXP1");
student.Vypis();
// student.cisloUctu = 0; // nejde prelozit
```

> Output:
>
> ```
> 123 Alena AXP1
> 
> ```

## Kompozice

Pokud jeden objekt zahrnuje druhý objekt, tak můžeme mluvit o vztahu HAS-A, tedy že jeden objekt má druhý objekt. V následujícím příkladu bude mít instance třídy `Motorka` reference na dvě instance třídy `Kola`. Protože objekt motorka objekty kola nesdílí s jinými objekty jde o kompozici. V tomto případě mluvíme o vlastnictví objektu, kdy vlastnictví objektu (ownership) v tomto kontextu znamená, že když zanikne objekt, tak s ním zaniknou i objekty, které vlastní.

```csharp
class Kolo(int prumer)
{
    public int Prumer { get; } = prumer;
}

class Motorka
{
    private Kolo predni;
    private Kolo zadni;

    public Motorka()
    {
        predni = new Kolo(20);
        zadni = new Kolo(19);
    }
}

Motorka motorka = new();
```

Pokud by objekt sdílel zahrnutý objekt s jinými objekty, tak by šlo o **agregaci**. U agregace mluvíme o tom, že objekt používá jiný objekt ale nevlastní ho. V následujícím příkladu sdílí `smsSender` více objednávek. Pojmy agregace a kompozice vycházejí z jazyka UML.

```csharp
class SmsSender
{
    public void PosliSms(string text)
    {
        Console.WriteLine($"Posilam sms: {text}");
    }
}

class Objednavka (int id, SmsSender sender)
{
    public int Id { get; } = id;
    private SmsSender sender = sender;

    public void Odeslat()
    {
        sender.PosliSms($"Objednavka {Id} odeslana");
    }
}

// klientsky kod
SmsSender smsSender = new ();

Objednavka objednavka1 = new (1, smsSender);
Objednavka objednavka2 = new (2, smsSender);

objednavka1.Odeslat();
objednavka2.Odeslat();

smsSender.PosliSms("Nejsem zavisly na Objednavce");

```

> Output:
>
> ```
> Posilam sms: Objednavka 1 odeslana
> Posilam sms: Objednavka 2 odeslana
> Posilam sms: Nejsem zavisly na Objednavce
> 
> ```

## Dědičnost vs kompozice

Dědičnost kódu můžeme nahradit do určité míry kompozicí. V následujícím příkladu nepoužíváme dědičnost, ale třída `Student` si vytváří vlastní instanci třídy `Osoba`. Všimněte si, že mezi třídami Osoba a Student pořád platí vztah, že Student je Osoba, což je možné u kompozice kde také platí vztah HAS-A. Ale u dědičnsoti musí jít vždy jen o vztah IS-A.

```csharp
class Osoba (int cisloUctu, string jmeno)
{
    public int cisloUctu { get; set; } = cisloUctu;
    public string Jmeno { get; set; } = jmeno;
}

class Student (int cisloUctu, string jmeno, string skupina)
{
    public Osoba Osoba { get; } = new (cisloUctu, skupina);
    public string Skupina { get; set; } = skupina;

    public void Vypis()
    {
        Console.WriteLine($"{Osoba.cisloUctu} {Osoba.Jmeno} {Skupina}"); 
    }
}

// klientsky kod
Student student = new Student(123, "Alena", "AXP1");
student.Osoba.Jmeno = "Tereza";
student.Vypis();
```

> Output:
>
> ```
> 123 Tereza AXP1
> 
> ```

## Příklady k procvičování

### 1. Dědičnost

Vytváříte softwarový systém pro evidenci produktů v eshopu.

Vytvořte pro třídy společného předka ```Control``` a dejte do něj vhodné property abychom nemuseli opakovat kód.


```csharp
// Doplnte a upravte kod

Sluchatka s = new()
{
    Id = 1,
    Cena = 3900m,
    PocetSkladem = 20,
    Protoly = ["SBC", "AAC", "aptX", "LDAC"]
};

Kniha d = new()
{
    Id = 1,
    Cena = 3900m,
    PocetSkladem = 700,
    Nazev = "Babicka"
};
```

### 2. Klíčové slovo protected

Vytváříte software pro evidenci vyučujících ve Stagu.

Máte třídu ```Osoba``` (rodičovská třída) a ```Vyucujici``` (potomek).
Zvolte správnou úroveň zabezpečení dat. V reálném softwaru nesmí být všechno public, aby nedošlo v kódu omylem k nechtěnému přepsání údajů a z toho plynoucím chybám.

Třída ``Osoba`` bude obsahovat tři členy, Zvolte pro každé pole správné klíčové slovo ze trojice: public, protected, private:

- property *Jmeno* – veřejný údaj, přístupný odkudkoliv z programu.
- field *cisloUctu* (typ int) – citlivý údaj, ke kterému smí přistupovat pouze samotný zaměstnanec a třídy, které z něj dědí (např. manažer). Zvenčí programu musí být skrytý. Inicializujte na hodnotu 123.
- field *heslo* (typ string) – nejpřísněji střežený údaj. Smí se k němu přistupovat výhradně uvnitř třídy Zamestnanec. Žádná jiná třída (ani odvozená) k němu nemá přístup. Inicializujte jej na hodnotu "abc123".

Třída ```Vyucujici``` bude dědit z třídy ```Osoba``` a

- Uvnitř třídy Manazer vytvořte metodu VypisUdaje(). V ní vypište do konzole údaje, které jsou pro manazera přístupné.
- Uvnitř třídy Zamestnanec vytvořte metodu VypisUdaje(). V ní vypište do konzole údaje, které jsou pro zaměstnance přístupné.

```csharp
// Zde doplnte definice trid

Vyucujici v = new()
{
    Jmeno = "Petr"
};

v.VypisUdaje();
v.VypisUdaje();
```

### 3. Dědičnost a konstruktory

Vyvíjíte softwarový systém pro evidenci zvířátek v ZOO.

Vytvořte třídu ```Zviratko```, která bude reprezentovat obecné zvířátko:
- Třída bude mít public readonly (bez setteru) property ```Jmeno``` typu string.
- Vytvořte pro tuto třídu konstruktor, který bude přijímat parametr ```jmeno``` a nastaví hodnotu property.
Vytvořte třídu ```Jaguar```, která dědí z třídy ```Zviratko```:
- Přidejte do ní jednu novou readonly (bez setteru) public property ```PocetSkvrn``` typu celé číslo.
- Vytvořte pro třídu ```Jaguar``` konstruktor, který bude přijímat dva parametry: ```jmeno``` a pocet ```skrvn```.
- Předejte potřebné parametry konstruktoru rodičovské třídy ```Zviratko```.

```csharp
Jaguar mlade = new("Mayara", 0);
```

### 4. Skládání objektů

Upravte předchozí příklady tak, aby místo dědičnosti používali skládání objektů.
