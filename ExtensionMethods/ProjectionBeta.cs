using System;
using System.Collections.Generic;

namespace ExtensionMethods
{
    internal class ProjectionBeta
    {
        private Func<TSource, TSource, int> ComparisonCriteria<TSource, TKey>(
                Func<TSource, TKey> selector,
                IComparer<TKey> sorterComparer)
                => (x, y) => sorterComparer.Compare(selector(x), selector(y));
    }
}