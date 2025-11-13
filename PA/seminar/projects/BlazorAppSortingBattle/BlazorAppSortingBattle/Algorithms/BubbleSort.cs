namespace BlazorAppSortingBattle.Algorithms
{
    /// <summary>
    /// Implements the Bubble Sort algorithm in an iterative, step-by-step manner. 
    /// Each call to NextIteration performs a single comparison and potential swap.
    /// It includes the optimization to exit early if a full pass occurs without any swaps.
    /// </summary>
    public class BubbleSort
    {
        /// <summary>
        /// Gets a value indicating whether the array is fully sorted.
        /// </summary>
        public bool IsSorted { get; private set; }

        /// <summary>
        /// Gets the index of the comparison being performed in the current pass.
        /// </summary>
        public int CurrentIndex { get; private set; }

        /// <summary>
        /// Gets the number of elements at the end of the array that are already in their final sorted position.
        /// This reduces the size of the array to check in subsequent passes.
        /// </summary>
        public int SortedCount { get; private set; }

        /// <summary>
        /// Tracks whether any swap occurred during the current full pass through the unsorted portion of the array.
        /// Used for the Bubble Sort optimization (early exit).
        /// </summary>
        private bool swappedInPass;

        public BubbleSort()
        {
            // Initialize the state when the object is created.
            Reset();
        }

        /// <summary>
        /// Resets the internal state to begin sorting a new or modified array from scratch.
        /// </summary>
        public void Reset()
        {
            CurrentIndex = 0;
            SortedCount = 0;
            IsSorted = false;
            swappedInPass = false;
        }

        /// <summary>
        /// Performs one step (one comparison and potential swap) of the bubble sort algorithm on the array.
        /// </summary>
        /// <param name="array">The array to perform one iteration on.</param>
        /// <returns>True if more iterations are required; False if the array is fully sorted.</returns>
        public bool NextIteration(int[] array)
        {
            // 1. Check if sorting is already complete (from a previous call).
            if (IsSorted)
            {
                return false;
            }

            // 2. Handle trivial case: array with less than 2 elements is always sorted.
            if (array.Length < 2)
            {
                IsSorted = true;
                return false;
            }

            // 3. Termination Check (Worst-Case Guarantee): 
            // Check if the maximum number of passes required (array.Length - 1) has been completed.
            if (SortedCount >= array.Length - 1)
            {
                IsSorted = true;
                return false;
            }

            // 4. Check if the current pass (loop) has finished.
            // The comparison index has moved past the last element in the unsorted section.
            if (CurrentIndex > array.Length - 2 - SortedCount)
            {
                // --- Early Exit Optimization Check (The second termination point) ---
                // If no swaps occurred during the *just completed* pass, the array is sorted.
                if (!swappedInPass)
                {
                    IsSorted = true;
                    return false;
                }

                // Start a new pass:
                CurrentIndex = 0;        // Reset index to start of the unsorted portion.
                ++SortedCount;           // Increment count of elements in their final place.
                swappedInPass = false;   // Reset swap flag for the new pass.
            }

            // 5. Perform the single comparison/swap step.
            if (array[CurrentIndex] > array[CurrentIndex + 1])
            {
                // The Tools.Swap is an assumed utility method.
                Tools.Swap(array, CurrentIndex, CurrentIndex + 1);

                // Mark that a swap occurred in this pass.
                swappedInPass = true;
            }

            // 6. Advance the current index for the next iteration call.
            ++CurrentIndex;

            // 7. Return true to signal that a step was executed. 
            // The sort completion check (if needed) will occur on the *next* call to NextIteration (in step 3 or 4).
            return true;
        }
    }
}
