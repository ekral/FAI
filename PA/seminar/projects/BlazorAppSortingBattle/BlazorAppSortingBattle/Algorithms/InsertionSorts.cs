using System;
using System.Reflection;

namespace BlazorAppSortingBattle.Algorithms
{
    public class InsertionSort
    {
        public int[] Array { get; private set; }

        public bool IsSorted { get; private set; }

        public int CurrentIndex { get; private set; }

        public int ElementPosition { get; private set; }

        public int Element { get; private set; }

        public InsertionSort(int[] array)
        {
            Reset(array);
        }

        public void Reset(int[] array)
        {
            Array = array;

            if (Array.Length < 2)
            {
                IsSorted = true;

                return;
            }

            CurrentIndex = 1;
            ElementPosition = CurrentIndex;
            Element = Array[ElementPosition];
        }

        public bool NextIteration()
        {
            if (IsSorted)
            {
                return false;
            }

            // Shift element to the right
            if ((ElementPosition > 0) && Element < Array[ElementPosition - 1])
            {
                Array[ElementPosition] = Array[ElementPosition - 1];
                ElementPosition--;
            }
            else
            {
                // Place the element in its correct position
                Array[ElementPosition] = Element;

                ++CurrentIndex;

                if (CurrentIndex >= Array.Length)
                {
                    CurrentIndex = 1;
                    ElementPosition = 1;

                    IsSorted = true;

                    return false;
                }

                ElementPosition = CurrentIndex;
                Element = Array[ElementPosition];
            }

            return true;
        }
    }
}
