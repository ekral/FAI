using System.ComponentModel.Design;

namespace BlazorAppBattle.Algorithms
{
    public class InsertionSort
    {
        public int Rank { get; set; }
        public int[] Array { get; private set; }
        public bool IsSorted { get; private set; }
        public int CurrentIndex { get; private set; }
        public int PlacementIndex { get; private set; }
        public int Number { get; private set; }

        public InsertionSort(int[] array)
        {
            Rank = 0;
            Array = array;
            IsSorted = false;
            
            if (Array.Length < 2)
            {
                IsSorted = true;

                return;
            }

            CurrentIndex = 1;
            PlacementIndex = 1;
            Number = Array[1];
        }

        public void NextIteration()
        {
            if (IsSorted)
            {
                return;
            }

            if(PlacementIndex > 0 && Number < Array[PlacementIndex - 1])
            {
                Array[PlacementIndex] = Array[PlacementIndex - 1];

                --PlacementIndex;
            }
            else
            {
                Array[PlacementIndex] = Number;

                if(CurrentIndex == Array.Length - 1)
                {
                    IsSorted = true;
                    
                    return;
                }
                else
                {
                    ++CurrentIndex;

                    PlacementIndex = CurrentIndex;
                    Number = Array[CurrentIndex];
                }
            }
           
        }
    }
}
