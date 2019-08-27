using System;
using System.Collections.Generic;

namespace ExtensionMethods
{
    public static class Extensions
    {
        public static bool All<TSource>(
            this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            ThrowNullException(source);

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
            ThrowNullException(source);

            foreach (var element in source)
            {
                if (predicate(element))
                {
                    return true;
                }
            }

            return false;
        }

        public static TSource First<TSource>(
            this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            ThrowNullException(source);

            foreach (var element in source)
            {
                if (predicate(element))
                {
                    return element;
                }
            }

            throw new InvalidOperationException(message: "No IEnumerable<TSource> element satisfies delegate condition!/t");
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(
            this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            ThrowNullException(source);
            ThrowNullSelectorException(selector);

            foreach (var item in source)
            {
                yield return selector(item);
            }
        }

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TResult>> selector)
        {
            ThrowNullException(source);
            ThrowNullSelectorException(selector);

            foreach (var item in source)
            {
                foreach (var element in selector(item))
                {
                    yield return element;
                }
            }
        }

        private static void ThrowNullSelectorException<TSource, TResult>(
            Func<TSource, TResult> selector)
        {
            if (selector != null)
            {
                return;
            }

            throw new ArgumentNullException(paramName: nameof(selector), message: "Selector cannot be null!/t");
        }

        private static void ThrowNullException<TSource>(IEnumerable<TSource> source)
        {
            if (source != null)
            {
                return;
            }

            throw new ArgumentNullException(paramName: nameof(source), message: "Object cannot be null!/t");
        }
    }
}