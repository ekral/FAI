# Algoritmus Quick Sort s Lomuto Partitioningem

## 🔢 Základní princip

Algoritmus **Quick Sort** je efektivní třídicí algoritmus, který používá princip "rozděl a panuj". Rozděluje pole kolem vybraného **pivotu** (obvykle poslední prvek v poli) a tímto způsobem zajišťuje, že:

- **Levá část** obsahuje hodnoty menší než pivot.
- **Pravá část** obsahuje hodnoty větší než pivot.

Po každém rozdělení je pivot umístěn na své správné místo mezi levé a pravé podpole. Tento proces se rekurzivně opakuje pro každou část (subarray), dokud není celé pole seřazeno.

## 🔄 Průběh algoritmu

1. **Partitioning (rozdělení)**:
   - Vybereme pivot, například poslední prvek pole.
   - Projdeme celé pole a všechny prvky, které jsou menší než pivot, umístíme na levou stranu.
   - Po dokončení průchodu umístíme pivot na jeho správné místo mezi levou a pravou část.

2. **Rekurzivní volání**:
   - Po rozdělení pole na levý a pravý podseznam zavoláme **QuickSort** rekurzivně pro každou z těchto částí.

3. Tento proces pokračuje, dokud všechny části pole nebudou mít pouze jeden prvek, což znamená, že je již seřazeno.

---

## 💻 Ukázkový kód v C#

```csharp
using System;

class Program
{
    // Funkce pro partitioning podle Lomuto algoritmu
    static int LomutoPartition(int[] arr, int low, int high)
    {
        // Pivot je poslední prvek pole
        int pivot = arr[high];
        int i = low - 1;  // Index pro menší prvek

        // Procházíme celé pole a přehazujeme menší hodnoty na levou stranu
        for (int j = low; j < high; j++)
        {
            if (arr[j] < pivot) // Pokud je prvek menší než pivot
            {
                i++;  // Posuneme index pro menší prvek
                Swap(ref arr[i], ref arr[j]); // Prohodíme prvek na pozici i a j
            }
        }

        // Umístíme pivot na správné místo
        Swap(ref arr[i + 1], ref arr[high]);

        // Vracíme index pivotu
        return i + 1;
    }

    // Funkce pro výměnu dvou hodnot
    static void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }

    // Funkce pro QuickSort
    static void QuickSort(int[] arr, int low, int high)
    {
        if (low < high)
        {
            // Získáme index pivotu
            int pivotIndex = LomutoPartition(arr, low, high);

            // Rekurzivní volání pro levý a pravý podseznam
            QuickSort(arr, low, pivotIndex - 1);  // Levá část (menší než pivot)
            QuickSort(arr, pivotIndex + 1, high); // Pravá část (větší než pivot)
        }
    }

    // Funkce pro tisk pole
    static void PrintArray(int[] arr)
    {
        foreach (var num in arr)
        {
            Console.Write(num + " ");
        }
        Console.WriteLine();
    }

    static void Main(string[] args)
    {
        int[] arr = { 10, 7, 8, 9, 1, 5 };
        
        Console.WriteLine("Původní pole:");
        PrintArray(arr);

        // Spustíme QuickSort
        QuickSort(arr, 0, arr.Length - 1);

        Console.WriteLine("Seřazené pole:");
        PrintArray(arr);
    }
}
```

## 🚀 Zadání

S pomocí kódu stránky níže implementuje algoritmus QuickSort, tak aby se na stisk tlačítka provedla jen **jedna iterace** a mohli jsme interaktivně sledovat průběh algoritmu. Místo rekurze použijte třídu `Queue<T>`.

Výchozí kód stránky:

```csharp
@page "/quick"

<PageTitle>Quick Sort</PageTitle>

<div class="d-flex gap-3 mb-3">
    @for (int i = 0; i < pole.Length; i++)
    {
        string textColor = (i >= low && i <= high) ? "text-primary" : "text-secondary";

        if (i == this.i)
        {
            <div class="fs-1 @textColor border-bottom border-primary border-3">@pole[i]</div>
        }
        else if (i == this.j)
        {
            <div class="fs-1 @textColor border-bottom border-secondary border-3">@pole[i]</div>
        }
        else
        {
            <div class="fs-1 @textColor">@pole[i]</div>
        }
    }
</div>

<div class="fs-1 mb-3">pivot @pivot</div>

<button class="btn btn-primary" @onclick="DalsiIterace">Next Iteration</button>

<div class="fs-1 mb-3">
    @foreach(var partition in partitions)
    {
        <div class="fs-1">@partition.low : @partition.high</div>
    }
</div>

@code {
    class Partition
    {
        public int low;
        public int high;

        public Partition(int low, int high)
        {
            this.low = low;
            this.high = high;
        }
    }

    Queue<Partition> partitions = [];

    int low;
    int high;
    int pivot;
    int j;
    int i;

    private int[] pole = ReservoarSampling.Generate(20, 20);

    protected override void OnInitialized()
    {
        Inicializace();
    }

    void Inicializace()
    {
        low = 0;
        high = pole.Length - 1;
        pivot = pole[high];
        i = 0;
        j = 0;
    }

    void DalsiIterace()
    {
        // Lomuto partitioning

        if (j <= high - 1)
        {
            if (pole[j] <= pivot)
            {
                int tmp = pole[i];
                pole[i] = pole[j];
                pole[j] = tmp;

                ++i;
            }

            ++j;
        }
        else
        {
            int tmp = pole[i];
            pole[i] = pole[j];
            pole[j] = tmp;

            // 💻 Vytvorit dve nove partition a pridat je do fronty

            if (partitions.Count > 0)
            {
                // 💻 Vybrat z fronty prvni partition a nastavit podle ni hodnoty
            }
            else
            {
                Inicializace();
            }

        }
    }

   public static class ReservoarSampling
   {
       public static int[] Generate(int n, int max)
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
}
```
