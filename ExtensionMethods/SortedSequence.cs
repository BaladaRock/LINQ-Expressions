using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionMethods
{
    internal class SortedSequence<TSource, TKey> : IOrderedEnumerable<TSource>
    {
        private readonly IComparer<TKey> comparer;
        private readonly Func<TSource, TKey> keySelector;
        private readonly IEnumerable<TSource> unsortedEnumerable;

        public SortedSequence(
            IEnumerable<TSource> enumerable,
            Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer)
        {
            this.comparer = comparer;
            this.keySelector = keySelector;
            unsortedEnumerable = enumerable;
        }

        public IOrderedEnumerable<TSource> CreateOrderedEnumerable(
            Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer,
            bool descending)
        {
            return new SortedSequence<TSource, TKey>(unsortedEnumerable, keySelector, comparer);
        }

        public IEnumerator<TSource> GetEnumerator()
        {
            var list = unsortedEnumerable.ToList();
            while (list.Count > 0)
            {
                TSource minElement = list[0];
                int minIndex = 0;
                for (int i = 1; i < list.Count; i++)
                {
                    if (comparer.Compare(keySelector(list[i]), keySelector(minElement)) < 0)
                    {
                        minElement = list[i];
                        minIndex = i;
                    }
                }

                list.RemoveAt(minIndex);
                yield return minElement;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}