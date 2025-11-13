using System;

namespace BlazorAppSortingBattle.Algorithms
{
    public static class Tools
    {
        public static void Swap(int[] array, int i, int j)
        {
            (array[i], array[j]) = (array[j], array[i]);
        }

        public static int[] GenerateNumbers(int n, int max)
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


    public class BubbleSort
    {
        public bool IsSorted { get; private set; }
        public int CurrentIndex { get; private set; }
        public int SortedCount { get; private set; }
        
        public BubbleSort() 
        {
            Reset();
        }

        public void Reset()
        {
            CurrentIndex = 0;
            SortedCount = 0;
            IsSorted = false;
        }

        /// <summary>
        /// Performs one step of bubble sort on the array.
        /// </summary>
        /// <param name="array">The array to perform one iteration on.</param>
        /// <returns>True if the sorting is complete; otherwise, false.</returns>
        public void NextIteration(int[] array)
        {
            if(IsSorted)
                return;

            if(array.Length < 2 || SortedCount >= array.Length - 1)
            {
                IsSorted = true;

                return;
            }

            if (CurrentIndex > array.Length - 2 - SortedCount)
            {
                CurrentIndex = 0;
                ++SortedCount;
            }

            if (array[CurrentIndex] > array[CurrentIndex + 1])
            {
                Tools.Swap(array, CurrentIndex, CurrentIndex + 1);
            }

            ++CurrentIndex;
        }
    }
}
