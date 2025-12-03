namespace BlazorAppBattle.Algorithms
{
    public class BubbleSort
    {
        public int Rank { get; set; }

        public int[] Array { get; private set; }

        public int Index { get; private set; }

        public int SortedCount { get; private set; }
        
        public bool IsSorted { get; private set; }

        private bool swapped;

        public BubbleSort(int[] array)
        {
            Rank = 0;
            Array = array;
            Index = 0;
            SortedCount = 0;
            IsSorted = false;
            swapped = false;

            if(Array.Length < 2)
            {
                IsSorted = true;
            }
        }

        public void NextIteration()
        {
            if (IsSorted)
                return;

            if (Array[Index] > Array[Index + 1])
            {
                Tools.Swap(Array, Index, Index + 1);
                
                swapped = true;
            }

            ++Index;

            if(Index > Array.Length - 2 - SortedCount)
            {
                Index = 0;
                 
                if(!swapped)
                {
                    IsSorted = true;
                }
                else
                {
                    ++SortedCount;

                    swapped = false;
                }
            }
        }
    }
}
