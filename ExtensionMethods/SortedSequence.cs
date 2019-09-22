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

            var list = unsortedEnumerable.ToList();
            ApplyCriteriaList(ref list);
            return this;
        }

        public IEnumerator<TSource> GetEnumerator()
        {
            var elements = unsortedEnumerable.ToList();
            ApplyCriteriaList(ref elements);

            foreach (var item in elements)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void ApplyCriteriaList(ref List<TSource> elements)
        {
            ApplySortingCriteria(ref elements, criteriaList[0], 0);
            if (criteriaList.Count == 1)
            {
                return;
            }

            for (int i = 1; i < criteriaList.Count; i++)
            {
                for (int j = 0; j < elements.Count - 1; j++)
                {
                    if (criteriaList[i].Compare(elements[j], elements[j + 1]) == 0)
                    {
                        ApplySortingCriteria(ref elements, criteriaList[0], j);
                    }
                }
            }

            criteriaList.Add(projection);
        }

        private void ApplySortingCriteria(ref List<TSource> list, Projection<TSource, TKey> criteria, int index)
        {
            for (int i = index; i < list.Count - 1; i++)
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