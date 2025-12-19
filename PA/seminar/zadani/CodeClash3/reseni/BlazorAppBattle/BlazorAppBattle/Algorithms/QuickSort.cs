using System.Collections.Concurrent;

namespace BlazorAppBattle.Algorithms
{
    public class QuickSort
    {
        public struct Partition(int low, int high)
        {
            public int Low = low;
            public int High = high;
        }

        public int Rank { get; set; }
        public int[] Array { get; private set; }
        public bool IsSorted { get; private set; }
        public int CurrentIndex { get; private set; }
        public int PlacementIndex { get; private set; }
        public int Low { get; private set; }
        public int High { get; private set; }
        public int Pivot { get; private set; }
        public Queue<Partition> Partitions { get; private set; } = [];

        public QuickSort(int[] array)
        {
            Rank = 0;
            Array = array;
            IsSorted = false;
            CurrentIndex = 0;
            PlacementIndex = 0;
            Low = 0;

            if (Array.Length < 2)
            {
                IsSorted = true;

                return;
            }

            High = Array.Length - 1;
            Pivot = Array[High];
        }

        public void NextIteration()
        {
            if (IsSorted)
            {
                return;
            }

            if(CurrentIndex < High)
            {
                if (Array[CurrentIndex] <= Pivot)
                {
                    Tools.Swap(Array, CurrentIndex, PlacementIndex);

                    ++PlacementIndex;
                }

                ++CurrentIndex;
            }
            else
            {
                Tools.Swap(Array, CurrentIndex, PlacementIndex);

                if(Low < PlacementIndex - 1)
                {
                    Partition left = new(Low, PlacementIndex - 1);

                    Partitions.Enqueue(left);
                }

                if (PlacementIndex + 1 < High)
                {
                    Partition right = new(PlacementIndex + 1, High);

                    Partitions.Enqueue(right);
                }

                if(Partitions.Count > 0)
                {
                    Partition partition = Partitions.Dequeue();

                    Low = partition.Low;
                    High = partition.High;
                    CurrentIndex = Low;
                    PlacementIndex = Low;
                    Pivot = Array[High];
                }
                else
                {
                    IsSorted = true;

                    return;
                }

            }

        }
    }
}
