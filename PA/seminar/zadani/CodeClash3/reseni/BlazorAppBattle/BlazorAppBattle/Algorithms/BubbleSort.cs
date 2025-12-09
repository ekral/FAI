namespace BlazorAppBattle.Algorithms
{
    public class BubbleSort
    {
        public int[] Array { get; private set; }
        public bool IsSorted { get; private set; }
        public int Rank { get; set; }
        public int Index { get; private set; }
        public int SortedCount { get; private set; }

        private bool swapped;

        public BubbleSort(int[] array)
        {
            Array = array;
            IsSorted = false;
            Rank = 0;
            Index = 0;
            SortedCount = 0;
            swapped = false;

            if(Array.Length < 2)
            {
                IsSorted = true;
            }
        }

        public void NextIteration()
        {
            if (IsSorted)
            {
                return;
            }

            if (Array[Index] > Array[Index + 1])
            {
                Tools.Swap(Array, Index, Index + 1);
                
                swapped = true;
            }

            if(Index < Array.Length - 2 - SortedCount)
            {
                ++Index;
            }
            else
            {
                if (!swapped || SortedCount >= Array.Length - 1)
                {
                    IsSorted = true;

                    return;
                }

                ++SortedCount;

                swapped = false;

                Index = 0;
            }
        }
    }
}
