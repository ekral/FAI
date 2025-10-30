# Insertion Sort v C#

## Popis

**Insertion Sort** (Třídění vkládáním) je jednoduchý algoritmus pro třídění, který postupně vkládá každý prvek na správné místo do již seřazené části seznamu.

## 🔄 Průběh algoritmu krok za krokem

1. Začíná od druhého prvku (první prvek je považován za již seřazený).
2. Tento prvek porovná s předchozími prvky a pokud je menší než některý z nich, posune je o jednu pozici doprava.
3. Poté tento prvek vloží na správné místo.
4. Tento proces se opakuje pro každý další prvek, dokud není celý seznam seřazený.

## Časová složitost:
- **Nejlepší případ:** O(n), pokud je seznam již seřazený.
- **Průměrný a nejhorší případ:** O(n²), kde n je počet prvků v seznamu.
- **Prostorová složitost:** O(1), protože algoritmus třídí seznam na místě.

## 💻 Ukázkový kód v C#

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

## 🚀 Zadání

S pomocí kódu stránky níže implementuje algoritmus Insertion Sort, tak aby se na stisk tlačítka provedla jen **jedna iterace** a mohli jsme interaktivně sledovat průběh algoritmu.

Výchozí kód stránky:

```csharp
@page "/template"

<PageTitle>Insertion Sort</PageTitle>

<div class="d-flex gap-3 mb-3">
    @for (int i = 0; i < pole.Length; i++)
    {
        if (i == index)
        {
            <div class="fs-1 border-bottom border-primary border-3">@pole[i]</div>
        }
        else if (i == j)
        {
            <div class="fs-1 border-bottom border-secondary border-3">@pole[i]</div>
        }
        else
        {
            <div class="fs-1">@pole[i]</div>
        }
    }
</div>

<div class="fs-1 mb-3">prvek @prvek</div>

<button class="btn btn-primary" @onclick="DalsiIterace">Next Iteration</button>

<div class="rz-p-0 rz-p-md-12">
    <RadzenChart>
        <RadzenColumnSeries Data="@Data" CategoryProperty="Caption" ValueProperty="Data">
            <RadzenSeriesDataLabels Visible="true" />
        </RadzenColumnSeries>
        <RadzenLegend Visible="false" />
    </RadzenChart>
</div>

@code {
    DataItem[] Data => pole.Select((x, i) => new DataItem((i == j) ? $">{i + 1}<" : $"{i + 1}", x)).ToArray();

    private int[] pole = ReservoarSampling.Generate(9, 9);

    int index;
    int j;
    int prvek;

    protected override void OnInitialized()
    {
        Inicializace();
    }

    void Inicializace()
    {
        index = 1;
        j = 1;
        prvek = pole[j];
    }

    void DalsiIterace()
    {

        if ((j > 0) /* 💻 test jestli je prvek mensi */)
        {

            // 💻 posun prvku v poli

            j--;
        }
        else
        {
            // 💻 umisteni prvku

            ++index;

            if (index >= pole.Length)
            {
                index = 1;
            }
      
            j = index;
            prvek = pole[j];
        }
    }
}
```
