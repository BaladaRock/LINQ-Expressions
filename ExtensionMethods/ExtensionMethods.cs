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
            ThrowNUllAggregateFunction(func);

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

            var set = new HashSet<TSource>();
            foreach (var item in source)
            {
                set.Add(item);
            }

            foreach (var item in set)
            {
                yield return item;
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
                    if (subItem.Equals(item))
                    {
                        yield return resultSelector(item, subItem);
                    }
                }
            }
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

        private static void ThrowNUllAggregateFunction<TSource, TAccumulate>(Func<TAccumulate, TSource, TAccumulate> func)
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