using System;
using System.Collections.Generic;
using System.Text;

namespace ExtensionMethods
{
    public class Projection<TSource, TKey> : IComparer<TSource>
    {
        private readonly Func<TSource, TKey> selector;
        private readonly IComparer<TKey> comparer;

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
