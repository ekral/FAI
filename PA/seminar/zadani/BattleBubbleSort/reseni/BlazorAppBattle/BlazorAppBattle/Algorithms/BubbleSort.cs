namespace BlazorAppBattle.Algorithms
{
    public class BubbleSort
    {
        public int[] Array { get; private set; }

        public int Index { get; private set; }

        public BubbleSort(int[] array)
        {
            Array = array;
            Index = 0;
        }

        public void NextIteration()
        {
            if (Array[Index] > Array[Index + 1])
            {
                Tools.Swap(Array, Index, Index + 1);
            }

            ++Index;
        }
    }
}
