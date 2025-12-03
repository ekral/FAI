namespace BlazorAppBattle.Algorithms
{
    public class SelectionSort
    {
        public int Rank { get; set; }
        public int[] Array { get; private set; }
        public int CurrentIndex { get; private set; }
        public int ElementIndex { get; private set; }
        public bool IsSorted { get; private set; }

        private int minIndex;

        public SelectionSort(int[] array)
        {
            Array = array;
            CurrentIndex = 0;
            minIndex = 0;
            ElementIndex = 1;
            IsSorted = false;

            if (Array.Length < 2)
            {
                IsSorted = true;
            }
        }

        public void NextIteration()
        {
            if (IsSorted)
                return;

            if (ElementIndex < Array.Length)
            {
                if(Array[ElementIndex] < Array[minIndex])
                {
                    minIndex = ElementIndex;
                }

                ++ElementIndex;
            }
            else
            {
                Tools.Swap(Array, CurrentIndex, minIndex);

                ++CurrentIndex;
                
                if(CurrentIndex >  Array.Length - 2)
                {
                    IsSorted = true;
                    return;
                }

                minIndex = CurrentIndex;
                ElementIndex = CurrentIndex + 1;
            }

        }
    }
}
