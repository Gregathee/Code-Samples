using System.Collections.Generic;

namespace HobbitUtilz
{
    /// <summary>
    /// Iterator for a list as a circular list. Prevents out of bound exceptions. Implicitly casts to int.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HU_ListIterator<T>
    {
        readonly List<T> _LIST;
        
        public int Index;

        public HU_ListIterator(List<T> listIn) { _LIST = listIn; }

        public static implicit operator int(HU_ListIterator<T> iterator) => iterator.Index;

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
                //reduce index by count until its in bounds.
                while (Index >= _LIST.Count) { Index -= _LIST.Count; }
            }
            //if negative
            else
            {
                Index += numberOfPositions;
                //increase index by count until it is in bounds.
                while (Index < 0) { Index += _LIST.Count; }
            }
            return _LIST[Index];
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
                while (tempIndex >= _LIST.Count) { tempIndex -= _LIST.Count; }
            }
            //if negative
            else
            {
                tempIndex += numberOfPositions;
                //increase index by length until it is within bounds.
                while (tempIndex < 0) { tempIndex += _LIST.Count; }
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
            if (++tempIndex >= _LIST.Count) { tempIndex = 0; }
            return _LIST[tempIndex];
        }

        /// <summary>
        /// Get element previous in the list without advancing the iterator.
        /// </summary>
        /// <returns></returns>
        public T PeakPrevious()
        {
            int tempIndex = Index;
            if (--tempIndex < 0) { tempIndex = _LIST.Count - 1; }
            return _LIST[tempIndex];
        }
    }
}
