using System;
using System.Collections.Generic;

namespace ExtensionMethods
{
    public class Projection<TSource, TKey> : IComparer<TSource>
    {
        private readonly IComparer<TKey> comparer;
        private readonly Func<TSource, TKey> selector;

        public Projection(
            Func<TSource, TKey> selector,
            IComparer<TKey> comparer)
        {
            this.selector = selector;
            this.comparer = comparer;
        }

        public int Compare(TSource x, TSource y)
        {
            return comparer.Compare(selector(x), selector(y));
        }
    }
}