using System;
using System.Collections.Generic;

namespace ExtensionMethods
{
    public static class Extensions
    {
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
            ThrowInputExceptions(source, keySelector, elementSelector);

            var dictionary = new Dictionary<TKey, TElement>();
            foreach (var element in source)
            {
                var newElement = new KeyValuePair<TKey, TElement>(keySelector(element), elementSelector(element));
                ThrowKeyExceptions(newElement.Key, dictionary);

                dictionary.Add(newElement.Key, newElement.Value);
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

        private static void ThrowInputExceptions<TSource, TKey, TElement>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            ThrowNullSourceException(source);
            ThrowNullDelegateException(keySelector);
            ThrowNullDelegateException(elementSelector);
        }

        private static void ThrowKeyExceptions<TKey, TElement>(TKey key, Dictionary<TKey, TElement> dictionary)
        {
            ThrowKeyIsNullException(key);

            if (!dictionary.ContainsKey(key))
            {
                return;
            }

            throw new ArgumentException("Could not add key duplicates!/t");
        }

        private static void ThrowKeyIsNullException<TKey>(TKey key)
        {
            if (key != null)
            {
                return;
            }

            throw new ArgumentNullException(paramName: nameof(key), message: "Key cannot be null!/t");
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

        private static void ThrowNullSourceException<TSource>(IEnumerable<TSource> source)
        {
            if (source != null)
            {
                return;
            }

            throw new ArgumentNullException(paramName: nameof(source), message: "Object cannot be null!/t");
        }
    }
}