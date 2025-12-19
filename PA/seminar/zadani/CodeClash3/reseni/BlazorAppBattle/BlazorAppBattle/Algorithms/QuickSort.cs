using System.Collections.Concurrent;

namespace BlazorAppBattle.Algorithms
{
    public class QuickSort
    {
        public struct Partition(int low, int high)
        {
            public int low = low;
            public int high = high;
        }

        public int Rank { get; set; }
        public int[] Array { get; private set; }
        public bool IsSorted { get; private set; }
        public int CurrentIndex { get; private set; }
        public int SmallerElementIndex { get; private set; }
        public int Low { get; private set; }
        public int High { get; private set; }
        public int Pivot { get; private set; }
        public Queue<Partition> Partitions { get; private set; } = [];

        public QuickSort(int[] array)
        {
            Array = array;
            IsSorted = false;

            if (Array.Length < 2)
            {
                IsSorted = true;

                return;
            }

            Low = 0;
            High = Array.Length - 1;

            CurrentIndex = 0;
            SmallerElementIndex = 0;

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
                    Tools.Swap(Array, SmallerElementIndex, CurrentIndex);

                    ++SmallerElementIndex;
                }

                ++CurrentIndex;
            }
            else
            {
                Tools.Swap(Array, SmallerElementIndex, CurrentIndex);

                Partition left = new(Low, SmallerElementIndex - 1);
                Partition right = new(SmallerElementIndex + 1, High);

                if (left.low < left.high)
                {
                    Partitions.Enqueue(left);
                }

                if (right.low < right.high)
                {
                    Partitions.Enqueue(right);
                }

                if (Partitions.Count > 0)
                {
                    Partition partition = Partitions.Dequeue();

                    Low = partition.low;
                    High = partition.high;
                    Pivot = Array[High];
                    SmallerElementIndex = Low;
                    CurrentIndex = Low;
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
