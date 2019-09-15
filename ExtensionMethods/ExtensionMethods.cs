using System;
using System.Collections.Generic;

namespace ExtensionMethods
{
    public static class Extensions
    {
        public static TAccumulate Aggregate<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> func)
        {
            ThrowNullSourceException(source);
            ThrowNullAggregateFunction(func);

            TAccumulate accumulator = seed;
            foreach (var element in source)
            {
                accumulator = func(accumulator, element);
            }

            return accumulator;
        }

        public static bool All<TSource>(
            this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            ThrowNullSourceException(source);

            foreach (var element in source)
            {
                if (!predicate(element))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Any<TSource>(
            this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            ThrowNullSourceException(source);

            foreach (var element in source)
            {
                if (predicate(element))
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<TSource> Distinct<TSource>(
            this IEnumerable<TSource> source,
            IEqualityComparer<TSource> comparer)
        {
            ThrowNullSourceException(source);

            var set = new HashSet<TSource>(comparer);
            foreach (var item in source)
            {
                if (set.Add(item))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<TSource> Except<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer)
        {
            ThrowNullSourceException(first);
            ThrowNullSourceException(second);

            var secondElements = new HashSet<TSource>(second, comparer);

            foreach (var item in first)
            {
                if (secondElements.Add(item))
                {
                    yield return item;
                }
            }
        }

        public static TSource First<TSource>(
            this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            ThrowNullSourceException(source);

            foreach (var element in source)
            {
                if (predicate(element))
                {
                    return element;
                }
            }

            throw new InvalidOperationException(message: "No IEnumerable<TSource> element satisfies delegate condition!/t");
        }

        public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            Func<TKey, IEnumerable<TElement>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            ThrowGroupByExceptions(source, keySelector, elementSelector, resultSelector);

            var dictionary = new Dictionary<TKey, List<TElement>>(comparer);

            foreach (var item in source)
            {
                var newKey = keySelector(item);
                var newElement = elementSelector(item);

                if (!dictionary.TryAdd(newKey, new List<TElement>() { newElement }))
                {
                    dictionary[newKey].Add(newElement);
                }
            }

            foreach (var key in dictionary.Keys)
            {
                yield return resultSelector(key, dictionary[key]);
            }
        }

        public static IEnumerable<TSource> Intersect<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer)
        {
            ThrowNullSourceException(first);
            ThrowNullSourceException(second);

            var hashSet = new HashSet<TSource>(second, comparer);

            foreach (var item in first)
            {
                if (hashSet.Remove(item))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            ThrowJoinExceptions(outer, inner, outerKeySelector, innerKeySelector, resultSelector);

            foreach (var item in outer)
            {
                foreach (var subItem in inner)
                {
                    if (innerKeySelector(subItem).Equals(outerKeySelector(item)))
                    {
                        yield return resultSelector(item, subItem);
                    }
                }
            }
        }

        public static System.Linq.IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer)
            where TSource : IComparable<TSource>
        {
            return new SortedSequence<TSource, TKey>(source, keySelector, comparer);
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(
            this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            ThrowNullSourceException(source);
            ThrowNullDelegateException(selector);

            foreach (var item in source)
            {
                yield return selector(item);
            }
        }

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TResult>> selector)
        {
            ThrowNullSourceException(source);
            ThrowNullDelegateException(selector);

            foreach (var item in source)
            {
                foreach (var element in selector(item))
                {
                    yield return element;
                }
            }
        }

        public static System.Linq.IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(
            this System.Linq.IOrderedEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer)
        {
            return source.CreateOrderedEnumerable(keySelector, comparer, false);
        }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
        {
            ThrowParamsExceptions(source, keySelector, elementSelector);

            var dictionary = new Dictionary<TKey, TElement>();
            foreach (var element in source)
            {
                dictionary.Add(keySelector(element), elementSelector(element));
            }

            return dictionary;
        }

        public static IEnumerable<TSource> Union<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer)
        {
            ThrowNullSourceException(first);
            ThrowNullSourceException(second);

            var hashSet = new HashSet<TSource>(comparer);
            foreach (var item in first)
            {
                if (hashSet.Add(item))
                {
                    yield return item;
                }
            }

            foreach (var item in second)
            {
                if (hashSet.Add(item))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            ThrowNullSourceException(source);
            ThrowNullDelegateException(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            ThrowNullSourceException(first);
            ThrowNullSourceException(second);

            var firstEnumerator = first.GetEnumerator();
            var secondEnumerator = second.GetEnumerator();

            while (secondEnumerator.MoveNext() && firstEnumerator.MoveNext())
            {
                yield return resultSelector(firstEnumerator.Current, secondEnumerator.Current);
            }
        }

        private static void ThrowGroupByExceptions<TSource, TKey, TElement, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        {
            ThrowNullSourceException(source);
            ThrowNullDelegateException(keySelector);
            ThrowNullDelegateException(elementSelector);
            ThrowNullSelectorException(resultSelector);
        }

        private static void ThrowJoinExceptions<TOuter, TInner, TKey, TResult>(
            IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            ThrowNullSourceException(outer);
            ThrowNullSourceException(inner);
            ThrowNullDelegateException(outerKeySelector);
            ThrowNullDelegateException(innerKeySelector);
            ThrowNullSelectorException(resultSelector);
        }

        private static void ThrowKeyIsNullException<TKey>(TKey key)
        {
            if (key != null)
            {
                return;
            }

            throw new ArgumentNullException(paramName: nameof(key), message: "Key cannot be null!/t");
        }

        private static void ThrowNullAggregateFunction<TSource, TAccumulate>(Func<TAccumulate, TSource, TAccumulate> func)
        {
            if (func != null)
            {
                return;
            }

            throw new ArgumentNullException(paramName: nameof(func), message: "Function cannot be null!/t");
        }

        private static void ThrowNullDelegateException<TSource, TResult>(
            Func<TSource, TResult> selector)
        {
            if (selector != null)
            {
                return;
            }

            throw new ArgumentNullException(paramName: nameof(selector), message: "Selector cannot be null!/t");
        }

        private static void ThrowNullSelectorException<TOuter, TInner, TResult>(Func<TOuter, TInner, TResult> resultSelector)
        {
            if (resultSelector != null)
            {
                return;
            }

            throw new ArgumentNullException(paramName: nameof(resultSelector), message: "ResultSelector cannot be null!/t");
        }

        private static void ThrowNullSourceException<TSource>(IEnumerable<TSource> source)
        {
            if (source != null)
            {
                return;
            }

            throw new ArgumentNullException(paramName: nameof(source), message: "Object cannot be null!/t");
        }

        private static void ThrowParamsExceptions<TSource, TKey, TElement>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            ThrowNullSourceException(source);
            ThrowNullDelegateException(keySelector);
            ThrowNullDelegateException(elementSelector);
        }
    }
}