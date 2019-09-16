using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionMethods
{
    internal class SortedSequence<TSource, TExtendedKey> : IOrderedEnumerable<TSource>
    {
        private readonly IComparer<TExtendedKey> extendedComparer;
        private readonly Func<TSource, TExtendedKey> extendedSelector;
        private readonly IEnumerable<TSource> unsortedEnumerable;

        public SortedSequence(
            IEnumerable<TSource> enumerable,
            Func<TSource, TExtendedKey> extendedSelector,
            IComparer<TExtendedKey> comparer)
        {
            extendedComparer = comparer;
            this.extendedSelector = extendedSelector;
            unsortedEnumerable = enumerable;
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

            Func<TSource, TExtendedKey> primarySelector = extendedSelector;
            ExtendedKey<TExtendedKey, TKey> NewSelector(TSource source)
                    => new ExtendedKey<TExtendedKey, TKey>(
                    primarySelector(source),
                    keySelector(source));

            IComparer<ExtendedKey<TExtendedKey, TKey>> newComparer =
                new ExtendedKey<TExtendedKey, TKey>.MyComparer(extendedComparer, comparer);

            return new SortedSequence<TSource, ExtendedKey<TExtendedKey, TKey>>(
                unsortedEnumerable, NewSelector, newComparer);
        }

        public IEnumerator<TSource> GetEnumerator()
        {
            var list = unsortedEnumerable.ToList();
            while (list.Count > 0)
            {
                TSource minElement = list[0];

                for (int i = 1; i < list.Count; i++)
                {
                    if (extendedComparer.Compare(extendedSelector(list[i]), extendedSelector(minElement)) < 0)
                    {
                        minElement = list[i];
                    }
                }

                yield return minElement;
                list.Remove(minElement);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal struct ExtendedKey<TFirst, TSecond>
        {
            internal ExtendedKey(TFirst first, TSecond second)
            {
                First = first;
                Second = second;
            }

            public TFirst First { get; }

            public TSecond Second { get; }

            internal class MyComparer : IComparer<ExtendedKey<TFirst, TSecond>>
            {
                private readonly IComparer<TFirst> primaryComparer;
                private readonly IComparer<TSecond> secondaryComparer;

                internal MyComparer(
                    IComparer<TFirst> primaryComparer,
                    IComparer<TSecond> secondaryComparer)
                {
                    this.primaryComparer = primaryComparer;
                    this.secondaryComparer = secondaryComparer;
                }

                public int Compare(
                    ExtendedKey<TFirst, TSecond> firstValue,
                    ExtendedKey<TFirst, TSecond> secondValue)
                {
                    int primaryResult = primaryComparer.Compare(firstValue.First, secondValue.First);
                    if (primaryResult != 0)
                    {
                        return primaryResult;
                    }

                    return secondaryComparer.Compare(firstValue.Second, secondValue.Second);
                }
            }
        }
    }
}