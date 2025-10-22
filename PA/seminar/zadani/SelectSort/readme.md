# Alogorimus Selection Sort

**Selection Sort** je jednoduchý algoritmus pro třídění seznamu. Funguje tak, že postupně prochází seznam a na každé pozici vybírá nejmenší (nebo největší) prvek a vymění ho s prvkem na aktuální pozici. Tento proces se opakuje pro všechny prvky seznamu, dokud není seznam seřazený.

## Princip algoritmu

1. Začínáme na začátku seznamu.
2. Najdeme nejmenší prvek v celém seznamu (nebo v části seznamu, která ještě není seřazena).
3. Vyměníme tento nejmenší prvek s prvkem na aktuální pozici.
4. Posuneme se o jednu pozici dál a zopakujeme krok 2 a 3, dokud není seznam celý seřazený.

## Složitost

- **Časová složitost**: O(n²) pro všechny případy (nejlepší, průměrný i nejhorší).
- **Prostorová složitost**: O(1), protože pracuje in-place.

## Výhody a nevýhody

**Výhody**:
- Jednoduchý a snadno pochopitelný.
- Nevyžaduje dodatečnou paměť, takže má nízkou prostorovou složitost.

**Nevýhody**:
- Je pomalý pro větší seznamy, protože jeho časová složitost je kvadratická.
- Neexistuje žádný výrazné zrychlení i v případě, že je seznam částečně seřazený.

## Ukázkový kód v C#

```csharp
using System;

class Program
{
 
    static void SelectionSort(int[] array)
    {
        int n = array.Length;
        
        // Procházejte seznam od začátku
        for (int i = 0; i < n - 1; i++)
        {
            // Hledejte index nejmenšího prvku v nezpracované části seznamu
            int minIndex = i;
            for (int j = i + 1; j < n; j++)
            {
                if (array[j] < array[minIndex])
                {
                    minIndex = j;
                }
            }

            // Vyměňte nalezený nejmenší prvek s prvkem na pozici i
            if (minIndex != i)
            {
                int temp = array[i];
                array[i] = array[minIndex];
                array[minIndex] = temp;
            }
        }
    }

    static void Main()
    {
        int[] array = { 64, 25, 12, 22, 11 };
        
        Console.WriteLine("Původní seznam:");
        Console.WriteLine(string.Join(", ", array));

        // Zavoláme funkci pro seřazení
        SelectionSort(array);

        Console.WriteLine("\nSeřazený seznam:");
        Console.WriteLine(string.Join(", ", array));
    }
}
```

## Zadání

S pomocí kódu stránky níže implementuje algoritmus Selection Sort, tak aby se na stisk tlačítka provedla jen **jedna iterace** a mohli jsme interaktivně sledovat průběh algoritmu.

Výchozí kód stránky:

```csharp
@page "/template"

<PageTitle>Sort</PageTitle>

<div class="d-flex gap-3 mb-3">
    @for (int i = 0; i < pole.Length; i++)
    {
        if (i == index)
        {
            <div class="fs-1 border-bottom border-primary border-3">@pole[i]</div>
        }
        else if(i == j)
        {
            <div class="fs-1 border-bottom border-secondary border-3">@pole[i]</div>
        }
        else
        {
            <div class="fs-1">@pole[i]</div>
        }
    }
</div>

<div class="fs-1 mb-3">minimum @pole[min_index]</div>

<button class="btn btn-primary" @onclick="DalsiIterace">Next Iteration</button>


@code {

    private int[] pole = GenerujCisla(9, 9);

    int index;
    int min_index;
    int j;


    protected override void OnInitialized()
    {
        Inicializace();
    }

    void Inicializace()
    {
        index = 0;
        min_index = 0;
        j = 0;
    }

    void DalsiIterace()
    {
        if(j < pole.Length - 1)
        {
            ++j;


        }
        else
        {


            index = (index + 1) % (pole.Length - 2);
            j = index;
            min_index = index;
        }
    }

    public static int[] GenerujCisla(int n, int max)
    {
        if (n > max)
        {
            throw new ArgumentException("n must be less than or equal to max");
        }

        int[] pole = new int[n];

        for (int i = 0; i < max; ++i)
        {
            if (i < n)
            {
                pole[i] = i + 1;
            }
            else
            {
                int randomIndex = Random.Shared.Next(0, i + 1);

                if (randomIndex < n)
                {
                    pole[randomIndex] = i + 1;
                }
            }
        }

        Random.Shared.Shuffle(pole);

        return pole;
    }
}
```
