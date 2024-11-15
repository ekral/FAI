# Úkol: Reference a přetěžování operátorů v C++

## Cíl úkolu:

Cílem tohoto úkolu je seznámit se s těmito základními koncepty v C++:

- Použití referencí
- Přetěžování operátorů

## Zadání:

1. Struktura ComplexNumber

- Vytvořte strukturu ComplexNumber, která bude reprezentovat komplexní číslo. Struktura by měla obsahovat:
- Dva public členy: real (reálná část) a imaginary (imaginární část) typu double.
    - Konstruktory:
        - Výchozí konstruktor (nastaví oba členy na nulu).
        - Konstruktor s parametry pro inicializaci reálné a imaginární části.
    - Metody:
        - display(), která zobrazí komplexní číslo ve formátu "a + bi".

2. Přetěžování operátorů

- Implementujte následující operátory:
    - Operátor sčítání (+), který umožní sečítat dvě komplexní čísla.
    - Operátor přiřazení (=), který správně přiřadí jedno komplexní číslo druhému.
    - Operátor rovnosti (==), který správně přiřadí jedno komplexní číslo druhému.
    - Operátor nerovnosti (!=), který správně přiřadí jedno komplexní číslo druhému.
    - Operátor výstupu (<<), který umožní vypsat komplexní číslo pomocí std::cout.


3. Funkce main
    
Ve funkci main vytvořte několik objektů třídy ComplexNumber. Použijte různé konstruktory a operátory (včetně přetížených) a zobrazte výsledky.

## Doplňující požadavky:

Použijte správnou syntaxi pro přetěžování operátorů a dodržujte konvence v C++.
