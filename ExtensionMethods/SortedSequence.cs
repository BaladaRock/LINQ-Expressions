using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionMethods
{
    internal class SortedSequence<TSource, TKey> : IOrderedEnumerable<TSource>
        where TSource : IComparable<TSource>
    {
        // private readonly IComparer<TKey> comparer;
        private readonly Func<TSource, TKey> keySelector;
        private readonly IEnumerable<TSource> unsortedEnumerable;

        public SortedSequence(
            IEnumerable<TSource> enumerable,
            Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer)
        {
            // this.comparer = comparer;
            this.keySelector = keySelector;
            unsortedEnumerable = enumerable;
        }

        public IOrderedEnumerable<TSource> CreateOrderedEnumerable<TKey>(
            Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer,
            bool descending)
        {
            return this;
        }

        public IEnumerator<TSource> GetEnumerator()
        {
            var newList = unsortedEnumerable.Select(x => keySelector(x));
            var list = unsortedEnumerable.ToList();

            list.Sort();
            /*var result = new List<TSource>();

            foreach (var element in list)
            {
                result.Add(element);
            }*/

            foreach (var element in list)
            {
                yield return element;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}