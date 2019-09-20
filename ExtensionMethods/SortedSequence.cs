using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionMethods
{
    internal class SortedSequence<TSource, TKey> : IOrderedEnumerable<TSource>
    {
        private readonly List<Projection<TSource, TKey>> criteriaList;
        private readonly Projection<TSource, TKey> projection;
        private readonly IEnumerable<TSource> unsortedEnumerable;
        private int criteriaIndex;

        public SortedSequence(
            IEnumerable<TSource> enumerable,
            Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer)
        {
            Comparer = comparer;
            KeySelector = keySelector;
            unsortedEnumerable = enumerable;
            projection = new Projection<TSource, TKey>(KeySelector, Comparer);
            criteriaList = new List<Projection<TSource, TKey>> { projection };
            criteriaIndex = 0;
        }

        public IComparer<TKey> Comparer { get; }

        public Func<TSource, TKey> KeySelector { get; }

        public IOrderedEnumerable<TSource> CreateOrderedEnumerable<TKey>(
            Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer,
            bool descending)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            return this;
        }

        public IEnumerator<TSource> GetEnumerator()
        {
            var elements = unsortedEnumerable.ToList();
            elements = ApplyListCriteria(ref elements);

            foreach (var item in elements)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private List<TSource> ApplyListCriteria(ref List<TSource> elements)
        {
            if (criteriaIndex == 0)
            {
                ApplySortingCriteria(ref elements, criteriaList[0]);
            }
            else
            {
                for (int i = 0; i < elements.Count - 1; i++)
                {
                    while (criteriaList[criteriaIndex - 1].Compare(elements[i], elements[i + 1]) == 0)
                    {
                        ApplySortingCriteria(ref elements, criteriaList[criteriaIndex]);
                        criteriaList[criteriaIndex] = projection;
                    }
                }
            }

            criteriaIndex++;
            return elements;
        }

        private void ApplySortingCriteria(ref List<TSource> list, Projection<TSource, TKey> criteria)
        {
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
                    SwapElements(ref list, i, minimum);
                }
            }
        }

        private void SwapElements(ref List<TSource> list, int oldIndex, int minimumIndex)
        {
            TSource temp = list[oldIndex];
            list[oldIndex] = list[minimumIndex];
            list[minimumIndex] = temp;
        }
    }
}