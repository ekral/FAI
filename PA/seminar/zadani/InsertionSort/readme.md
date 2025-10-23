# Insertion Sort v C#

## Popis

**Insertion Sort** (Třídění vkládáním) je jednoduchý algoritmus pro třídění, který postupně vkládá každý prvek na správné místo do již seřazené části seznamu.

Algoritmus funguje takto:
1. Začíná od druhého prvku (první prvek je považován za již seřazený).
2. Tento prvek porovná s předchozími prvky a pokud je menší než některý z nich, posune je o jednu pozici doprava.
3. Poté tento prvek vloží na správné místo.
4. Tento proces se opakuje pro každý další prvek, dokud není celý seznam seřazený.

## Časová složitost:
- **Nejlepší případ:** O(n), pokud je seznam již seřazený.
- **Průměrný a nejhorší případ:** O(n²), kde n je počet prvků v seznamu.
- **Prostorová složitost:** O(1), protože algoritmus třídí seznam na místě.

## Ukázka kódu v C#

```csharp
using System;

class Program
{
    // Implementace Insertion Sortu
    static void InsertionSort(int[] arr)
    {
        int n = arr.Length;
        
        // Začínáme od druhého prvku (index 1)
        for (int i = 1; i < n; i++)
        {
            int key = arr[i];
            int j = i - 1;
            
            // Posouváme prvky, které jsou větší než key, o jednu pozici doprava
            while (j >= 0 && arr[j] > key)
            {
                arr[j + 1] = arr[j];
                j = j - 1;
            }
            
            // Umístíme key na správnou pozici
            arr[j + 1] = key;
        }
    }

    // Pomocná metoda pro tisk pole
    static void PrintArray(int[] arr)
    {
        foreach (int item in arr)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();
    }

    static void Main()
    {
        int[] arr = { 64, 25, 12, 22, 11 };
        
        Console.WriteLine("Původní pole:");
        PrintArray(arr);
        
        InsertionSort(arr);
        
        Console.WriteLine("Seřazené pole:");
        PrintArray(arr);
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


            index = (index + 1) % (pole.Length - 1);
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