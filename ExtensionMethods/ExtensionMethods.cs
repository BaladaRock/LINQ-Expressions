using System;
using System.Collections.Generic;

namespace ExtensionMethods
{
    public class ExtensionMethods
    {
        public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);
    }
}