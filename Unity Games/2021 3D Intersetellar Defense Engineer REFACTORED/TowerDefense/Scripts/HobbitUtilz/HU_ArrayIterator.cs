namespace HobbitUtilz
{
    /// <summary>
    /// Iterator for an array as a circular list. Prevents out of bound exceptions. Implicitly casts to int.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HU_ArrayIterator<T>
    {
        readonly T[] _ARRAY;
        
        public int Index;

        public HU_ArrayIterator(T[] arrayIn) { _ARRAY = arrayIn; }

        public static implicit operator int(HU_ArrayIterator<T> iterator) { return iterator?.Index ?? 0; }

        /// <summary>
        /// Safely iterates over list by a desired number of positions. 
        /// </summary>
        /// <param name="numberOfPositions"></param>
        /// <returns> Returns element at adjusted index.</returns>
        public T Advance(int numberOfPositions = 1)
        {
            //if positive
            if (numberOfPositions > -1)
            {
                Index += numberOfPositions;
                //reduce index by length until it is within bounds.
                while (Index >= _ARRAY.Length) { Index -= _ARRAY.Length; }
            }
            //if negative
            else
            {
                Index += numberOfPositions;
                //increase index by length until it is within bounds.
                while (Index < 0) { Index += _ARRAY.Length; }
            }
            return _ARRAY[Index];
        }

        /// <summary>
        /// Returns index as if advanced by number of positions without actually advancing the index.
        /// </summary>
        /// <param name="numberOfPositions"></param>
        /// <returns></returns>
        public int AdjustedIndex(int numberOfPositions)
        {
            int tempIndex = Index;
            //if positive
            if (numberOfPositions > -1)
            {
                tempIndex += numberOfPositions;
                //reduce index by length until it is within bounds.
                while (tempIndex >= _ARRAY.Length) { tempIndex -= _ARRAY.Length; }
            }
            //if negative
            else
            {
                tempIndex += numberOfPositions;
                //increase index by length until it is within bounds.
                while (tempIndex < 0) { tempIndex += _ARRAY.Length; }
            }
            return tempIndex;
        }

        /// <summary>
        /// Get element next in the list without advancing the iterator.
        /// </summary>
        /// <returns></returns>
        public T PeakNext()
        {
            int tempIndex = Index;
            if (++tempIndex >= _ARRAY.Length) { tempIndex = 0; }
            return _ARRAY[tempIndex];
        }

        /// <summary>
        /// Get element previous in the list without advancing the iterator.
        /// </summary>
        /// <returns></returns>
        public T PeakPrevious()
        {
            int tempIndex = Index;
            if (--tempIndex < 0) { tempIndex = _ARRAY.Length - 1; }
            return _ARRAY[tempIndex];
        }
    }
}
