# Lomuto Partitioning v Quick Sortu (C#)

## Co je Lomuto partitioning

**Lomutova metoda rozdělení** je způsob, jak rozdělit pole podle jednoho prvku – tzv. *pivotu*.  
Pivot určuje místo, kde se pole rozdělí na dvě části:

- nalevo od pivotu budou všechny hodnoty **menší nebo rovné pivotu**,  
- napravo budou všechny **větší hodnoty**.

Tento princip tvoří základ algoritmu **Quick Sort**, který díky tomu pole postupně třídí.

---

## 🔢 Jak Lomuto partitioning funguje

Představ si, že máš řadu čísel a na konci vybereš jeden prvek jako **pivot**.  
Cílem je uspořádat pole tak, aby vlevo od pivotu byla všechna čísla **menší nebo stejná** a vpravo všechna **větší**.

Abychom to dokázali, používáme dva ukazatele (indexy):

- **`i`** – ukazuje na **první pozici, kam můžeme vložit další menší číslo** než pivot.  
  Na začátku je tedy nastaven na `low` (začátek části pole, kterou třídíme).
- **`j`** – ukazuje na **aktuální prvek, který právě zkoumáme**.  
  Postupně prochází celé pole od `low` až po prvek těsně před pivotem (`high - 1`).

---

### 🔄 Průběh algoritmu krok za krokem

1. Vybereme **pivot** – poslední prvek části pole (`arr[high]`).  
2. Otestujeme prvek s indexem `j`:  
   - Pokud je `arr[j]` **menší nebo stejný** než pivot:  
     1. Prohodíme `arr[i]` a `arr[j]` – tím přesuneme menší číslo nalevo.  
     2. Posuneme `i` o 1 doprava, protože jsme rozšířili oblast menších čísel.  
   - Pokud je `arr[j]` **větší** než pivot, nic neděláme.  
3. Zvýšíme index `j` o 1, abychom zkontrolovali další prvek, a pokračujeme bodem 2, dokud neprojdeme všechny prvky mimo pivot.  
4. Nakonec pivot vyměníme s prvkem na pozici `i`.  
   Tím se pivot přesune na své **správné místo** – mezi levou a pravou část.

---

### 🧩 Co tím získáme

- Všechny prvky **vlevo od pivotu** jsou menší nebo rovné pivotu.  
- Všechny **vpravo** jsou větší než pivot.  
- Index `i` (kam jsme pivot přesunuli) je nová **hranice rozdělení** pole.

---

## 💻 Ukázkový kód v C#

```csharp
using System;

class QuickSortExample
{
    // Upravený Lomuto partitioning – i začíná na "low"
    static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high];
        int i = low; // první pozice pro menší číslo

        for (int j = low; j < high; j++)
        {
            if (arr[j] <= pivot)
            {
                Swap(arr, i, j);
                i++; // posun po vložení menšího čísla
            }
        }

        // Umístění pivotu na správné místo
        Swap(arr, i, high);
        return i;
    }
}
```

## Zadání

S pomocí kódu stránky níže implementuje algoritmus Lomuto Partitioning, tak aby se na stisk tlačítka provedla jen **jedna iterace** a mohli jsme interaktivně sledovat průběh algoritmu.

Výchozí kód stránky:

```csharp
@page "/lomuto"

<PageTitle>Lomuto Partitioning</PageTitle>

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

@code {
  
    int low;
    int high;
    int pivot;
    int j;
    int i;

    private int[] pole = ReservoarSampling.Generate(9, 9);

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
        if (j < high)
        {
           
            // 💻

            ++j;
        }
        else
        {
            // 💻

            Inicializace();
        }
    }
}
```
