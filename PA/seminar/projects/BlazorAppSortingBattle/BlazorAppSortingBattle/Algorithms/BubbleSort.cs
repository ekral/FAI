namespace BlazorAppSortingBattle.Algorithms
{
    public class BubbleSort
    {
        public int[] Array { get; private set; }

        public bool IsSorted { get; private set; }

        public int CurrentIndex { get; private set; }

        public int SortedCount { get; private set; }

        private bool swappedInPass;

        public BubbleSort()
        {
            Reset([]);
        }

        public void Reset(int[] array)
        {
            Array = array;
            CurrentIndex = 0;
            SortedCount = 0;
            IsSorted = false;
            swappedInPass = false;

            if (Array.Length < 2)
            {
                IsSorted = true;
            }
        }

        public bool NextIteration()
        {
            if (IsSorted)
            {
                return false;
            }

            if (Array[CurrentIndex] > Array[CurrentIndex + 1])
            {
                Tools.Swap(Array, CurrentIndex, CurrentIndex + 1);

                swappedInPass = true;
            }

            ++CurrentIndex;

            if (CurrentIndex > Array.Length - 2 - SortedCount)
            {
                CurrentIndex = 0;

                if (!swappedInPass)
                {
                    IsSorted = true;

                    return false;
                }

                ++SortedCount;

                if (SortedCount >= Array.Length - 1)
                {
                    IsSorted = true;

                    return false;
                }
   
                swappedInPass = false;
            }

            return true;
        }
    }
}
