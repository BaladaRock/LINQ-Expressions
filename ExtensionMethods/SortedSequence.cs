using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionMethods
{
    internal class SortedSequence<TSource, TKey> : IOrderedEnumerable<TSource>
    {
        private readonly MultiComparer<TSource> criteria;
        private readonly IEnumerable<TSource> unsortedEnumerable;

        public SortedSequence(
            IEnumerable<TSource> enumerable,
            params IComparer<TSource>[] criteriaList)
        {
            unsortedEnumerable = enumerable;
            criteria = new MultiComparer<TSource>(criteriaList);
        }

        public IOrderedEnumerable<TSource> CreateOrderedEnumerable<TKey>(
            Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer,
            bool descending)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            var newProjection = new Projection<TSource, TKey>(keySelector, comparer);
            return new SortedSequence<TSource, TKey>(
                unsortedEnumerable,
                criteria,
                newProjection);
        }

        public IEnumerator<TSource> GetEnumerator()
        {
            var list = unsortedEnumerable.ToList();
            for (int i = 0; i < list.Count - 1; i++)
            {
                var minimum = i;

                for (int j = i + 1; j < list.Count; j++)
                {
                    if (criteria.Compare(list[j], list[minimum]) < 0)
                    {
                        minimum = j;
                    }
                }

                if (!minimum.Equals(i))
                {
                    SwapElements(list, i, minimum);
                }
            }

            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void SwapElements(List<TSource> list, int oldIndex, int minimumIndex)
        {
            TSource temp = list[oldIndex];
            list[oldIndex] = list[minimumIndex];
            list[minimumIndex] = temp;
        }
    }
}