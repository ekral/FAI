namespace BlazorAppBattle.Algorithms
{
    public class SelectionSort
    {
        public int Rank { get; set; }
        public int[] Array { get; private set; }
        public bool IsSorted { get; private set; }
        public int CurrentIndex { get; private set; }
        public int ElementIndex { get; private set; }
        public int MinIndex { get; private set; }

        public SelectionSort(int[] array)
        {
            Rank = 0;
            Array = array;
            IsSorted = false;
            CurrentIndex = 0;
            MinIndex = 0;
            ElementIndex = 1;

            if (Array.Length < 2)
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
         
            if (ElementIndex < Array.Length - 1)
            {
                ++ElementIndex;
            }
            else
            {
                Tools.Swap(Array, CurrentIndex, MinIndex);

                if (CurrentIndex >= Array.Length - 2)
                {
                    IsSorted = true;

                    return;
                }

                ++CurrentIndex;

                MinIndex = CurrentIndex;
                ElementIndex = CurrentIndex + 1;
            }

            if (Array[ElementIndex] < Array[MinIndex])
            {
                MinIndex = ElementIndex;
            }
        }
    }
}
